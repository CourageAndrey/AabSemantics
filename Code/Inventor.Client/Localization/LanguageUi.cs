using System;
using System.Xml.Serialization;

namespace Inventor.Client.Localization
{
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
				Ok = "ОК",
				Cancel = "Отмена",
				Next = "Далее >",
				Back = "< Назад",
				StatementTypeDialogHeader = "Выберите тип утверждения",
				CreateNewStatement = "Создать новое утверждение? (Нет - редактирование старого)",
				CreateNewQuestion = "Создать новый вопрос? (Нет - редактирование старого)",
				QuestionDialogXml = LanguageQuestionDialog.CreateDefault(),
				MainFormXml = LanguageMainForm.CreateDefault(),
				EditingXml = LanguageEditing.CreateDefault(),
			};
		}
	}
}
