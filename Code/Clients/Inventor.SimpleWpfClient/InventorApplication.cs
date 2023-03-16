using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using Inventor.Semantics;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Utils;
using Inventor.Semantics.Mathematics;
using Inventor.Semantics.Processes;
using Inventor.Semantics.Set;
using Inventor.Semantics.WPF;
using Inventor.Semantics.WPF.Dialogs;

namespace Inventor.SimpleWpfClient
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
				return ConfigurationFile.DeserializeFromFile<InventorConfiguration>();
			}
			catch
			{
				var configuration = new InventorConfiguration();
				configuration.SerializeToFile(ConfigurationFile);
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
			new Semantics.Modules.Boolean.BooleanModule().RegisterMetadata();
			new Semantics.Modules.Classification.ClassificationModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
			new WpfUiModule().RegisterMetadata();

			Language.PrepareModulesToSerialization<Language>();
		}

		private void initializeSemanticNetwork()
		{
#if DEBUG
			SemanticNetwork = new Semantics.Test.Sample.TestSemanticNetwork(CurrentLanguage).SemanticNetwork;
#else
			SemanticNetwork = new SemanticNetwork(CurrentLanguage);
#endif
		}

		#endregion
	}
}
