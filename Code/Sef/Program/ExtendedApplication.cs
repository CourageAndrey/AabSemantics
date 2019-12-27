using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

using Sef.Exceptions;
using Sef.Localization;
using Sef.Xml;

namespace Sef.Program
{
    public abstract class ExtendedApplication<ConfigurationT, LanguageT, FormT> : Application
        where ConfigurationT : Configuration, new()
        where LanguageT : Language, new()
        where FormT : Window, new()
    {
        #region Properties

        public String StartupPath
        { get { return System.Windows.Forms.Application.StartupPath; } }

        public abstract LanguageT CurrentLanguage
        { get; set; }

        public IList<LanguageT> Languages
        { get; private set; }

        public ConfigurationT Configuration
        { get; private set; }

        public FormT MainForm
        { get; private set; }

        private String ConfigurationFile
        { get { return Path.Combine(StartupPath, Program.Configuration.FileName); } }

        protected Boolean SaveConfigOnExit
        { get; set; }

        #endregion

        protected ExtendedApplication(AppDomain appDomain)
        {
            // exception handling
            DispatcherUnhandledException += dispatcherUnhandledException;
            appDomain.UnhandledException += dispatcherAppDomainException;

            // addition languages
            var languages = Language.LoadAdditional<LanguageT>(StartupPath);
            languages.Add(CurrentLanguage);
            Languages = languages.AsReadOnly();

            // configuration
            try
            {
                Configuration = ConfigurationFile.DeserializeFromFile<ConfigurationT>();
            }
            catch
            {
                Configuration = new ConfigurationT();
                SaveConfiguration();
            }

            // form
            MainForm = new FormT();
        }

        #region Exception handling

        private static void dispatcherAppDomainException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is WarningException)
            {
                MessageBox.Show(
                    (e.ExceptionObject as WarningException).Message,
                    Language.Current.Errors.Warning,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else if (e.ExceptionObject is Exception)
            {
                new ExceptionDialog(new ExceptionWrapper(e.ExceptionObject as Exception), ExceptionDialog.Mode.ProcessFatalError).ShowDialog();
            }
        }

        private void dispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is WarningException)
            {
                MessageBox.Show(
                    e.Exception.Message,
                    CurrentLanguage.Errors.Warning,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                e.Handled = true;
            }
            else
            {
                if (new ExceptionDialog(e.Exception, false).ShowDialog() == true)
                {
                    Shutdown();
                }
                e.Handled = true;
            }
        }

        #endregion

        #region Overrides

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);

            // select language
            if (!String.IsNullOrEmpty(Configuration.SelectedLanguage))
            {
                var storedLanguage = Languages.FirstOrDefault(l => l.Culture == Configuration.SelectedLanguage);
                if (storedLanguage != null)
                {
                    CurrentLanguage = storedLanguage;
                }
                else
                {
                    Configuration.SelectedLanguage = CurrentLanguage.Culture;
                }
            }
            else
            {
                CurrentLanguage = Language.FindAppropriate(Languages, CurrentLanguage);
                Configuration.SelectedLanguage = CurrentLanguage.Culture;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (SaveConfigOnExit)
            {
                Configuration.SerializeToFile(ConfigurationFile);
            }

            base.OnExit(e);
        }

        #endregion

        public void SaveConfiguration()
        {
            Configuration.SerializeToFile(ConfigurationFile);
        }
    }
}
