using System.Windows;

using Inventor.Core;

namespace Inventor.Client.Dialogs
{
	public partial class GroupStatementDialog
	{
		public GroupStatementDialog()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_comboBoxConcept.ItemsSource = knowledgeBase.Concepts;
			_comboBoxArea.ItemsSource = knowledgeBase.Concepts;
		}

		public ViewModels.GroupStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.GroupStatement; }
			set { _contextControl.DataContext = value; }
		}

		private void okClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void cancelClick(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
