using System.Linq;
using System.Windows;
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

			SetBinding(TitleProperty, new Binding("Ui.MainForm.Title")
			{
				Source = Resources["language"],
				Mode = BindingMode.OneTime,
			});
		}

		internal void Initialize(InventorApplication application)
		{
			dockPanelMain.DataContext = _application = application;
			_saveLoadController = new SaveLoadController(buttonNew, buttonLoad, buttonSave, buttonSaveAs,
				createNew, loadFromFile, saveToFile,
				() => createOpenFileDialog(Core.Localization.Language.Current), () => createSaveFileDialog(Core.Localization.Language.Current),
				(s, a) => { },
				application.KnowledgeBase);
			realoadKnowledgeBaseTree();
		}

		private InventorApplication _application;
		private KnowledgeBaseNode _knowledgeBaseNode;
		private SaveLoadController _saveLoadController;

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
			_application.KnowledgeBase = KnowledgeBase.New(Core.Localization.Language.Current);
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
			treeViewKnowledgeBase.Items.Add(_knowledgeBaseNode = new KnowledgeBaseNode(_application.KnowledgeBase));
			_knowledgeBaseNode.IsExpanded = true;
		}

		#endregion

		#region Knowledgebase actions

		private void askQuestionClick(object sender, RoutedEventArgs e)
		{
			var dialog = new QuestionDialog(_application.KnowledgeBase, Core.Localization.Language.Current)
			{
				Owner = this,
			};
			if (dialog.ShowDialog() == true)
			{
				new FormattedTextDialog(
					QuestionProcessor.Process(_application.KnowledgeBase, dialog.Question, Core.Localization.Language.Current),
					knowledgeObjectPicked)
				{
					Owner = this,
					Title = Core.Localization.Language.Current.Misc.Answer,
				}.Show();
			}
		}

		private void showAllKnowledgeClick(object sender, RoutedEventArgs e)
		{
			new FormattedTextDialog(
				_application.KnowledgeBase.DescribeRules(Core.Localization.Language.Current),
				knowledgeObjectPicked)
			{
				Owner = this,
				Title = Core.Localization.Language.Current.Misc.Rules,
			}.Show();
		}

		private void checkKnowledgeClick(object sender, RoutedEventArgs e)
		{
			new FormattedTextDialog(
				_application.KnowledgeBase.CheckConsistensy(Core.Localization.Language.Current),
				knowledgeObjectPicked)
			{
				Owner = this,
				Title = Core.Localization.Language.Current.Misc.CheckResult,
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
