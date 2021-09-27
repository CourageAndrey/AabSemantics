using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Win32;

using Inventor.Client.Dialogs;
using Inventor.Client.Localization;
using Inventor.Client.TreeNodes;
using Inventor.Core;
using Inventor.Core.Xml;

namespace Inventor.Client
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			_localizationProvider = (ObjectDataProvider) Resources["language"];
			_localizator = (Localizator) _localizationProvider.Data;

			_changeController = new ChangeController();
			_changeController.Refreshed += (sender, args) => refreshFileButtonsAndTitle();
		}

		internal void Initialize(InventorApplication application)
		{
			dockPanelMain.DataContext = _application = application;
			setModel(application.SemanticNetwork, string.Empty);
		}

		private readonly ObjectDataProvider _localizationProvider;
		private readonly Localizator _localizator;
		private InventorApplication _application;
		private SemanticNetworkNode _semanticNetworkNode;
		private String _fileName;
		private readonly ChangeController _changeController;

		private void selectedLanguageChanged(object sender, SelectionChangedEventArgs e)
		{
			_localizator.Change(_application.CurrentLanguage);
			_localizationProvider.Refresh();
			reloadSemanticNetworkTree();
			refreshFileButtonsAndTitle();
		}

		#region Knowledgebase actions

		private void askQuestionClick(object sender, RoutedEventArgs e)
		{
			var dialog = new QuestionDialog(_application.SemanticNetwork, _application.CurrentLanguage)
			{
				Owner = this,
			};
			if (dialog.ShowDialog() == true)
			{
				var question = dialog.Question.BuildQuestion();
				var answer = question.Ask(_application.SemanticNetwork.Context, _application.CurrentLanguage);
				answer.Display(this, _application.CurrentLanguage, knowledgeObjectPicked);
			}
		}

		private void showAllKnowledgeClick(object sender, RoutedEventArgs e)
		{
			_application.SemanticNetwork.DisplayRulesDescription(this, _application.CurrentLanguage, knowledgeObjectPicked);
		}

		private void checkKnowledgeClick(object sender, RoutedEventArgs e)
		{
			_application.SemanticNetwork.DisplayConsistencyCheckResult(this, _application.CurrentLanguage, knowledgeObjectPicked);
		}

		private void modulesClick(object sender, RoutedEventArgs e)
		{
			var selectModulesDialog = new SelectModulesDialog
			{
				Owner = this,
				IsReadOnly = true,
			};
			selectModulesDialog.Initialize(_application.CurrentLanguage);
			selectModulesDialog.SelectedModules = _application.SemanticNetwork.Modules.Values;
			selectModulesDialog.ShowDialog();
		}

		private void knowledgeObjectPicked(IKnowledge entity)
		{
			var path = _semanticNetworkNode.Find(entity).OfType<object>().ToList();
			if (path.Count > 0)
			{
				treeViewSemanticNetwork.ExecuteWithItem(path, item =>
				{
					item.IsSelected = true;
					item.BringIntoView();
				});
			}
		}

		#endregion

		#region Knowledge Tree Menu

		private void renameClick(object sender, RoutedEventArgs e)
		{
			var semanticNetworkNode = treeViewSemanticNetwork.SelectedItem as SemanticNetworkNode;
			if (semanticNetworkNode == null) return;

			var editingName = semanticNetworkNode.SemanticNetwork.Name;

			var control = new Controls.LocalizedStringVariableControl();
			control.Localize(_application.CurrentLanguage);
			control.EditValue = ViewModels.LocalizedString.From(editingName);

			var dialog = new EditDialog
			{
				Owner = this,
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
				var command = control.EditValue.CreateRenameCommand(_semanticNetworkNode, _application);
				_changeController.Perform(command);
				semanticNetworkNode.RefreshView();
			}
		}

		private void addKnowledgeClick(object sender, RoutedEventArgs e)
		{
			Type type = null;
			var selectedItem = treeViewSemanticNetwork.SelectedItem;
			if (selectedItem is ConceptNode || selectedItem is SemanticNetworkConceptsNode)
			{
				type = typeof(Core.Concepts.Concept);
			}
			else if (selectedItem is StatementNode || selectedItem is SemanticNetworkStatementsNode)
			{
				var statementTypesDialog = new SelectStatementTypeDialog
				{
					Owner = this,
				};
				statementTypesDialog.Initialize(_application.CurrentLanguage, _application.SemanticNetwork);
				if (statementTypesDialog.ShowDialog() == true)
				{
					type = statementTypesDialog.SelectedType;
				}
			}
			if (type == null) return;

			IKnowledgeViewModel viewModel = ViewModels.Factory.CreateByCoreType(type, _application.CurrentLanguage);
			var editDialog = viewModel.CreateEditDialog(this, _application.SemanticNetwork, _application.CurrentLanguage);

			if (editDialog.ShowDialog() == true)
			{
				var command = viewModel.CreateAddCommand(_semanticNetworkNode, _application);
				if (command != null)
				{
					_changeController.Perform(command);
				}
			}
		}

		private void editKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var selectedNode = treeViewSemanticNetwork.SelectedItem as ExtendedTreeNode;
			if (selectedNode == null) return;
			var viewModel = ViewModels.Factory.CreateByTreeNode(selectedNode, _application.CurrentLanguage);
			if (viewModel == null) return;

			var editDialog = viewModel.CreateEditDialog(this, _application.SemanticNetwork, _application.CurrentLanguage);

			if (editDialog.ShowDialog() == true)
			{
				var command = viewModel.CreateEditCommand(_semanticNetworkNode, _application);
				if (command != null)
				{
					_changeController.Perform(command);
				}
				selectedNode.RefreshView();
			}
		}

		private void deleteKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var extendedNode = treeViewSemanticNetwork.SelectedItem as ExtendedTreeNode;
			var command = extendedNode.CreateDeleteCommand(_semanticNetworkNode, _application);
			if (command != null)
			{
				_changeController.Perform(command);
			}
		}

		private void knowledgeContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			var selectedItem = treeViewSemanticNetwork.SelectedItem;
			bool isSemanticNetworkNode = selectedItem is SemanticNetworkNode;
			//bool isConceptsNode = selectedItem is SemanticNetworkConceptsNode;
			//bool isStatementsNode = selectedItem is SemanticNetworkStatementsNode;
			bool isConceptNode = selectedItem is ConceptNode;
			var statementNode = selectedItem as StatementNode;
			_rename.Visibility = isSemanticNetworkNode
				? Visibility.Visible
				: Visibility.Collapsed;
			_addKnowledgeItem.Visibility = !isSemanticNetworkNode
				? Visibility.Visible
				: Visibility.Collapsed;
			_editKnowledgeItem.Visibility = _deleteKnowledgeItem.Visibility = isConceptNode || statementNode?.Statement.Context.IsSystem == false
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		#endregion

		#region Save/Load

		private void reloadSemanticNetworkTree()
		{
			treeViewSemanticNetwork.Items.Clear();
			treeViewSemanticNetwork.Items.Add(_semanticNetworkNode = new SemanticNetworkNode(_application.SemanticNetwork, _application));
			_semanticNetworkNode.IsExpanded = true;
		}

		private OpenFileDialog createOpenFileDialog()
		{
			var language = _application.CurrentLanguage;
			return new OpenFileDialog
			{
				DefaultExt = ".xml",
				Filter = language.GetExtension<IWpfUiModule>().Misc.DialogKbFileFilter,
				RestoreDirectory = true,
				Title = language.GetExtension<IWpfUiModule>().Misc.DialogKbOpenTitle,
			};
		}

		private SaveFileDialog createSaveFileDialog()
		{
			var language = _application.CurrentLanguage;
			return new SaveFileDialog
			{
				DefaultExt = ".xml",
				Filter = language.GetExtension<IWpfUiModule>().Misc.DialogKbFileFilter,
				RestoreDirectory = true,
				Title = language.GetExtension<IWpfUiModule>().Misc.DialogKbSaveTitle,
			};
		}

		private void refreshFileButtonsAndTitle()
		{
			buttonNew.IsEnabled = buttonLoad.IsEnabled = buttonSaveAs.IsEnabled = true;
			buttonSave.IsEnabled = _changeController.HasChanges;

			_buttonUndo.IsEnabled = _changeController.CanUndo;
			_buttonRedo.IsEnabled = _changeController.CanRedo;

			string changesSign = _changeController.HasChanges ? "*" : string.Empty;
			Title = $"{_fileName}{changesSign} - {_application.CurrentLanguage.GetExtension<IWpfUiModule>().Ui.MainForm.Title}";
		}

		private void buttonNew_Click(object sender, RoutedEventArgs e)
		{
			if (canProceedAfterSave())
			{
				var selectModulesDialog = new SelectModulesDialog
				{
					Owner = this,
				};
				selectModulesDialog.Initialize(_application.CurrentLanguage);
				if (selectModulesDialog.ShowDialog() == true)
				{
					var semanticNetwork = new Core.SemanticNetwork(_application.CurrentLanguage)
						.WithModules(selectModulesDialog.SelectedModules);
					setModel(semanticNetwork, string.Empty);
				}
			}
		}

		private void buttonLoad_Click(object sender, RoutedEventArgs e)
		{
			if (canProceedAfterSave())
			{
				var dialog = createOpenFileDialog();
				dialog.FileName = _fileName;
				if (dialog.ShowDialog() == true)
				{
					setModel(dialog.FileName.LoadSemanticNetworkFromXml(_application.CurrentLanguage), dialog.FileName);
				}
			}
		}

		private void buttonSave_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(_fileName))
			{
				saveAs();
			}
			else
			{
				save();
			}
		}

		private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
		{
			saveAs();
		}

		private void buttonCreateTest_Click(object sender, RoutedEventArgs e)
		{
			if (canProceedAfterSave())
			{
				setModel(new Test.Sample.TestSemanticNetwork(_application.CurrentLanguage).SemanticNetwork, string.Empty);
			}
		}

		private bool canProceedAfterSave()
		{
			if (!_changeController.HasChanges) return true;

			var language = _application.CurrentLanguage.GetExtension<IWpfUiModule>().Ui.MainForm;
			var promptResult = MessageBox.Show(
				language.SavePromt,
				language.SaveTitle,
				MessageBoxButton.YesNoCancel,
				MessageBoxImage.Question);

			return	promptResult == MessageBoxResult.No ||
					(promptResult == MessageBoxResult.Yes && saveAs());
		}

		private void setModel(ISemanticNetwork semanticNetwork, string fileName)
		{
			_application.SemanticNetwork = semanticNetwork;
			reloadSemanticNetworkTree();

			_fileName = fileName;

			_changeController.ClearHistory();
		}

		private bool saveAs()
		{
			var dialog = createSaveFileDialog();
			dialog.FileName = _fileName;
			if (dialog.ShowDialog() == true)
			{
				_fileName = dialog.FileName;
				save();
				return true;
			}
			else
			{
				return false;
			}
		}

		private void save()
		{
			_semanticNetworkNode.SemanticNetwork.Save(_fileName);
			_changeController.SaveHistory();
		}

		private void onWindowClosing(object sender, CancelEventArgs e)
		{
			if (!canProceedAfterSave())
			{
				e.Cancel = true;
			}
		}

		private void undoMenuClick(object sender, RoutedEventArgs e)
		{
			_changeController.Undo();
		}

		private void redoMenuClick(object sender, RoutedEventArgs e)
		{
			_changeController.Redo();
		}

		#endregion
	}
}
