using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Win32;

using Inventor.Client.UI.Nodes;
using Inventor.Core;
using Inventor.Core.Localization;

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
			_application.KnowledgeBase = KnowledgeBase.New(_application.CurrentLanguage);
			realoadKnowledgeBaseTree();
			return _application.KnowledgeBase;
		}

		private void createTestClick(object sender, RoutedEventArgs e)
		{
			_application.KnowledgeBase = KnowledgeBase.CreateTest();
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

		private readonly QuestionProcessorRepository _questionProcessorRepository = new QuestionProcessorRepository();

		private void askQuestionClick(object sender, RoutedEventArgs e)
		{
			var dialog = new QuestionDialog(_application.KnowledgeBase, _application.CurrentLanguage)
			{
				Owner = this,
			};
			if (dialog.ShowDialog() == true)
			{
				var questionProcessor = _questionProcessorRepository.CreateQuestionProcessor(dialog.Question, _application.CurrentLanguage);
				var context = new ProcessingContext(_application.KnowledgeBase, dialog.Question, _questionProcessorRepository, _application.CurrentLanguage);
				var answer = questionProcessor.Process(context);
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
	}
}
