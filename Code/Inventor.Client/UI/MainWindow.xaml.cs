using System.Linq;
using System.Windows;
using System.Windows.Data;

using Inventor.Core;
using Inventor.Core.Localization;
using Inventor.Core.Processing;

using Sef.Program;
using Sef.UI;

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
            saveLoadController = new SaveLoadController(buttonNew, buttonLoad, buttonSave, buttonSaveAs,
                                                        createNew, loadFromFile, saveToFile,
                                                        KnowledgeBase.CreateOpenFileDialog, KnowledgeBase.CreateSaveFileDialog,
                                                        (s, a) => { },
                                                        InventorApplication.Singleton.KnowledgeBase);
            realoadKnowledgeBaseTree();
        }

        private KnowledgeBaseNode knowledgeBaseNode;
        private SaveLoadController saveLoadController;

        #region Main menu

        private void saveToFile(IChangeable changeable, string fileName)
        {
            knowledgeBaseNode.KnowledgeBase.Save(fileName);
        }

        private IChangeable loadFromFile(string fileName)
        {
            InventorApplication.Singleton.KnowledgeBase = KnowledgeBase.Load(fileName);
            realoadKnowledgeBaseTree();
            return InventorApplication.Singleton.KnowledgeBase;
        }

        private IChangeable createNew()
        {
            InventorApplication.Singleton.KnowledgeBase = KnowledgeBase.New();
            realoadKnowledgeBaseTree();
            return InventorApplication.Singleton.KnowledgeBase;
        }

        private void createTestClick(object sender, RoutedEventArgs e)
        {
            InventorApplication.Singleton.KnowledgeBase = KnowledgeBase.CreateTest();
            realoadKnowledgeBaseTree();
            saveLoadController.ChangeEntity(InventorApplication.Singleton.KnowledgeBase);
        }

        private void realoadKnowledgeBaseTree()
        {
            treeViewKnowledgeBase.Items.Clear();
            if (knowledgeBaseNode != null)
            {
                knowledgeBaseNode.Clear();
            }
            treeViewKnowledgeBase.Items.Add(knowledgeBaseNode = new KnowledgeBaseNode(InventorApplication.Singleton.KnowledgeBase));
            knowledgeBaseNode.IsExpanded = true;
        }

        #endregion

        #region Knowledgebase actions

        private void askQuestionClick(object sender, RoutedEventArgs e)
        {
            var dialog = new QuestionDialog(InventorApplication.Singleton.KnowledgeBase)
            {
                Owner = this,
                Title = LanguageEx.CurrentEx.Ui.QuestionDialog.Title,
            };
            if (dialog.ShowDialog() == true)
            {
                new FormattedTextDialog(
                    QuestionProcessor.Process(InventorApplication.Singleton.KnowledgeBase, dialog.Question), 
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
                InventorApplication.Singleton.KnowledgeBase.DescribeRules(),
                knowledgeObjectPicked)
            {
                Owner = this,
                Title = LanguageEx.CurrentEx.Misc.Rules,
            }.Show();
        }

        private void checkKnowledgeClick(object sender, RoutedEventArgs e)
        {
            new FormattedTextDialog(
                InventorApplication.Singleton.KnowledgeBase.CheckConsistensy(),
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
            var path = knowledgeBaseNode.Find(entity).OfType<object>().ToList();
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
