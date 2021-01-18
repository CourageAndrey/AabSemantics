using System.Windows;

using Inventor.Core;

namespace Inventor.Client.Dialogs
{
	public partial class ConsistsOfStatementDialog
	{
		public ConsistsOfStatementDialog()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_comboBoxParent.ItemsSource = knowledgeBase.Concepts;
			_comboBoxChild.ItemsSource = knowledgeBase.Concepts;
		}

		public ViewModels.ConsistsOfStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.ConsistsOfStatement; }
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
