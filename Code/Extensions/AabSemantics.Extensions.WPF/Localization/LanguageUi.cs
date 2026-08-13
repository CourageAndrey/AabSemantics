using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Captions of the wizard-style dialogs.</summary>
	public interface ILanguageUi
	{
		/// <summary>Caption of the confirm button.</summary>
		String Ok
		{ get; }

		/// <summary>Caption of the cancel button.</summary>
		String Cancel
		{ get; }

		/// <summary>Caption of the wizard's forward button.</summary>
		String Next
		{ get; }

		/// <summary>Caption of the wizard's back button.</summary>
		String Back
		{ get; }

		/// <summary>Header of the statement type selection dialog.</summary>
		String StatementTypeDialogHeader
		{ get; }

		/// <summary>Header of the module selection dialog.</summary>
		String SelectModulesDialogHeader
		{ get; }

		/// <summary>Header of the knowledge graph dialog.</summary>
		String GraphDialogHeader
		{ get; }

		/// <summary>Caption of the command creating a new statement.</summary>
		String CreateNewStatement
		{ get; }

		/// <summary>Captions of the main window.</summary>
		ILanguageMainForm MainForm
		{ get; }

		/// <summary>Captions of the question dialog.</summary>
		ILanguageQuestionDialog QuestionDialog
		{ get; }

		/// <summary>Captions of the editing dialogs.</summary>
		ILanguageEditing Editing
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageUi"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageUi : ILanguageUi
	{
		#region Constants

		[XmlIgnore]
		private const String ElementMainForm = "MainForm";
		[XmlIgnore]
		private const String ElementQuestionDialog = "QuestionDialog";
		[XmlIgnore]
		private const String ElementEditing = "Editing";

		#endregion

		#region Properties

		/// <summary>Caption of the confirm button.</summary>
		[XmlElement]
		public String Ok
		{ get; set; }

		/// <summary>Caption of the cancel button.</summary>
		[XmlElement]
		public String Cancel
		{ get; set; }

		/// <summary>Caption of the wizard's forward button.</summary>
		[XmlElement]
		public String Next
		{ get; set; }

		/// <summary>Caption of the wizard's back button.</summary>
		[XmlElement]
		public String Back
		{ get; set; }

		/// <summary>Header of the statement type selection dialog.</summary>
		[XmlElement]
		public String StatementTypeDialogHeader
		{ get; set; }

		/// <summary>Header of the module selection dialog.</summary>
		[XmlElement]
		public String SelectModulesDialogHeader
		{ get; set; }

		/// <summary>Header of the knowledge graph dialog.</summary>
		[XmlElement]
		public String GraphDialogHeader
		{ get; set; }

		/// <summary>Caption of the command creating a new statement.</summary>
		[XmlElement]
		public String CreateNewStatement
		{ get; set; }

		/// <summary>Captions of the main window. In serializable form.</summary>
		[XmlElement(ElementMainForm)]
		public LanguageMainForm MainFormXml
		{ get; set; }

		/// <summary>Captions of the question dialog. In serializable form.</summary>
		[XmlElement(ElementQuestionDialog)]
		public LanguageQuestionDialog QuestionDialogXml
		{ get; set; }

		/// <summary>Captions of the editing dialogs. In serializable form.</summary>
		[XmlElement(ElementEditing)]
		public LanguageEditing EditingXml
		{ get; set; }

		/// <summary>Captions of the main window.</summary>
		[XmlIgnore]
		public ILanguageMainForm MainForm
		{ get { return MainFormXml; } }

		/// <summary>Captions of the question dialog.</summary>
		[XmlIgnore]
		public ILanguageQuestionDialog QuestionDialog
		{ get { return QuestionDialogXml; } }

		/// <summary>Captions of the editing dialogs.</summary>
		[XmlIgnore]
		public ILanguageEditing Editing
		{ get { return EditingXml; } }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageUi CreateDefault()
		{
			return new LanguageUi
			{
				Ok = "OK",
				Cancel = "Cancel",
				Next = "Next >",
				Back = "< Prev",
				StatementTypeDialogHeader = "Chose statement type",
				SelectModulesDialogHeader = "Modules",
				GraphDialogHeader = "Graph",
				CreateNewStatement = "Create new statement? (No - edit existing)",
				QuestionDialogXml = LanguageQuestionDialog.CreateDefault(),
				MainFormXml = LanguageMainForm.CreateDefault(),
				EditingXml = LanguageEditing.CreateDefault(),
			};
		}
	}
}
