using System.Linq;
using System.Windows;
using System.Windows.Data;

using Inventor.Client.UI.Nodes;
using Inventor.Core;
using Inventor.Core.Localization;

using Sef.Program;

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

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			dockPanelMain.DataContext = InventorApplication.Singleton;
			_saveLoadController = new SaveLoadController(buttonNew, buttonLoad, buttonSave, buttonSaveAs,
				createNew, loadFromFile, saveToFile,
				() => KnowledgeBase.CreateOpenFileDialog(LanguageEx.CurrentEx), () => KnowledgeBase.CreateSaveFileDialog(LanguageEx.CurrentEx),
				(s, a) => { },
				InventorApplication.Singleton.KnowledgeBase);
			realoadKnowledgeBaseTree();
		}

		private KnowledgeBaseNode _knowledgeBaseNode;
		private SaveLoadController _saveLoadController;

		#region Main menu

		private void saveToFile(IChangeable changeable, string fileName)
		{
			_knowledgeBaseNode.KnowledgeBase.Save(fileName);
		}

		private IChangeable loadFromFile(string fileName)
		{
			InventorApplication.Singleton.KnowledgeBase = KnowledgeBase.Load(fileName);
			realoadKnowledgeBaseTree();
			return InventorApplication.Singleton.KnowledgeBase;
		}

		private IChangeable createNew()
		{
			InventorApplication.Singleton.KnowledgeBase = KnowledgeBase.New(LanguageEx.CurrentEx);
			realoadKnowledgeBaseTree();
			return InventorApplication.Singleton.KnowledgeBase;
		}

		private void createTestClick(object sender, RoutedEventArgs e)
		{
			InventorApplication.Singleton.KnowledgeBase = KnowledgeBase.CreateTest();
			realoadKnowledgeBaseTree();
			_saveLoadController.ChangeEntity(InventorApplication.Singleton.KnowledgeBase);
		}

		private void realoadKnowledgeBaseTree()
		{
			treeViewKnowledgeBase.Items.Clear();
			if (_knowledgeBaseNode != null)
			{
				_knowledgeBaseNode.Clear();
			}
			treeViewKnowledgeBase.Items.Add(_knowledgeBaseNode = new KnowledgeBaseNode(InventorApplication.Singleton.KnowledgeBase));
			_knowledgeBaseNode.IsExpanded = true;
		}

		#endregion

		#region Knowledgebase actions

		private void askQuestionClick(object sender, RoutedEventArgs e)
		{
			var dialog = new QuestionDialog(InventorApplication.Singleton.KnowledgeBase, LanguageEx.CurrentEx)
			{
				Owner = this,
			};
			if (dialog.ShowDialog() == true)
			{
				new FormattedTextDialog(
					QuestionProcessor.Process(InventorApplication.Singleton.KnowledgeBase, dialog.Question, LanguageEx.CurrentEx),
					knowledgeObjectPicked)
				{
					Owner = this,
					Title = LanguageEx.CurrentEx.Misc.Answer,
				}.Show();
			}
		}

		private void showAllKnowledgeClick(object sender, RoutedEventArgs e)
		{
			new FormattedTextDialog(
				InventorApplication.Singleton.KnowledgeBase.DescribeRules(LanguageEx.CurrentEx),
				knowledgeObjectPicked)
			{
				Owner = this,
				Title = LanguageEx.CurrentEx.Misc.Rules,
			}.Show();
		}

		private void checkKnowledgeClick(object sender, RoutedEventArgs e)
		{
			new FormattedTextDialog(
				InventorApplication.Singleton.KnowledgeBase.CheckConsistensy(LanguageEx.CurrentEx),
				knowledgeObjectPicked)
			{
				Owner = this,
				Title = LanguageEx.CurrentEx.Misc.CheckResult,
			}.Show();
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
