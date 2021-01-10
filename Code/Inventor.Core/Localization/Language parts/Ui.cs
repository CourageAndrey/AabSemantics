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

	public interface ILanguageMainForm
	{
		String Title
		{ get; }

		String CreateNew
		{ get; }

		String Load
		{ get; }

		String Save
		{ get; }

		String SaveAs
		{ get; }

		String CreateTest
		{ get; }

		String DescribeKnowledge
		{ get; }

		String CheckKnowledge
		{ get; }

		String AskQuestion
		{ get; }

		String SelectLanguage
		{ get; }

		String Configuration
		{ get; }
	}

	public interface ILanguageQuestionDialog
	{
		String Title
		{ get; }

		String SelectQuestion
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

	public class LanguageMainForm : ILanguageMainForm
	{
		#region Properties

		[XmlElement]
		public String Title
		{ get; set; }

		[XmlElement]
		public String CreateNew
		{ get; set; }

		[XmlElement]
		public String Load
		{ get; set; }

		[XmlElement]
		public String Save
		{ get; set; }

		[XmlElement]
		public String SaveAs
		{ get; set; }

		[XmlElement]
		public String CreateTest
		{ get; set; }

		[XmlElement]
		public String DescribeKnowledge
		{ get; set; }

		[XmlElement]
		public String CheckKnowledge
		{ get; set; }

		[XmlElement]
		public String AskQuestion
		{ get; set; }

		[XmlElement]
		public String SelectLanguage
		{ get; set; }

		[XmlElement]
		public String Configuration
		{ get; set; }

		#endregion

		internal static LanguageMainForm CreateDefault()
		{
			return new LanguageMainForm
			{
				Title = "Вспомогательная утилита \"Изобретатель\" (демо-версия)",
				CreateNew = "Создать новую базу знаний",
				Load = "Открыть...",
				Save = "Сохранить",
				SaveAs = "Сохранить как...",
				CreateTest = "Создать тестовую базу знаний",
				DescribeKnowledge = "Описать все знания...",
				CheckKnowledge = "Выполнить проверку знаний на непротиворечивость...",
				AskQuestion = "Задать вопрос...",
				SelectLanguage = "Язык:",
				Configuration = "Настройки...",
			};
		}
	}

	public class LanguageQuestionDialog : ILanguageQuestionDialog
	{
		#region Properties

		[XmlElement]
		public String Title
		{ get; set; }

		[XmlElement]
		public String SelectQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionDialog CreateDefault()
		{
			return new LanguageQuestionDialog
			{
				Title = "Формирование вопроса",
				SelectQuestion = "Выберите вопрос: ",
			};
		}
	}
}
