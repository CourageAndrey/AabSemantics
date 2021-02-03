using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageStatements : ILanguageStatements
	{
		#region Properties

		[XmlElement]
		public String SubjectArea
		{ get; set; }

		[XmlElement]
		public String Clasification
		{ get; set; }

		[XmlElement]
		public String HasSign
		{ get; set; }

		[XmlElement]
		public String SignValue
		{ get; set; }

		[XmlElement]
		public String Composition
		{ get; set; }

		[XmlElement]
		public String Comparison
		{ get; set; }

		[XmlElement]
		public String Processes
		{ get; set; }

		#endregion

		public static LanguageStatements CreateDefaultNames()
		{
			return new LanguageStatements
			{
				SubjectArea = "Предметная область",
				Clasification = "Классификация",
				HasSign = "Признак",
				SignValue = "Значение признака",
				Composition = "Часть-целое",
				Comparison = "Сравнение",
				Processes = "Процессы",
			};
		}

		internal static LanguageStatements CreateDefaultHints()
		{
			return new LanguageStatements
			{
				SubjectArea = "Отношение описывает вхождение терминов в некоторую предметную область.",
				Clasification = "Отношение описывает связь между родительским и дочерним (подчинённым) понятиями.",
				HasSign = "Отношение описывает наличие некоторого признака, описывающего свойства понятия.",
				SignValue = "Отношение описывает значение признака, свойственного данному понятию.",
				Composition = "Отношение описывает вхождение одного понятия в другое качестве элемента структуры последнего.",
				Comparison = "Отношение описывает сравнение двух значений",
				Processes = "Отношение описывает временные и причинно-следственные отношения между процессами",
			};
		}

		internal static LanguageStatements CreateDefaultTrue()
		{
			return new LanguageStatements
			{
				SubjectArea = string.Format("Понятие {0} входит в предметную область {1}.", Strings.ParamConcept, Strings.ParamArea),
				Clasification = string.Format("{0} есть {1}.", Strings.ParamChild, Strings.ParamParent),
				HasSign = string.Format("{0} имеет признак {1}.", Strings.ParamConcept, Strings.ParamSign),
				SignValue = string.Format("{0} имеет значение признака {1} равным {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = string.Format("{0} является частью {1}.", Strings.ParamChild, Strings.ParamParent),
				Comparison = string.Format("{0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
				Processes = string.Format("{0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		internal static LanguageStatements CreateDefaultFalse()
		{
			return new LanguageStatements
			{
				SubjectArea = string.Format("Понятие {0} не входит в предметную область {1}.", Strings.ParamConcept, Strings.ParamArea),
				Clasification = string.Format("{0} не есть {1}.", Strings.ParamChild, Strings.ParamParent),
				HasSign = string.Format("У {0} отсутствует признак {1}.", Strings.ParamConcept, Strings.ParamSign),
				SignValue = string.Format("{0} не имеет значение признака {1} равным {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = string.Format("{0} не является частью {1}.", Strings.ParamChild, Strings.ParamParent),
				Comparison = string.Format("Неверно, что {0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
				Processes = string.Format("Неверно, что {0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		internal static LanguageStatements CreateDefaultQuestion()
		{
			return new LanguageStatements
			{
				SubjectArea = string.Format("Входит ли понятие {0} в предметную область {1}?", Strings.ParamConcept, Strings.ParamArea),
				Clasification = string.Format("На самом ли деле {0} есть {1}?", Strings.ParamChild, Strings.ParamParent),
				HasSign = string.Format("Есть ли у {0} признак {1}?", Strings.ParamConcept, Strings.ParamSign),
				SignValue = string.Format("Является ли {2} значением признака {1} у {0}?", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = string.Format("Является ли {0} частью {1}?", Strings.ParamChild, Strings.ParamParent),
				Comparison = string.Format("Верно ли, что {0} {1} {2}?", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
				Processes = string.Format("Верно ли, что {0} {1} {2}?", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}
	}
}
