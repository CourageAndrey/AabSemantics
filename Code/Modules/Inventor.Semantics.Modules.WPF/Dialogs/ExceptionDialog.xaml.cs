using System;
using System.Windows;
using System.Windows.Data;

using Microsoft.Win32;

using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.WPF.Dialogs
{
	public enum ExceptionDialogMode
	{
		ViewOnly,
		ShowInnerExeption,
		ProcessError,
		ProcessFatalError
	}

	public partial class ExceptionDialog
	{
		private readonly ExceptionDialogMode _mode;
		private readonly IExceptionWrapper _exception;
		private readonly ILanguage _language;

		#region Constructor

		public ExceptionDialog(IExceptionWrapper exception, ExceptionDialogMode mode, ILanguage language)
		{
			_mode = mode;

			InitializeComponent();

			SetBinding(TitleProperty, new Binding("Errors.DialogHeader")
			{
				Source = Resources["language"],
				Mode = BindingMode.OneTime
			});

			buttonInnerException.Visibility = (exception.InnerException == null) ? Visibility.Collapsed : Visibility.Visible;
			switch (mode)
			{
				case ExceptionDialogMode.ViewOnly:
				case ExceptionDialogMode.ShowInnerExeption:
				case ExceptionDialogMode.ProcessFatalError:
					buttonAbort.Visibility = Visibility.Collapsed;
					buttonIgnore.Visibility = Visibility.Collapsed;
					break;
				case ExceptionDialogMode.ProcessError:
					buttonClose.Visibility = Visibility.Collapsed;
					break;
				default:
					throw new NotSupportedException();
			}

			_language = language;
			localize();

			gridData.DataContext = _exception = exception;
		}

		public ExceptionDialog(IExceptionWrapper exception, Boolean viewOnly, ILanguage language)
			: this(exception, viewOnly ? ExceptionDialogMode.ViewOnly : ExceptionDialogMode.ProcessError, language)
		{ }

		public ExceptionDialog(Exception exception, Boolean viewOnly, ILanguage language)
			: this(new ExceptionWrapper(exception), viewOnly, language)
		{ }

		#endregion

		#region Implementation of ILocalizable

		private void localize()
		{
			var localizationProvider = (ObjectDataProvider) Resources["language"];
			localizationProvider.ConstructorParameters.Add(_language);

			var uiModule = _language.GetExtension<IWpfUiModule>();
			var languageErrors = uiModule.Errors;
			switch (_mode)
			{
				case ExceptionDialogMode.ViewOnly:
					labelCommonMessage.Text = languageErrors.DialogMessageView;
					break;
				case ExceptionDialogMode.ShowInnerExeption:
					labelCommonMessage.Text = languageErrors.DialogMessageInner;
					break;
				case ExceptionDialogMode.ProcessError:
					labelCommonMessage.Text = languageErrors.DialogMessageCommon;
					break;
				case ExceptionDialogMode.ProcessFatalError:
					labelCommonMessage.Text = languageErrors.DialogMessageFatal;
					break;
				default:
					throw new NotSupportedException();
			}

			saveDialog.Title = uiModule.Common.SaveFile;
			saveDialog.Filter = languageErrors.SaveFilter;
		}

		#endregion

		#region Button handlers

		private void buttonAbort_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private readonly SaveFileDialog saveDialog = new SaveFileDialog
		{
			RestoreDirectory = true
		};

		private void buttonSave_Click(Object sender, RoutedEventArgs e)
		{
			if (saveDialog.ShowDialog() == true)
			{
				_exception.SerializeToXmlFile(saveDialog.FileName);
			}
		}

		private void buttonInnerException_Click(Object sender, RoutedEventArgs e)
		{
			new ExceptionDialog(_exception.InnerException, ExceptionDialogMode.ShowInnerExeption, _language).ShowDialog();
		}

		#endregion
	}
}
