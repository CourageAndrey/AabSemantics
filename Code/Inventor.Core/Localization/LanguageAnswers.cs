using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		[XmlElement]
		public String Unknown
		{ get; set; }

		[XmlElement]
		public String IsTrue
		{ get; set; }

		[XmlElement]
		public String IsFalse
		{ get; set; }

		[XmlElement]
		public String IsSubjectAreaTrue
		{ get; set; }

		[XmlElement]
		public String IsSubjectAreaFalse
		{ get; set; }

		[XmlElement]
		public String SignTrue
		{ get; set; }

		[XmlElement]
		public String SignFalse
		{ get; set; }

		[XmlElement]
		public String ValueTrue
		{ get; set; }

		[XmlElement]
		public String ValueFalse
		{ get; set; }

		[XmlElement]
		public String HasSignTrue
		{ get; set; }

		[XmlElement]
		public String HasSignFalse
		{ get; set; }

		[XmlElement]
		public String HasSignsTrue
		{ get; set; }

		[XmlElement]
		public String HasSignsFalse
		{ get; set; }

		[XmlElement]
		public String IsDescription
		{ get; set; }

		[XmlElement]
		public String IsDescriptionWithSign
		{ get; set; }

		[XmlElement]
		public String IsDescriptionWithSignValue
		{ get; set; }

		[XmlElement]
		public String Enumerate
		{ get; set; }

		[XmlElement]
		public String SubjectArea
		{ get; set; }

		[XmlElement]
		public String SubjectAreaConcepts
		{ get; set; }

		[XmlElement]
		public String ConceptSigns
		{ get; set; }

		[XmlElement]
		public String SignValue
		{ get; set; }

		[XmlElement]
		public String RecursiveTrue
		{ get; set; }

		[XmlElement]
		public String RecursiveFalse
		{ get; set; }

		[XmlElement]
		public String IsPartOfTrue
		{ get; set; }

		[XmlElement]
		public String IsPartOfFalse
		{ get; set; }

		[XmlElement]
		public String EnumerateParts
		{ get; set; }

		[XmlElement]
		public String EnumerateContainers
		{ get; set; }

		[XmlElement]
		public String ProcessesSequence
		{ get; set; }

		[XmlElement]
		public String Explanation
		{ get; set; }

		#endregion

		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				Unknown = "Ответ неизвестен (в базе не содержится таких знаний).",
				IsTrue = $"Да, {Strings.ParamChild} есть {Strings.ParamParent}.",
				IsFalse = $"Нет, {Strings.ParamChild} не есть {Strings.ParamParent}.",
				IsSubjectAreaTrue = $"Да, понятие {Strings.ParamConcept} входит в предметную область {Strings.ParamArea}.",
				IsSubjectAreaFalse = $"Нет, понятие {Strings.ParamConcept} не входит в предметную область {Strings.ParamArea}.",
				IsDescription = $"{Strings.ParamChild} - это {Strings.ParamParent}.",
				IsDescriptionWithSign = $"{Strings.ParamChild} - это {Strings.ParamParent}, который имеет следующие отличительные признаки:",
				IsDescriptionWithSignValue = $"... значение признака {Strings.ParamSign} имеет равным {Strings.ParamValue}",
				SignTrue = $"Да, {Strings.ParamConcept} является признаком.",
				SignFalse = $"Нет, {Strings.ParamConcept} не является признаком.",
				ValueTrue = $"Да, {Strings.ParamConcept} является значением признака.",
				ValueFalse = $"Нет, {Strings.ParamConcept} не является значением признака.",
				HasSignTrue = $"Да, {Strings.ParamConcept} имеет признак {Strings.ParamSign}{{0}}.",
				HasSignFalse = $"Нет, {Strings.ParamConcept} не имеет признак {Strings.ParamSign}{{0}}.",
				HasSignsTrue = $"Да, {Strings.ParamConcept} имеет признаки{{0}}.",
				HasSignsFalse = $"Нет, {Strings.ParamConcept} не имеет признаков{{0}}.",
				Enumerate = $"{Strings.ParamParent} бывает следующим: ",
				SubjectArea = $"{Strings.ParamConcept} входит в предметную область {Strings.ParamArea}.",
				SubjectAreaConcepts = $"В предметную область {Strings.ParamArea} входят следующие понятия: ",
				ConceptSigns = $"Понятие {Strings.ParamConcept} имеет следующие признаки{{0}}: {{1}}.",
				SignValue = $"{Strings.ParamConcept} имеет значение признака {Strings.ParamSign} равным {Strings.ParamValue} (значение определено для понятия {Strings.ParamDefined}).",
				RecursiveTrue = $" (с учётом родительских понятий)",
				RecursiveFalse = $" (без учёта родительских понятий)",
				IsPartOfTrue = $"Да, {Strings.ParamChild} является частью {Strings.ParamParent}.",
				IsPartOfFalse = $"Нет, {Strings.ParamChild} не является частью {Strings.ParamParent}.",
				EnumerateParts = $"В {Strings.ParamParent} входят следующие составные части: ",
				EnumerateContainers = $"{Strings.ParamChild} может выступать в качестве составной части для: ",
				ProcessesSequence = $"{Strings.ParamProcessA} {Strings.ParamSequenceSign} {Strings.ParamProcessB}",
				Explanation = "Объяснение:",
			};
		}
	}
}
