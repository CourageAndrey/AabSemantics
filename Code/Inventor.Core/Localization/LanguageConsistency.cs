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

		#endregion

		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				CheckResult = "Результат проверки",
				CheckOk = "В результате проверки ошибок не выявлено.",
				ConsistencyErrorDuplicate = $"Дублирование отношения {Strings.ParamStatement}.",
				ConsistencyErrorCyclic = $"Отношение {Strings.ParamStatement} приводит к циклической ссылке понятий друг на друга.",
				ConsistencyErrorMultipleSubjectArea = $"Предметная область {Strings.ParamArea} задана более одного раза.",
				ConsistencyErrorMultipleSign = $"{Strings.ParamStatement} приводит к повторному определению признака у понятия.",
				ConsistencyErrorMultipleSignValue = $"Значение признака {Strings.ParamSign} понятия {Strings.ParamConcept} не может быть корректно определено, так как задано в нескольких его предках.",
				ConsistencyErrorSignWithoutValue = $"{Strings.ParamStatement} задаёт значение признака, который отсутствует у понятия.",
			};
		}
	}
}
