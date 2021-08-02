using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Win32;

using Inventor.Client.Dialogs;
using Inventor.Client.Localization;
using Inventor.Client.TreeNodes;
using Inventor.Core;
using Inventor.Core.Base;

namespace Inventor.Client
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			_localizationProvider = (ObjectDataProvider) Resources["language"];
			_localizator = (Localizator) _localizationProvider.Data;

			SetBinding(TitleProperty, new Binding("Ui.MainForm.Title")
			{
				Source = _localizationProvider,
				Mode = BindingMode.OneTime,
			});
		}

		internal void Initialize(InventorApplication application)
		{
			dockPanelMain.DataContext = _application = application;
			ChangeEntity(application.SemanticNetwork ?? createNew());
			reloadSemanticNetworkTree();
		}

		private readonly ObjectDataProvider _localizationProvider;
		private readonly Localizator _localizator;
		private InventorApplication _application;
		private SemanticNetworkNode _semanticNetworkNode;

		private void selectedLanguageChanged(object sender, SelectionChangedEventArgs e)
		{
			_localizator.Change(_application.CurrentLanguage);
			_localizationProvider.Refresh();
		}

		#region Main menu

		private void saveToFile(IChangeable changeable, string fileName)
		{
			_semanticNetworkNode.SemanticNetwork.Save(fileName);
		}

		private IChangeable loadFromFile(string fileName)
		{
			_application.SemanticNetwork = SemanticNetwork.Load(fileName, _application.CurrentLanguage);
			reloadSemanticNetworkTree();
			return _application.SemanticNetwork;
		}

		private IChangeable createNew()
		{
			_application.SemanticNetwork = new SemanticNetwork(_application.CurrentLanguage);
			reloadSemanticNetworkTree();
			return _application.SemanticNetwork;
		}

		private void createTestClick(object sender, RoutedEventArgs e)
		{
			_application.SemanticNetwork = new TestSemanticNetwork(_application.CurrentLanguage).SemanticNetwork;
			reloadSemanticNetworkTree();
			ChangeEntity(_application.SemanticNetwork);
		}

		private void reloadSemanticNetworkTree()
		{
			treeViewSemanticNetwork.Items.Clear();
			if (_semanticNetworkNode != null)
			{
				_semanticNetworkNode.Clear();
			}
			treeViewSemanticNetwork.Items.Add(_semanticNetworkNode = new SemanticNetworkNode(_application.SemanticNetwork, _application));
			_semanticNetworkNode.IsExpanded = true;
		}

		#endregion

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
				var answer = question.Ask(_application.SemanticNetwork.Context);

				var processingResult = answer.Description;
				if (answer.Explanation.Statements.Count > 0)
				{
					processingResult.Add(new FormattedLine(() => string.Empty, new Dictionary<string, INamed>()));
					processingResult.Add(new FormattedLine(() => _application.CurrentLanguage.Answers.Explanation, new Dictionary<string, INamed>()));

					foreach (var statement in answer.Explanation.Statements)
					{
						processingResult.Add(statement.DescribeTrue(_application.CurrentLanguage));
					}
				}

				new FormattedTextDialog(
					_application.CurrentLanguage,
					processingResult,
					knowledgeObjectPicked)
				{
					Owner = this,
					Title = _application.CurrentLanguage.Misc.Answer,
				}.Show();
			}
		}

		private void showAllKnowledgeClick(object sender, RoutedEventArgs e)
		{
			new FormattedTextDialog(
				_application.CurrentLanguage,
				_application.SemanticNetwork.DescribeRules(_application.CurrentLanguage),
				knowledgeObjectPicked)
			{
				Owner = this,
				Title = _application.CurrentLanguage.Misc.Rules,
			}.Show();
		}

		private void checkKnowledgeClick(object sender, RoutedEventArgs e)
		{
			new FormattedTextDialog(
				_application.CurrentLanguage,
				_application.SemanticNetwork.CheckConsistensy(_application.CurrentLanguage),
				knowledgeObjectPicked)
			{
				Owner = this,
				Title = _application.CurrentLanguage.Consistency.CheckResult,
			}.Show();
		}

		#endregion

		#region Object picking

		private void knowledgeObjectPicked(INamed entity)
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
				var command = control.EditValue.CreateRenameCommand(_application.SemanticNetwork);
				command.Apply();
				semanticNetworkNode.RefreshView();
			}
		}

		private void addKnowledgeClick(object sender, RoutedEventArgs e)
		{
			Type type = null;
			var selectedItem = treeViewSemanticNetwork.SelectedItem;
			if (selectedItem is ConceptNode || selectedItem is SemanticNetworkConceptsNode)
			{
				type = typeof(Concept);
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
				var command = viewModel.CreateAddCommand(_application.SemanticNetwork);
				if (command != null)
				{
					command.Apply();
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
				var command = viewModel.CreateEditCommand(_application.SemanticNetwork, _application.CurrentLanguage);
				if (command != null)
				{
					command.Apply();
				}
				selectedNode.RefreshView();
			}
		}

		private void deleteKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var extendedNode = treeViewSemanticNetwork.SelectedItem as ExtendedTreeNode;
			var command = extendedNode.CreateDeleteCommand(_application.SemanticNetwork);
			if (command != null)
			{
				command.Apply();
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

		private OpenFileDialog createOpenFileDialog()
		{
			var language = _application.CurrentLanguage;
			return new OpenFileDialog
			{
				DefaultExt = ".xml",
				Filter = language.Misc.DialogKbFileFilter,
				RestoreDirectory = true,
				Title = language.Misc.DialogKbOpenTitle,
			};
		}

		private SaveFileDialog createSaveFileDialog()
		{
			var language = _application.CurrentLanguage;
			return new SaveFileDialog
			{
				DefaultExt = ".xml",
				Filter = language.Misc.DialogKbFileFilter,
				RestoreDirectory = true,
				Title = language.Misc.DialogKbSaveTitle,
			};
		}

		private void update()
		{
			buttonNew.IsEnabled = buttonLoad.IsEnabled = buttonSaveAs.IsEnabled = true;
			buttonSave.IsEnabled = _isChanged;
		}

		public void ChangeEntity(IChangeable newEntity, String newFileName = null)
		{
			if (newEntity == null) throw new ArgumentNullException(nameof(newEntity));

			if (_entity != null)
			{
				_entity.Changed -= entityModified;
			}
			_entity = newEntity;
			_entity.Changed += entityModified;

			_isChanged = false;
			_fileName = newFileName;
			update();
		}

		private void buttonNew_Click(object sender, RoutedEventArgs e)
		{
			var semanticNetwork = new SemanticNetwork(_application.CurrentLanguage);
			_application.SemanticNetwork = semanticNetwork;
			reloadSemanticNetworkTree();
			ChangeEntity(semanticNetwork);
		}

		private void buttonLoad_Click(object sender, RoutedEventArgs e)
		{
			var dialog = createOpenFileDialog();
			dialog.FileName = _fileName;
			if (dialog.ShowDialog() == true)
			{
				ChangeEntity(loadFromFile(dialog.FileName), dialog.FileName);
			}
		}

		private void buttonSave_Click(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(_fileName))
			{
				buttonSaveAs_Click(sender, e);
			}
			else
			{
				saveToFile(_entity, _fileName);
				_isChanged = false;
				update();
			}
		}

		private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
		{
			var dialog = createSaveFileDialog();
			dialog.FileName = _fileName;
			if (dialog.ShowDialog() == true)
			{
				saveToFile(_entity, _fileName = dialog.FileName);
				_isChanged = false;
				update();
			}
		}

		private void entityModified(Object sender, EventArgs eventArgs)
		{
			_isChanged = true;
			update();
		}

		private IChangeable _entity;
		private Boolean _isChanged;
		private String _fileName;

		#endregion
	}
}
