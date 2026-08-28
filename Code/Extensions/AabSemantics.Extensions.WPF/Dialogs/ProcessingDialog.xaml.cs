using System;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AabSemantics.Extensions.WPF.Dialogs
{
	/// <summary>
	/// Modal "please wait" dialog running a long operation on a background thread, so that the user
	/// interface stays responsive and the operation can be cancelled while it is under way.
	/// <para>
	/// Inference itself never yields — it walks in-memory collections from start to finish — so it
	/// has to be moved off the interface thread here rather than merely awaited. Cancelling does not
	/// close the dialog at once: the operation is asked to stop and the dialog waits for it to
	/// unwind, which keeps the knowledge base from being edited while a traversal is still reading it.
	/// </para>
	/// </summary>
	public partial class ProcessingDialog
	{
		#region Properties

		private readonly ILanguage _language;
		private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

		private Func<CancellationToken, Task<Object>> _operation;
		private Object _result;
		private ExceptionDispatchInfo _failure;
		private Boolean _started;
		private Boolean _finished;

		#endregion

		/// <summary>Creates the dialog.</summary>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <param name="caption">Describes what is being done, e.g. "Looking for the answer...".</param>
		public ProcessingDialog(ILanguage language, String caption)
		{
			_language = language;

			InitializeComponent();

			var localizationProvider = (ObjectDataProvider) Resources["language"];
			localizationProvider.ConstructorParameters.Add(_language);

			Title = textBlockCaption.Text = caption;
		}

		/// <summary>
		/// Runs an operation while showing the dialog, and blocks until it either finishes or the
		/// user cancels it.
		/// </summary>
		/// <typeparam name="T">Type of the operation's result.</typeparam>
		/// <param name="owner">Window the dialog belongs to.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <param name="caption">Describes what is being done.</param>
		/// <param name="operation">Operation to run; it is expected to observe the token it is given.</param>
		/// <param name="result">Receives the operation's result, or the type's default when cancelled.</param>
		/// <returns><c>true</c> when the operation ran to the end, <c>false</c> when the user cancelled it.</returns>
		/// <exception cref="Exception">Whatever the operation threw, rethrown once the dialog is closed.</exception>
		public static Boolean TryRun<T>(Window owner, ILanguage language, String caption, Func<CancellationToken, Task<T>> operation, out T result)
		{
			var dialog = new ProcessingDialog(language, caption)
			{
				Owner = owner,
			};

			try
			{
				dialog._operation = async cancellationToken => (Object) await operation(cancellationToken).ConfigureAwait(false);

				Boolean completed = dialog.ShowDialog() == true;

				// the operation has unwound by now, so its failure can be reported the usual way
				dialog._failure?.Throw();

				result = completed ? (T) dialog._result : default(T);
				return completed;
			}
			finally
			{
				dialog._cancellation.Dispose();
			}
		}

		private async void dialogLoaded(Object sender, RoutedEventArgs e)
		{
			// the event can be raised more than once, but the operation has to run exactly once
			if (_started)
			{
				return;
			}
			_started = true;

			try
			{
				// the operation is synchronous under its asynchronous signature, hence Task.Run
				_result = await Task.Run(() => _operation(_cancellation.Token), _cancellation.Token);
			}
			catch (OperationCanceledException)
			{
				_result = null;
			}
			catch (Exception error)
			{
				_failure = ExceptionDispatchInfo.Capture(error);
			}
			finally
			{
				_finished = true;
				DialogResult = _failure == null && !_cancellation.IsCancellationRequested;
			}
		}

		private void cancelClick(Object sender, RoutedEventArgs e)
		{
			requestCancellation();
		}

		private void dialogClosing(Object sender, CancelEventArgs e)
		{
			if (!_finished)
			{
				// closing the window is just another way of asking to stop
				requestCancellation();
				e.Cancel = true;
			}
		}

		private void requestCancellation()
		{
			if (_finished || _cancellation.IsCancellationRequested)
			{
				return;
			}

			_cancellation.Cancel();

			buttonCancel.IsEnabled = false;
			textBlockCaption.Text = _language.GetExtension<IWpfUiModule>().Common.Cancelling;
		}
	}
}
