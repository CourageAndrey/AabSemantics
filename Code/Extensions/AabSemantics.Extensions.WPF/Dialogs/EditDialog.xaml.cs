using System.Windows;

namespace AabSemantics.Extensions.WPF.Dialogs
{
	public partial class EditDialog
	{
		/// <summary>Creates the dialog.</summary>
		public EditDialog()
		{
			InitializeComponent();
		}

		private UIElement _editor;

		/// <summary>The hosted editor control.</summary>
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

		/// <summary>Applies the language's captions to this element.</summary>
		/// <param name="language">Language to localize in.</param>
		public void Localize(ILanguage language)
		{
			_buttonOk.Content = language.GetExtension<IWpfUiModule>().Common.Ok;
			_buttonCancel.Content = language.GetExtension<IWpfUiModule>().Common.Cancel;
		}
	}
}
