using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageMisc : ILanguageMisc
	{
		#region Properties

		[XmlElement]
		public String NameKnowledgeBase
		{ get; set; }

		[XmlElement]
		public String NameCategoryConcepts
		{ get; set; }

		[XmlElement]
		public String NameCategoryStatements
		{ get; set; }

		[XmlElement]
		public String StrictEnumeration
		{ get; set; }

		[XmlElement]
		public String ClasificationSign
		{ get; set; }

		[XmlElement]
		public String NewKbName
		{ get; set; }

		[XmlElement]
		public String Rules
		{ get; set; }

		[XmlElement]
		public String Answer
		{ get; set; }

		[XmlElement]
		public String Required
		{ get; set; }

		[XmlElement]
		public String CheckResult
		{ get; set; }

		[XmlElement]
		public String CheckOk
		{ get; set; }

		[XmlElement]
		public String DialogKbOpenTitle
		{ get; set; }

		[XmlElement]
		public String DialogKbSaveTitle
		{ get; set; }

		[XmlElement]
		public String DialogKbFileFilter
		{ get; set; }

		[XmlElement]
		public String ConsistencyErrorDuplicate
		{ get; set; }

		[XmlElement]
		public String ConsistencyErrorCyclic
		{ get; set; }

		[XmlElement]
		public String ConsistencyErrorMultipleSubjectArea
		{ get; set; }

		[XmlElement]
		public String ConsistencyErrorMultipleSign
		{ get; set; }

		[XmlElement]
		public String ConsistencyErrorMultipleSignValue
		{ get; set; }

		[XmlElement]
		public String ConsistencyErrorSignWithoutValue
		{ get; set; }

		[XmlElement]
		public String True
		{ get; set; }

		[XmlElement]
		public String False
		{ get; set; }

		[XmlElement]
		public String TrueHint
		{ get; set; }

		[XmlElement]
		public String FalseHint
		{ get; set; }

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		internal static LanguageMisc CreateDefault()
		{
			return new LanguageMisc
			{
				NameKnowledgeBase = "База знаний",
				NameCategoryConcepts = "Понятия",
				NameCategoryStatements = "Утверждения",
				StrictEnumeration = " и не может принимать другое значение",
				ClasificationSign = " по признаку \"{0}\"",
				NewKbName = "Новая база знаний",
				Rules = "Все правила базы знаний:",
				Answer = "Ответ:",
				Required = "обязательный",
				CheckResult = "Результат проверки",
				CheckOk = "В результате проверки ошибок не выявлено.",
				DialogKbOpenTitle = "Загрузка базы знаний",
				DialogKbSaveTitle = "Сохранение базы знаний",
				DialogKbFileFilter = "XML с базой знаний|*.xml",
				ConsistencyErrorDuplicate = "Дублирование отношения #STATEMENT#.",
				ConsistencyErrorCyclic = "Отношение #STATEMENT# приводит к циклической ссылке понятий друг на друга.",
				ConsistencyErrorMultipleSubjectArea = "Предметная область #AREA# задана более одного раза.",
				ConsistencyErrorMultipleSign = "#STATEMENT# приводит к повторному определению признака у понятия.",
				ConsistencyErrorMultipleSignValue = "Значение признака #SIGN# понятия #CONCEPT# не может быть корректно определено, так как задано в нескольких его предках.",
				ConsistencyErrorSignWithoutValue = "#STATEMENT# задаёт значение признака, который отсутствует у понятия.",
				True = "Да",
				False = "Нет",
				TrueHint = "Логическое значение: истина.",
				FalseHint = "Логическое значение: ложь.",
				Concept = "Понятие",
			};
		}
	}
}
