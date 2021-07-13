using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using Inventor.Client.Localization;
using Inventor.Core;
using Inventor.Core.Utils;

namespace Inventor.Client
{
	public class InventorApplication : Application
	{
		#region Properties

		public String StartupPath
		{ get { return AppDomain.CurrentDomain.BaseDirectory; } }

		private ILanguage _currentLanguage;

		public ILanguage CurrentLanguage
		{
			get { return _currentLanguage; }
			set
			{
				_currentLanguage = value;
				Configuration.SelectedLanguage = value.Culture;
			}
		}

		public ICollection<ILanguage> Languages
		{ get; }

		public InventorConfiguration Configuration
		{ get; }

		public MainWindow MainForm
		{ get; }

		private String ConfigurationFile
		{ get { return Path.Combine(StartupPath, InventorConfiguration.FileName); } }

		public IKnowledgeBase KnowledgeBase
		{ get; internal set; }

		#endregion

		public InventorApplication()
		{
			// exception handling
			DispatcherUnhandledException += dispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += dispatcherAppDomainException;

			// addition languages
			var languages = Language.LoadAdditional(StartupPath);
			languages.Add((ILanguage) Core.Localization.Language.Default);
			Languages = languages.ToArray();

			// configuration
			try
			{
				Configuration = ConfigurationFile.DeserializeFromFile<InventorConfiguration>();
			}
			catch
			{
				Configuration = new InventorConfiguration();
				SaveConfiguration();
			}

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
				CurrentLanguage = Languages.FindAppropriate((Language) Core.Localization.Language.Default);
				Configuration.SelectedLanguage = CurrentLanguage.Culture;
			}

			// form
			MainForm = new MainWindow();

			MainWindow = MainForm;
			ShutdownMode = ShutdownMode.OnMainWindowClose;
		}

		[STAThread]
		private static void Main()
		{
			var application = new InventorApplication();

#if DEBUG
			application.KnowledgeBase = new Core.Base.TestKnowledgeBase(application.CurrentLanguage).KnowledgeBase;
#else
			application.KnowledgeBase = Core.Base.KnowledgeBase.New(application.CurrentLanguage);
#endif
			application.MainForm.Initialize(application);
			application.MainWindow.Show();
			application.Run();
		}

		#region Exception handling

		private void dispatcherAppDomainException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception)
			{
				new Dialogs.ExceptionDialog(new ExceptionWrapper(e.ExceptionObject as Exception), Dialogs.ExceptionDialogMode.ProcessFatalError, CurrentLanguage).ShowDialog();
			}
		}

		private void dispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			if (new Dialogs.ExceptionDialog(e.Exception, false, CurrentLanguage).ShowDialog() == true)
			{
				Shutdown();
			}
			e.Handled = true;
		}

		#endregion

		public void SaveConfiguration()
		{
			Configuration.SerializeToFile(ConfigurationFile);
		}
	}
}
