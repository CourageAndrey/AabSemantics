using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

using Sef.Localization;

namespace Sef.UI
{
    public partial class AutoWaitDialog
    {
        #region Properties

        private Action<BackgroundWorker, DoWorkEventArgs> code = (bw, args) => { };
        private readonly BackgroundWorker worker;

        public String Header
        {
            get { return labelCaption.Content as String; }
            set { labelCaption.Content = value; }
        }

        public Boolean ShowProgress
        {
            get { return !progressBar.IsIndeterminate; }
            set
            {
                labelCaption.Content = value;
                if (value)
                {
                    progressBar.IsIndeterminate = false;
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;
                    progressBar.Value = 0;
                }
                else
                {
                    progressBar.IsIndeterminate = true;
                }
            }
        }

        public Boolean CanCancel
        {
            get { return buttonCancel.IsEnabled; }
            set { buttonCancel.IsEnabled = value; }
        }

        public Exception ProcessingError
        { get; private set; }

        #endregion

        public AutoWaitDialog()
        {
            InitializeComponent();

            var bounds = Screen.PrimaryScreen.Bounds;
            MaxHeight = bounds.Height;
            MaxWidth = bounds.Width;

            buttonCancel.Content = Localization.Language.Current.Common.Cancel;
            textBlockDescription.Text = Localization.Language.Current.Common.WaitPromt;

            labelCaption.FontSize *= 1.5;

            worker = new BackgroundWorker();
            worker.DoWork += background_DoWork;
            worker.RunWorkerCompleted += background_RunWorkerCompleted;
            worker.ProgressChanged += background_ProgressChanged;
        }

        private void form_Loaded(Object sender, RoutedEventArgs e)
        {
            worker.WorkerReportsProgress = ShowProgress;
            worker.WorkerSupportsCancellation = CanCancel;
            worker.RunWorkerAsync();
        }

        private void buttonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }

        #region Background worker events handlers

        private void background_ProgressChanged(Object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            textBlockDescription.Text = e.UserState.ToString();
        }

        private void background_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessingError = e.Error;
            if (!e.Cancelled && progressBar.Value < 100)
            {
                progressBar.Value = 100;
            }
            Close();
        }

        private void background_DoWork(Object sender, DoWorkEventArgs e)
        {
            code(worker, e);
        }

        #endregion

        public void SetCode(Action<BackgroundWorker, DoWorkEventArgs> action)
        {
            code = action;
        }

        public void SetCode(Action action)
        {
            code = (bw, args) => action();
        }
    }
}
