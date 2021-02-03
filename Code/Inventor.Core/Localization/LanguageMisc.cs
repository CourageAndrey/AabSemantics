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
				ConsistencyErrorDuplicate = $"Дублирование отношения {Strings.ParamStatement}.",
				ConsistencyErrorCyclic = $"Отношение {Strings.ParamStatement} приводит к циклической ссылке понятий друг на друга.",
				ConsistencyErrorMultipleSubjectArea = $"Предметная область {Strings.ParamArea} задана более одного раза.",
				ConsistencyErrorMultipleSign = $"{Strings.ParamStatement} приводит к повторному определению признака у понятия.",
				ConsistencyErrorMultipleSignValue = $"Значение признака {Strings.ParamSign} понятия {Strings.ParamConcept} не может быть корректно определено, так как задано в нескольких его предках.",
				ConsistencyErrorSignWithoutValue = $"{Strings.ParamStatement} задаёт значение признака, который отсутствует у понятия.",
				Concept = "Понятие",
			};
		}
	}
}
