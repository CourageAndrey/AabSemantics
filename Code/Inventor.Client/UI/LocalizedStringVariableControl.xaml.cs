using System.Windows;

namespace Inventor.Client.UI
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
	}
}
