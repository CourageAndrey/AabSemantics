using System.Windows;

namespace Inventor.Client.UI
{
	public partial class ConceptDialog
	{
		public ConceptDialog()
		{
			InitializeComponent();
		}

		public ViewModels.Concept EditValue
		{
			get { return _contextControl.DataContext as ViewModels.Concept; }
			set
			{
				_contextControl.DataContext = value;
				_nameConrol.IsEnabled = value.Name != null;
				_hintConrol.IsEnabled = value.Hint != null;
			}
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
