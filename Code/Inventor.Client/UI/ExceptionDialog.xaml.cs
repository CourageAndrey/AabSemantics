using System;
using System.Windows;
using System.Windows.Data;

using Microsoft.Win32;

using Inventor.Core;
using Inventor.Core.Localization;

namespace Inventor.Client.UI
{
	public partial class ExceptionDialog : ILocalizable
	{
		public enum Mode
		{
			ViewOnly,
			ShowInnerExeption,
			ProcessError,
			ProcessFatalError
		}

		private readonly Mode _mode;
		private readonly IExceptionWrapper _exception;

		#region Constructor

		public ExceptionDialog(IExceptionWrapper exception, Mode mode)
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
				case Mode.ViewOnly:
				case Mode.ShowInnerExeption:
				case Mode.ProcessFatalError:
					buttonAbort.Visibility = Visibility.Collapsed;
					buttonIgnore.Visibility = Visibility.Collapsed;
					break;
				case Mode.ProcessError:
					buttonClose.Visibility = Visibility.Collapsed;
					break;
				default:
					throw new NotSupportedException();
			}
			Localize();
			gridData.DataContext = _exception = exception;
		}

		public ExceptionDialog(IExceptionWrapper exception, Boolean viewOnly)
			: this(exception, viewOnly ? Mode.ViewOnly : Mode.ProcessError)
		{ }

		public ExceptionDialog(Exception exception, Boolean viewOnly)
			: this(new ExceptionWrapper(exception), viewOnly)
		{ }

		#endregion

		#region Implementation of ILocalizable

		public void Localize()
		{
			((ObjectDataProvider) Resources["language"]).Refresh();
			switch (_mode)
			{
				case Mode.ViewOnly:
					labelCommonMessage.Text = Core.Localization.Language.Current.Errors.DialogMessageView;
					break;
				case Mode.ShowInnerExeption:
					labelCommonMessage.Text = Core.Localization.Language.Current.Errors.DialogMessageInner;
					break;
				case Mode.ProcessError:
					labelCommonMessage.Text = Core.Localization.Language.Current.Errors.DialogMessageCommon;
					break;
				case Mode.ProcessFatalError:
					labelCommonMessage.Text = Core.Localization.Language.Current.Errors.DialogMessageFatal;
					break;
				default:
					throw new NotSupportedException();
			}
			saveDialog.Title = Core.Localization.Language.Current.Common.SaveFile;
			saveDialog.Filter = Core.Localization.Language.Current.Errors.SaveFilter;
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
				_exception.SerializeToFile(saveDialog.FileName);
			}
		}

		private void buttonInnerException_Click(Object sender, RoutedEventArgs e)
		{
			new ExceptionDialog(_exception.InnerException, Mode.ShowInnerExeption).ShowDialog();
		}

		#endregion
	}
}
