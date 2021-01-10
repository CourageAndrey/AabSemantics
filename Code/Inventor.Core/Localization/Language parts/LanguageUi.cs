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

		LanguageMainForm MainForm
		{ get; }

		LanguageQuestionDialog QuestionDialog
		{ get; }
	}

	public class LanguageUi : ILanguageUi
	{
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
		public LanguageMainForm MainForm
		{ get; set; }

		[XmlElement]
		public LanguageQuestionDialog QuestionDialog
		{ get; set; }

		#endregion

		internal static LanguageUi CreateDefault()
		{
			return new LanguageUi
			{
				Ok = "ОК",
				Cancel = "Отмена",
				Next = "Далее >",
				Back = "< Назад",
				QuestionDialog = LanguageQuestionDialog.CreateDefault(),
				MainForm = LanguageMainForm.CreateDefault(),
			};
		}
	}
}
