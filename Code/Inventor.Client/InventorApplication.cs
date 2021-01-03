using System;

using Inventor.Client.UI;
using Inventor.Core;
using Inventor.Core.Localization;

using Sef.Program;

namespace Inventor.Client
{
	public class InventorApplication : ExtendedApplication<InventorConfiguration, LanguageEx, MainWindow>
	{
		#region Properties

		public KnowledgeBase KnowledgeBase
		{ get; internal set; }

		public override LanguageEx CurrentLanguage
		{
			get { return LanguageEx.CurrentEx; }
			set
			{
				LanguageEx.CurrentEx = value;
				Configuration.SelectedLanguage = value.Culture;
			}
		}

		#endregion

		public InventorApplication()
			: base(AppDomain.CurrentDomain)
		{
			MainWindow = MainForm;
			ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
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
	}
}
