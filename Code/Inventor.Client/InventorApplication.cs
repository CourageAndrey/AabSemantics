using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

using Inventor.Client.UI;
using Inventor.Core;
using Inventor.Core.Localization;

namespace Inventor.Client
{
	public class InventorApplication : Application
	{
		#region Properties

		public String StartupPath
		{ get { return AppDomain.CurrentDomain.BaseDirectory; } }

		public LanguageEx CurrentLanguage
		{
			get { return LanguageEx.CurrentEx; }
			set
			{
				LanguageEx.CurrentEx = value;
				Configuration.SelectedLanguage = value.Culture;
			}
		}

		public IList<LanguageEx> Languages
		{ get; }

		public InventorConfiguration Configuration
		{ get; }

		public MainWindow MainForm
		{ get; }

		private String ConfigurationFile
		{ get { return Path.Combine(StartupPath, InventorConfiguration.FileName); } }

		public KnowledgeBase KnowledgeBase
		{ get; internal set; }

		#endregion

		public InventorApplication()
		{
			// exception handling
			DispatcherUnhandledException += dispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += dispatcherAppDomainException;

			// addition languages
			var languages = Language.LoadAdditional<LanguageEx>(StartupPath);
			languages.Add(CurrentLanguage);
			Languages = languages.AsReadOnly();

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

			// form
			MainForm = new MainWindow();

			MainWindow = MainForm;
			ShutdownMode = ShutdownMode.OnMainWindowClose;
		}

		[STAThread]
		private static void Main()
		{
			var application = new InventorApplication
			{
#if DEBUG
				KnowledgeBase = KnowledgeBase.CreateTest()
#else
				KnowledgeBase = KnowledgeBase.New(LanguageEx.CurrentEx)
#endif
			};
			application.MainForm.Initialize(application);
			application.MainWindow.Show();
			application.Run();
		}

		#region Exception handling

		private static void dispatcherAppDomainException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception)
			{
				new ExceptionDialog(new ExceptionWrapper(e.ExceptionObject as Exception), ExceptionDialog.Mode.ProcessFatalError).ShowDialog();
			}
		}

		private void dispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			if (new ExceptionDialog(e.Exception, false).ShowDialog() == true)
			{
				Shutdown();
			}
			e.Handled = true;
		}

		#endregion

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

		public void SaveConfiguration()
		{
			Configuration.SerializeToFile(ConfigurationFile);
		}
	}
}
