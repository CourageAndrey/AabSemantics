using System;
using System.Collections.Generic;
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

		private void selectedLanguageChanged(object sender, SelectionChangedEventArgs e)
		{
			_localizator.Change(_application.CurrentLanguage);
			_localizationProvider.Refresh();
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
				PerformCommand(command);
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
					PerformCommand(command);
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
					PerformCommand(command);
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
				PerformCommand(command);
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
			if (_semanticNetworkNode != null)
			{
				_semanticNetworkNode.Clear();
			}
			treeViewSemanticNetwork.Items.Add(_semanticNetworkNode = new SemanticNetworkNode(_application.SemanticNetwork, _application));
			_semanticNetworkNode.IsExpanded = true;
		}

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

		private void refreshFileButtonsAndTitle()
		{
			buttonNew.IsEnabled = buttonLoad.IsEnabled = buttonSaveAs.IsEnabled = true;
			buttonSave.IsEnabled = isChanged();

			_buttonUndo.IsEnabled = _editHistory.Count > 0 && _currentEditPointer >= 0;
			_buttonRedo.IsEnabled = _editHistory.Count > 0 && _currentEditPointer < _editHistory.Count - 1;

			string changesSign = isChanged() ? "*" : string.Empty;
			Title = $"{_fileName}{changesSign} - {_application.CurrentLanguage.Ui.MainForm.Title}";
		}

		private void buttonNew_Click(object sender, RoutedEventArgs e)
		{
			if (canProceedAfterSave())
			{
				setModel(new SemanticNetwork(_application.CurrentLanguage), string.Empty);
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
					setModel(SemanticNetwork.Load(dialog.FileName, _application.CurrentLanguage), dialog.FileName);
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
				_semanticNetworkNode.SemanticNetwork.Save(_fileName);
				_savedPointer = _currentEditPointer;
				refreshFileButtonsAndTitle();
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
				setModel(new TestSemanticNetwork(_application.CurrentLanguage).SemanticNetwork, string.Empty);
			}
		}

		private bool canProceedAfterSave()
		{
			if (!isChanged()) return true;

			var language = _application.CurrentLanguage.Ui.MainForm;
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

			_editHistory.Clear();
			_savedPointer = _currentEditPointer = -1;

			refreshFileButtonsAndTitle();
		}

		private bool saveAs()
		{
			var dialog = createSaveFileDialog();
			dialog.FileName = _fileName;
			if (dialog.ShowDialog() == true)
			{
				_semanticNetworkNode.SemanticNetwork.Save(_fileName = dialog.FileName);
				_savedPointer = _currentEditPointer;
				refreshFileButtonsAndTitle();
				return true;
			}
			else
			{
				return false;
			}
		}

		private String _fileName;
		private readonly List<IEditCommand> _editHistory = new List<IEditCommand>();
		private int _currentEditPointer = -1;
		private int _savedPointer = -1;

		private bool isChanged()
		{
			return _editHistory.Count > 0 && _currentEditPointer != _savedPointer;
		}

		public void PerformCommand(IEditCommand command)
		{
			command.Apply();

			_editHistory.RemoveRange(_currentEditPointer + 1, _editHistory.Count - _currentEditPointer - 1);

			_currentEditPointer = _editHistory.Count;
			_editHistory.Add(command);
			refreshFileButtonsAndTitle();
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
			_editHistory[_currentEditPointer].Rollback();
			_currentEditPointer--;
			refreshFileButtonsAndTitle();
		}

		private void redoMenuClick(object sender, RoutedEventArgs e)
		{
			_currentEditPointer++;
			_editHistory[_currentEditPointer].Apply();
			refreshFileButtonsAndTitle();
		}

		#endregion
	}
}
