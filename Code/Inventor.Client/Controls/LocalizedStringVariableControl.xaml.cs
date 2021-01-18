using System.Windows;

using Inventor.Core;

namespace Inventor.Client.Controls
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
				(dependencyObject, e) => ((LocalizedStringVariableControl) dependencyObject)._contextControl.DataContext = e.NewValue));

		public void Localize(ILanguage language)
		{
			_columnLanguage.Header = language.Ui.Editing.ColumnHeaderLanguage;
			_columnValue.Header = language.Ui.Editing.ColumnHeaderValue;
		}
	}
}
