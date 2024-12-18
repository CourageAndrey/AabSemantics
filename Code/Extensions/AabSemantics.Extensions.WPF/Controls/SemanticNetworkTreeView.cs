using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Extensions.WPF.TreeNodes;

namespace AabSemantics.Extensions.WPF.Controls
{
	public class SemanticNetworkTreeView : TreeView
	{
		public SemanticNetworkTreeView()
		{
			_contextMenu = new ContextMenu();
			_contextMenu.ContextMenuOpening += knowledgeContextMenuOpening;

			IsReadOnly = false;
		}

		private MenuItem createMenuItem(string name, string headerLocalizationKey, RoutedEventHandler onClick)
		{
			var menuItem = new MenuItem { Name = name };

			menuItem.SetBinding(HeaderedItemsControl.HeaderProperty, new Binding(headerLocalizationKey)
			{
				Mode = BindingMode.OneTime,
			});

			menuItem.Click += onClick;

			_contextMenu.Items.Add(menuItem);

			return menuItem;
		}

		#region Public API

		public void Initialize(
			IInventorApplication application,
			ChangeController changeController,
			ViewModels.IViewModelFactory viewModelFactory,
			Commands.ICommandsFactory commandsFactory)
		{
			_application = application;
			_changeController = changeController;
			_viewModelFactory = viewModelFactory;
			_commandsFactory = commandsFactory;
			_owner = (Window) application.MainForm;

			_contextMenu.DataContext = _owner.Resources["language"];

			_renameKnowledgeItem = createMenuItem("_renameKnowledgeItem", "Ui.MainForm.ContextMenuRename", renameKnowledgeClick);
			_addKnowledgeItem = createMenuItem("_addKnowledgeItem", "Ui.MainForm.ContextMenuKnowledgeAdd", addKnowledgeClick);
			_editKnowledgeItem = createMenuItem("_editKnowledgeItem", "Ui.MainForm.ContextMenuKnowledgeEdit", editKnowledgeClick);
			_deleteKnowledgeItem = createMenuItem("_deleteKnowledgeItem", "Ui.MainForm.ContextMenuKnowledgeDelete", deleteKnowledgeClick);

			Reload();
		}

		public bool IsReadOnly
		{
			get { return ContextMenu == null; }
			set { ContextMenu = !value ? _contextMenu : null; }
		}

		public void Select(IKnowledge entity)
		{
			var path = _semanticNetworkNode.Find(entity).OfType<object>().ToList();
			if (path.Count > 0)
			{
				this.ExecuteWithItem(path, item =>
				{
					item.IsSelected = true;
					item.BringIntoView();
				});
			}
		}

		public void Reload()
		{
			Items.Clear();

			Items.Add(_semanticNetworkNode = new SemanticNetworkNode(_application));
			_semanticNetworkNode.IsExpanded = true;
		}

		private SemanticNetworkNode _semanticNetworkNode;

		#endregion

		#region Context Menu

		private readonly ContextMenu _contextMenu;
		private MenuItem _renameKnowledgeItem;
		private MenuItem _addKnowledgeItem;
		private MenuItem _editKnowledgeItem;
		private MenuItem _deleteKnowledgeItem;

		private void renameKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var semanticNetworkNode = SelectedItem as SemanticNetworkNode;
			if (semanticNetworkNode == null) return;

			var editingName = semanticNetworkNode.SemanticNetwork.Name;

			var control = new LocalizedStringVariableControl();
			control.Localize(_application.CurrentLanguage);
			control.EditValue = ViewModels.LocalizedString.From(editingName);

			var dialog = new EditDialog
			{
				Owner = _owner,
				Editor = control,
				Title = editingName.GetValue(_application.CurrentLanguage),
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(_application.CurrentLanguage);

			if (dialog.ShowDialog() == true)
			{
				var command = _commandsFactory.CreateRenameCommand(control.EditValue, _semanticNetworkNode, _application);
				_changeController.Perform(command);
				semanticNetworkNode.RefreshView();
			}
		}

		private void addKnowledgeClick(object sender, RoutedEventArgs e)
		{
			Type type = null;
			var selectedItem = SelectedItem;
			if (selectedItem is ConceptNode || selectedItem is SemanticNetworkConceptsNode)
			{
				type = typeof(Concepts.Concept);
			}
			else if (selectedItem is StatementNode || selectedItem is SemanticNetworkStatementsNode)
			{
				var statementTypesDialog = new SelectStatementTypeDialog
				{
					Owner = _owner,
				};
				statementTypesDialog.Initialize(_application.CurrentLanguage, _application.SemanticNetwork);
				if (statementTypesDialog.ShowDialog() == true)
				{
					type = statementTypesDialog.SelectedType;
				}
			}
			if (type == null) return;

			IKnowledgeViewModel viewModel = _viewModelFactory.CreateByCoreType(type, _application.CurrentLanguage);
			var editDialog = viewModel.CreateEditDialog(_owner, _application.SemanticNetwork, _application.CurrentLanguage);

			if (editDialog.ShowDialog() == true)
			{
				var command = _commandsFactory.CreateAddCommand(viewModel, _semanticNetworkNode, _application);
				if (command != null)
				{
					_changeController.Perform(command);
				}
			}
		}

		private void editKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var selectedNode = SelectedItem as ExtendedTreeNode;
			if (selectedNode == null) return;
			var viewModel = _viewModelFactory.CreateByTreeNode(selectedNode, _application.CurrentLanguage);
			if (viewModel == null) return;

			var editDialog = viewModel.CreateEditDialog(_owner, _application.SemanticNetwork, _application.CurrentLanguage);

			if (editDialog.ShowDialog() == true)
			{
				var command = _commandsFactory.CreateEditCommand(viewModel, _semanticNetworkNode, _application, _viewModelFactory);
				if (command != null)
				{
					_changeController.Perform(command);
				}
				selectedNode.RefreshView();
			}
		}

		private void deleteKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var extendedNode = SelectedItem as ExtendedTreeNode;
			var command = _commandsFactory.CreateDeleteCommand(extendedNode, _semanticNetworkNode, _application);
			if (command != null)
			{
				_changeController.Perform(command);
			}
		}

		private void knowledgeContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			var selectedItem = SelectedItem;
			bool isSemanticNetworkNode = selectedItem is SemanticNetworkNode;
			//bool isConceptsNode = selectedItem is SemanticNetworkConceptsNode;
			//bool isStatementsNode = selectedItem is SemanticNetworkStatementsNode;
			bool isConceptNode = selectedItem is ConceptNode;
			var statementNode = selectedItem as StatementNode;
			_renameKnowledgeItem.Visibility = isSemanticNetworkNode
				? Visibility.Visible
				: Visibility.Collapsed;
			_addKnowledgeItem.Visibility = !isSemanticNetworkNode
				? Visibility.Visible
				: Visibility.Collapsed;
			_editKnowledgeItem.Visibility = _deleteKnowledgeItem.Visibility = isConceptNode || statementNode?.Statement.Context.IsSystem == false
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		private IInventorApplication _application;
		private ChangeController _changeController;
		private ViewModels.IViewModelFactory _viewModelFactory;
		private Commands.ICommandsFactory _commandsFactory;
		private Window _owner;

		#endregion
	}
}
