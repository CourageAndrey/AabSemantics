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
			_saveLoadController = new SaveLoadController(buttonNew, buttonLoad, buttonSave, buttonSaveAs,
				createNew, loadFromFile, saveToFile,
				() => createOpenFileDialog(_application.CurrentLanguage), () => createSaveFileDialog(_application.CurrentLanguage),
				(s, a) => { },
				application.SemanticNetwork);
			reloadSemanticNetworkTree();
		}

		private readonly ObjectDataProvider _localizationProvider;
		private readonly Localizator _localizator;
		private InventorApplication _application;
		private SemanticNetworkNode _semanticNetworkNode;
		private SaveLoadController _saveLoadController;

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
			_saveLoadController.ChangeEntity(_application.SemanticNetwork);
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

		private static OpenFileDialog createOpenFileDialog(ILanguage language)
		{
			return new OpenFileDialog
			{
				DefaultExt = ".xml",
				Filter = language.Misc.DialogKbFileFilter,
				RestoreDirectory = true,
				Title = language.Misc.DialogKbOpenTitle,
			};
		}

		private static SaveFileDialog createSaveFileDialog(ILanguage language)
		{
			return new SaveFileDialog
			{
				DefaultExt = ".xml",
				Filter = language.Misc.DialogKbFileFilter,
				RestoreDirectory = true,
				Title = language.Misc.DialogKbSaveTitle,
			};
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
				viewModel.ApplyCreate(_application.SemanticNetwork);
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
				viewModel.ApplyUpdate();
				selectedNode.RefreshView();
			}
		}

		private void deleteKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var conceptNode = treeViewSemanticNetwork.SelectedItem as ConceptNode;
			if (conceptNode != null)
			{
				_application.SemanticNetwork.Concepts.Remove(conceptNode.Concept);
				return;
			}

			var statementNode = treeViewSemanticNetwork.SelectedItem as StatementNode;
			if (statementNode != null)
			{
				_application.SemanticNetwork.Statements.Remove(statementNode.Statement);
				return;
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
			_addKnowledgeItem.Visibility = !isSemanticNetworkNode
				? Visibility.Visible
				: Visibility.Collapsed;
			_editKnowledgeItem.Visibility = _deleteKnowledgeItem.Visibility = isConceptNode || statementNode?.Statement.Context.IsSystem == false
					? Visibility.Visible
					: Visibility.Collapsed;
		}

		#endregion
	}
}
