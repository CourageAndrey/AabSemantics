using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Win32;

using Inventor.Semantics;
using Inventor.Semantics.Xml;
using Inventor.Semantics.WPF;
using Inventor.Semantics.WPF.Commands;
using Inventor.Semantics.WPF.Dialogs;
using Inventor.Semantics.WPF.Localization;
using Inventor.Semantics.WPF.ViewModels;

namespace Inventor.SimpleWpfClient
{
	public partial class MainWindow : IMainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			_localizationProvider = (ObjectDataProvider) Resources["language"];
			_localizator = (Localizator) _localizationProvider.Data;

			_changeController = new ChangeController();
			_changeController.Refreshed += (sender, args) => refreshFileButtonsAndTitle();

			_viewModelFactory = new ViewModelFactory();
			_commandsFactory = new CommandsFactory();
		}

		public void Initialize(IInventorApplication application)
		{
			dockPanelMain.DataContext = _application = application;
			treeViewSemanticNetwork.Initialize(application, _changeController, _viewModelFactory, _commandsFactory);
			setModel(application.SemanticNetwork, string.Empty);
		}

		private readonly ObjectDataProvider _localizationProvider;
		private readonly Localizator _localizator;
		private IInventorApplication _application;
		private String _fileName;
		private readonly ChangeController _changeController;
		private readonly IViewModelFactory _viewModelFactory;
		private readonly ICommandsFactory _commandsFactory;

		private void selectedLanguageChanged(object sender, SelectionChangedEventArgs e)
		{
			_localizator.Change(_application.CurrentLanguage);
			_localizationProvider.Refresh();
			treeViewSemanticNetwork.Reload();
			refreshFileButtonsAndTitle();
		}

		#region Knowledgebase actions

		private void askQuestionClick(object sender, RoutedEventArgs e)
		{
			var dialog = new QuestionDialog(_application.SemanticNetwork, _application.CurrentLanguage, _viewModelFactory)
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
			treeViewSemanticNetwork.Select(entity);
		}

		#endregion

		#region Save/Load

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
					var semanticNetwork = new Semantics.SemanticNetwork(_application.CurrentLanguage)
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
				setModel(new Semantics.Test.Sample.TestSemanticNetwork(_application.CurrentLanguage).SemanticNetwork, string.Empty);
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
			treeViewSemanticNetwork.Reload();

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
			_application.SemanticNetwork.Save(_fileName);
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
