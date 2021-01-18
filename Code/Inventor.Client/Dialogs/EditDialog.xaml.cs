using System.Windows;

using Inventor.Core;

namespace Inventor.Client.Dialogs
{
	public partial class EditDialog
	{
		public EditDialog()
		{
			InitializeComponent();
		}

		private UIElement _editor;

		public UIElement Editor
		{
			get { return _editor; }
			set
			{
				if (_editor != null)
				{
					_dockPanel.Children.Remove(_editor);
				}
				_editor = value;
				if (_editor != null)
				{
					_dockPanel.Children.Add(_editor);
				}
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

		public void Localize(ILanguage language)
		{
			_buttonOk.Content = language.Common.Ok;
			_buttonCancel.Content = language.Common.Cancel;
		}
	}
}
