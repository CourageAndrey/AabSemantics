using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		[XmlElement]
		public String CheckResult
		{ get; set; }

		[XmlElement]
		public String CheckOk
		{ get; set; }

		[XmlElement]
		public String ErrorDuplicate
		{ get; set; }

		[XmlElement]
		public String ErrorCyclic
		{ get; set; }

		[XmlElement]
		public String ErrorMultipleSubjectArea
		{ get; set; }

		[XmlElement]
		public String ErrorMultipleSign
		{ get; set; }

		[XmlElement]
		public String ErrorMultipleSignValue
		{ get; set; }

		[XmlElement]
		public String ErrorSignWithoutValue
		{ get; set; }

		[XmlElement]
		public String ErrorComparisonContradiction
		{ get; set; }

		[XmlElement]
		public String ErrorProcessesContradiction
		{ get; set; }

	#endregion

	internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				CheckResult = "Результат проверки",
				CheckOk = "В результате проверки ошибок не выявлено.",
				ErrorDuplicate = $"Дублирование отношения {Strings.ParamStatement}.",
				ErrorCyclic = $"Отношение {Strings.ParamStatement} приводит к циклической ссылке понятий друг на друга.",
				ErrorMultipleSubjectArea = $"Предметная область {Strings.ParamArea} задана более одного раза.",
				ErrorMultipleSign = $"{Strings.ParamStatement} приводит к повторному определению признака у понятия.",
				ErrorMultipleSignValue = $"Значение признака {Strings.ParamSign} понятия {Strings.ParamConcept} не может быть корректно определено, так как задано в нескольких его предках.",
				ErrorSignWithoutValue = $"{Strings.ParamStatement} задаёт значение признака, который отсутствует у понятия.",
				ErrorComparisonContradiction = $"Невозможно сравнить {Strings.ParamLeftValue} и {Strings.ParamRightValue}. Возможные значения: ",
				ErrorProcessesContradiction = $"Невозможно установить взаимную последовательность {Strings.ParamProcessA} и {Strings.ParamProcessB}. Возможные варианты: ",
			};
		}
	}
}
