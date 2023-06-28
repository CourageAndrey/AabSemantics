using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using AabSemantics.Extensions.WPF;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Localization;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Set;
using AabSemantics.Serialization.Xml;
using AabSemantics.Test.Sample;

namespace AabSemantics.SimpleWpfClient
{
	public class InventorApplication : Application, IInventorApplication
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

		public IMainWindow MainForm
		{ get; }

		private String ConfigurationFile
		{ get { return Path.Combine(StartupPath, InventorConfiguration.FileName); } }

		public ISemanticNetwork SemanticNetwork
		{ get; set; }

		#endregion

		public InventorApplication()
		{
			initializeExceptionHandling();

			Languages = initializeLanguages();

			Configuration = initializeConfiguration();

			selectLanguage();

			var mainForm = new MainWindow();
			MainForm = mainForm;
			MainWindow = mainForm;

			ShutdownMode = ShutdownMode.OnMainWindowClose;
		}

		#region Constructor routines

		private ICollection<ILanguage> initializeLanguages()
		{
			var languages = StartupPath.LoadAdditionalLanguages();
			languages.Add(Language.Default);
			return languages.ToArray();
		}

		private InventorConfiguration initializeConfiguration()
		{
			try
			{
				return ConfigurationFile.DeserializeFromXmlFile<InventorConfiguration>();
			}
			catch
			{
				var configuration = new InventorConfiguration();
				configuration.SerializeToXmlFile(ConfigurationFile);
				return configuration;
			}
		}

		private void selectLanguage()
		{
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
				CurrentLanguage = Languages.FindAppropriate(Language.Default);
			}
		}

		#endregion

		#region Exception handling

		private void initializeExceptionHandling()
		{
			DispatcherUnhandledException += dispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += dispatcherAppDomainException;
		}

		private void dispatcherAppDomainException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception)
			{
				new ExceptionDialog(
					new ExceptionWrapper(e.ExceptionObject as Exception),
					ExceptionDialogMode.ProcessFatalError,
					CurrentLanguage ?? Language.Default).ShowDialog();
			}
		}

		private void dispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			if (new ExceptionDialog(e.Exception, false, CurrentLanguage ?? Language.Default).ShowDialog() == true)
			{
				Shutdown();
			}
			e.Handled = true;
		}

		#endregion

		[STAThread]
		private static void Main()
		{
			registerModules();

			var application = new InventorApplication();
			application.initializeSemanticNetwork();

			application.MainForm.Initialize(application);
			application.MainWindow.Show();
			application.Run();
		}

		#region Main routines

		private static void registerModules()
		{
			new Modules.Boolean.BooleanModule().RegisterMetadata();
			new Modules.Classification.ClassificationModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
			new WpfUiModule().RegisterMetadata();

			Language.PrepareModulesToSerialization<Language>();
		}

		private void initializeSemanticNetwork()
		{
#if DEBUG
			SemanticNetwork = new SemanticNetwork(CurrentLanguage);
			SemanticNetwork.CreateCombinedTestData();
#else
			SemanticNetwork = new SemanticNetwork(CurrentLanguage);
#endif
		}

		#endregion
	}
}
