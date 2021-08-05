using System;
using System.Xml.Serialization;

namespace Inventor.Client.Localization
{
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
		public String ContextMenuRename
		{ get; set; }

		[XmlElement]
		public String ContextMenuKnowledgeAdd
		{ get; set; }

		[XmlElement]
		public String ContextMenuKnowledgeEdit
		{ get; set; }

		[XmlElement]
		public String ContextMenuKnowledgeDelete
		{ get; set; }

		[XmlElement]
		public String SavePromt
		{ get; set; }

		[XmlElement]
		public String SaveTitle
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
				ContextMenuRename = "Переименовать...",
				ContextMenuKnowledgeAdd = "Добавить...",
				ContextMenuKnowledgeEdit = "Редактировать...",
				ContextMenuKnowledgeDelete = "Удалить",
				SavePromt = "Файл был изменён. Сохранить изменения?",
				SaveTitle = "Сохранение изменений",
			};
		}
	}
}