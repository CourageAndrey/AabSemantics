using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Win32;

using Inventor.Client.TreeNodes;
using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Client.UI
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
				application.KnowledgeBase);
			realoadKnowledgeBaseTree();
		}

		private readonly ObjectDataProvider _localizationProvider;
		private readonly Localizator _localizator;
		private InventorApplication _application;
		private KnowledgeBaseNode _knowledgeBaseNode;
		private SaveLoadController _saveLoadController;

		private void selectedLanguageChanged(object sender, SelectionChangedEventArgs e)
		{
			_localizator.Change(_application.CurrentLanguage);
			_localizationProvider.Refresh();
		}

		#region Main menu

		private void saveToFile(IChangeable changeable, string fileName)
		{
			_knowledgeBaseNode.KnowledgeBase.Save(fileName);
		}

		private IChangeable loadFromFile(string fileName)
		{
			_application.KnowledgeBase = KnowledgeBase.Load(fileName);
			realoadKnowledgeBaseTree();
			return _application.KnowledgeBase;
		}

		private IChangeable createNew()
		{
			_application.KnowledgeBase = new KnowledgeBase(_application.CurrentLanguage);
			realoadKnowledgeBaseTree();
			return _application.KnowledgeBase;
		}

		private void createTestClick(object sender, RoutedEventArgs e)
		{
			_application.KnowledgeBase = KnowledgeBase.CreateTest(_application.CurrentLanguage);
			realoadKnowledgeBaseTree();
			_saveLoadController.ChangeEntity(_application.KnowledgeBase);
		}

		private void realoadKnowledgeBaseTree()
		{
			treeViewKnowledgeBase.Items.Clear();
			if (_knowledgeBaseNode != null)
			{
				_knowledgeBaseNode.Clear();
			}
			treeViewKnowledgeBase.Items.Add(_knowledgeBaseNode = new KnowledgeBaseNode(_application.KnowledgeBase, _application));
			_knowledgeBaseNode.IsExpanded = true;
		}

		#endregion

		#region Knowledgebase actions

		private void askQuestionClick(object sender, RoutedEventArgs e)
		{
			var dialog = new QuestionDialog(_application.KnowledgeBase, _application.CurrentLanguage)
			{
				Owner = this,
			};
			if (dialog.ShowDialog() == true)
			{
				var answer = dialog.Question.Ask(_application.KnowledgeBase.Context);
				new FormattedTextDialog(
						_application.CurrentLanguage,
						answer.Description,
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
				_application.KnowledgeBase.DescribeRules(_application.CurrentLanguage),
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
				_application.KnowledgeBase.CheckConsistensy(_application.CurrentLanguage),
				knowledgeObjectPicked)
			{
				Owner = this,
				Title = _application.CurrentLanguage.Misc.CheckResult,
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
			var path = _knowledgeBaseNode.Find(entity).OfType<object>().ToList();
			if (path.Count > 0)
			{
				treeViewKnowledgeBase.ExecuteWithItem(path, item =>
				{
					item.IsSelected = true;
					item.BringIntoView();
				});
			}
		}

		#endregion

		#region Knowledge Tree Menu

		private static IViewModel viewModelFactory(Type type)
		{
			if (type == typeof (Concept))
			{
				return new ViewModels.Concept();
			}
			else if (type == typeof(ConsistsOfStatement))
			{
				return new ViewModels.ConsistsOfStatement();
			}
			else if (type == typeof(GroupStatement))
			{
				return new ViewModels.GroupStatement();
			}
			else if (type == typeof(HasSignStatement))
			{
				return new ViewModels.HasSignStatement();
			}
			else if (type == typeof(IsStatement))
			{
				return new ViewModels.IsStatement();
			}
			else if (type == typeof(SignValueStatement))
			{
				return new ViewModels.SignValueStatement();
			}
			else
			{
				throw new NotSupportedException(type.FullName);
			}
		}

		private static IViewModel viewModelFactory(ExtendedTreeNode treeNode)
		{
			var conceptNode = treeNode as ConceptNode;
			var statementNode = treeNode as StatementNode;

			if (conceptNode != null)
			{
				return new ViewModels.Concept(conceptNode.Concept as Concept);
			}
			else if (statementNode != null)
			{
				if (statementNode.Statement is ConsistsOfStatement)
				{
					return new ViewModels.ConsistsOfStatement(statementNode.Statement as ConsistsOfStatement);
				}
				else if (statementNode.Statement is GroupStatement)
				{
					return new ViewModels.GroupStatement(statementNode.Statement as GroupStatement);
				}
				else if (statementNode.Statement is HasSignStatement)
				{
					return new ViewModels.HasSignStatement(statementNode.Statement as HasSignStatement);
				}
				else if (statementNode.Statement is IsStatement)
				{
					return new ViewModels.IsStatement(statementNode.Statement as IsStatement);
				}
				else if (statementNode.Statement is SignValueStatement)
				{
					return new ViewModels.SignValueStatement(statementNode.Statement as SignValueStatement);
				}
			}

			throw new NotSupportedException(treeNode.GetType().FullName);
		}
		

		private void addKnowledgeClick(object sender, RoutedEventArgs e)
		{
			Type type = null;
			var selectedItem = treeViewKnowledgeBase.SelectedItem;
			if (selectedItem is ConceptNode || selectedItem is KnowledgeBaseConceptsNode)
			{
				type = typeof(Concept);
			}
			else if (selectedItem is StatementNode || selectedItem is KnowledgeBaseStatementsNode)
			{
				var statementTypesDialog = new SelectStatementTypeDialog
				{
					Owner = this,
				};
				statementTypesDialog.Initialize(_application.CurrentLanguage);
				if (statementTypesDialog.ShowDialog() == true)
				{
					type = statementTypesDialog.SelectedType;
				}
			}
			if (type == null) return;

			IViewModel viewModel = viewModelFactory(type);
			var editDialog = viewModel.CreateEditDialog(this, _application.KnowledgeBase, _application.CurrentLanguage);

			if (editDialog.ShowDialog() == true)
			{
				viewModel.ApplyCreate(_application.KnowledgeBase);
			}
		}

		private void editKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var selectedNode = treeViewKnowledgeBase.SelectedItem as ExtendedTreeNode;
			if (selectedNode == null) return;
			var viewModel = viewModelFactory(selectedNode);
			if (viewModel == null) return;

			var editDialog = viewModel.CreateEditDialog(this, _application.KnowledgeBase, _application.CurrentLanguage);

			if (editDialog.ShowDialog() == true)
			{
				viewModel.ApplyUpdate();
				selectedNode.RefreshView();
			}
		}

		private void deleteKnowledgeClick(object sender, RoutedEventArgs e)
		{
			var conceptNode = treeViewKnowledgeBase.SelectedItem as ConceptNode;
			if (conceptNode != null)
			{
				_application.KnowledgeBase.Concepts.Remove(conceptNode.Concept);
				return;
			}

			var statementNode = treeViewKnowledgeBase.SelectedItem as StatementNode;
			if (statementNode != null)
			{
				_application.KnowledgeBase.Statements.Remove(statementNode.Statement);
				return;
			}
		}

		private void knowledgeContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			var selectedItem = treeViewKnowledgeBase.SelectedItem;
			bool isKnowledgeBaseNode = selectedItem is KnowledgeBaseNode;
			//bool isConceptsNode = selectedItem is KnowledgeBaseConceptsNode;
			//bool isStatementsNode = selectedItem is KnowledgeBaseStatementsNode;
			bool isConceptNode = selectedItem is ConceptNode;
			var statementNode = selectedItem as StatementNode;
			_addKnowledgeItem.Visibility = !isKnowledgeBaseNode
				? Visibility.Visible
				: Visibility.Collapsed;
			_editKnowledgeItem.Visibility = _deleteKnowledgeItem.Visibility = isConceptNode || statementNode?.Statement.Context.IsSystem == false
					? Visibility.Visible
					: Visibility.Collapsed;
		}

		#endregion
	}
}
