using System.Windows;

using Inventor.Core;

namespace Inventor.Client.UI
{
	public partial class IsStatementDialog
	{
		public IsStatementDialog()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_comboBoxParent.ItemsSource = knowledgeBase.Concepts;
			_comboBoxChild.ItemsSource = knowledgeBase.Concepts;
		}

		public ViewModels.IsStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.IsStatement; }
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
