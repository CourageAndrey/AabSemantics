using System.Windows;
using System.Windows.Controls;

namespace Inventor.Semantics.Extensions.WPF.Controls
{
	public partial class LocalizedStringVariableControl
	{
		public LocalizedStringVariableControl()
		{
			InitializeComponent();
		}

		public ViewModels.LocalizedString EditValue
		{
			get { return GetValue(EditValueProperty) as ViewModels.LocalizedString; }
			set { SetValue(EditValueProperty, value); }
		}

		public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register(
			nameof(EditValue),
			typeof(ViewModels.LocalizedString),
			typeof(LocalizedStringVariableControl),
			new FrameworkPropertyMetadata(
				null,
				FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
				(dependencyObject, e) => ((LocalizedStringVariableControl) dependencyObject).applyEditValue(e.NewValue as ViewModels.LocalizedString)));

		public void Localize(ILanguage language)
		{
			_language = language;
			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_columnLanguage.Header = languageEditing.ColumnHeaderLanguage;
			_columnValue.Header = languageEditing.ColumnHeaderValue;
		}

		private ILanguage _language;

		private void applyEditValue(ViewModels.LocalizedString value)
		{
			if (value is ViewModels.LocalizedStringVariable)
			{
				_contextControl.DataContext = value;
			}
			else
			{
				_contextControl.Children.Clear();
				var constant = value as ViewModels.LocalizedStringConstant;
				if (constant != null)
				{
					_contextControl.Children.Add(new TextBlock { Text = constant.Original.GetValue(_language) });
				}
			}
		}
	}
}
