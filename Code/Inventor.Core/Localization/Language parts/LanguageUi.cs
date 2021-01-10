using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
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

		ILanguageMainForm MainForm
		{ get; }

		ILanguageQuestionDialog QuestionDialog
		{ get; }
	}

	public class LanguageUi : ILanguageUi
	{
		#region Constants

		[XmlIgnore]
		private const String ElementMainForm = "MainForm";
		[XmlIgnore]
		private const String ElementQuestionDialog = "QuestionDialog";

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

		[XmlElement(ElementMainForm)]
		public LanguageMainForm MainFormXml
		{ get; set; }

		[XmlElement(ElementQuestionDialog)]
		public LanguageQuestionDialog QuestionDialogXml
		{ get; set; }

		[XmlIgnore]
		public ILanguageMainForm MainForm
		{ get { return MainFormXml; } }

		[XmlIgnore]
		public ILanguageQuestionDialog QuestionDialog
		{ get { return QuestionDialogXml; } }

		#endregion

		internal static LanguageUi CreateDefault()
		{
			return new LanguageUi
			{
				Ok = "ОК",
				Cancel = "Отмена",
				Next = "Далее >",
				Back = "< Назад",
				QuestionDialogXml = LanguageQuestionDialog.CreateDefault(),
				MainFormXml = LanguageMainForm.CreateDefault(),
			};
		}
	}
}
