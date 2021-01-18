using System.Windows;

using Inventor.Core;

namespace Inventor.Client.Dialogs
{
	public partial class SignValueStatementDialog
	{
		public SignValueStatementDialog()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_comboBoxConcept.ItemsSource = knowledgeBase.Concepts;
			_comboBoxSign.ItemsSource = knowledgeBase.Concepts;
			_comboBoxValue.ItemsSource = knowledgeBase.Concepts;
		}

		public ViewModels.SignValueStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.SignValueStatement; }
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
