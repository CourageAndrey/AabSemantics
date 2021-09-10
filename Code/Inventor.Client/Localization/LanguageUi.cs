using System;
using System.Xml.Serialization;

namespace Inventor.Client.Localization
{
	public interface ILanguageUi
	{
		String Ok
		{ get; }

		String Cancel
		{ get; }

		String Next
		{ get; }

		String Back
		{ get; }

		String StatementTypeDialogHeader
		{ get; }

		String CreateNewStatement
		{ get; }

		String CreateNewQuestion
		{ get; }

		ILanguageMainForm MainForm
		{ get; }

		ILanguageQuestionDialog QuestionDialog
		{ get; }

		ILanguageEditing Editing
		{ get; }
	}

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

		[XmlElement]
		public String Ok
		{ get; set; }

		[XmlElement]
		public String Cancel
		{ get; set; }

		[XmlElement]
		public String Next
		{ get; set; }

		[XmlElement]
		public String Back
		{ get; set; }

		[XmlElement]
		public String StatementTypeDialogHeader
		{ get; set; }

		[XmlElement]
		public String CreateNewStatement
		{ get; set; }

		[XmlElement]
		public String CreateNewQuestion
		{ get; set; }

		[XmlElement(ElementMainForm)]
		public LanguageMainForm MainFormXml
		{ get; set; }

		[XmlElement(ElementQuestionDialog)]
		public LanguageQuestionDialog QuestionDialogXml
		{ get; set; }

		[XmlElement(ElementEditing)]
		public LanguageEditing EditingXml
		{ get; set; }

		[XmlIgnore]
		public ILanguageMainForm MainForm
		{ get { return MainFormXml; } }

		[XmlIgnore]
		public ILanguageQuestionDialog QuestionDialog
		{ get { return QuestionDialogXml; } }

		[XmlIgnore]
		public ILanguageEditing Editing
		{ get { return EditingXml; } }

		#endregion

		internal static LanguageUi CreateDefault()
		{
			return new LanguageUi
			{
				Ok = "OK",
				Cancel = "Cancel",
				Next = "Next >",
				Back = "< Prev",
				StatementTypeDialogHeader = "Chose statement type",
				CreateNewStatement = "Create new statement? (No - edit existing)",
				CreateNewQuestion = "Create new question? (No - edit existing)",
				QuestionDialogXml = LanguageQuestionDialog.CreateDefault(),
				MainFormXml = LanguageMainForm.CreateDefault(),
				EditingXml = LanguageEditing.CreateDefault(),
			};
		}
	}
}
