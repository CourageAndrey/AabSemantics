using System.Windows;
using System.Windows.Controls;

namespace AabSemantics.Extensions.WPF.Controls
{
	public partial class LocalizedStringVariableControl
	{
		/// <summary>Creates the control.</summary>
		public LocalizedStringVariableControl()
		{
			InitializeComponent();
		}

		/// <summary>The localized string being edited.</summary>
		public ViewModels.LocalizedString EditValue
		{
			get { return GetValue(EditValueProperty) as ViewModels.LocalizedString; }
			set { SetValue(EditValueProperty, value); }
		}

		/// <summary>Backing dependency property of <see cref="EditValue"/>.</summary>
		public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register(
			nameof(EditValue),
			typeof(ViewModels.LocalizedString),
			typeof(LocalizedStringVariableControl),
			new FrameworkPropertyMetadata(
				null,
				FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
				(dependencyObject, e) => ((LocalizedStringVariableControl) dependencyObject).applyEditValue(e.NewValue as ViewModels.LocalizedString)));

		/// <summary>Applies the language's captions to this element.</summary>
		/// <param name="language">Language to localize in.</param>
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
