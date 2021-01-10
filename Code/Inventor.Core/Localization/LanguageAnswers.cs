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

		#endregion

		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				Unknown = "Ответ неизвестен (в базе не содержится таких знаний).",
				IsTrue = "Да, #CHILD# есть #PARENT#.",
				IsFalse = "Нет, #CHILD# не есть #PARENT#.",
				IsSubjectAreaTrue = "Да, понятие #CONCEPT# входит в предметную область #AREA#.",
				IsSubjectAreaFalse = "Нет, понятие #CONCEPT# не входит в предметную область #AREA#.",
				IsDescription = "#CHILD# - это #PARENT#.",
				IsDescriptionWithSign = "#CHILD# - это #PARENT#, который имеет следующие отличительные признаки:",
				IsDescriptionWithSignValue = "... значение признака #SIGN# имеет равным #VALUE#",
				SignTrue = "Да, #CONCEPT# является признаком.",
				SignFalse = "Нет, #CONCEPT# не является признаком.",
				ValueTrue = "Да, #CONCEPT# является значением признака.",
				ValueFalse = "Нет, #CONCEPT# не является значением признака.",
				HasSignTrue = "Да, #CONCEPT# имеет признак #SIGN#{0}.",
				HasSignFalse = "Нет, #CONCEPT# не имеет признак #SIGN#{0}.",
				HasSignsTrue = "Да, #CONCEPT# имеет признаки{0}.",
				HasSignsFalse = "Нет, #CONCEPT# не имеет признаков{0}.",
				Enumerate = "#PARENT# бывает следующим: ",
				SubjectArea = "#CONCEPT# входит в предметную область #AREA#.",
				SubjectAreaConcepts = "В предметную область #AREA# входят следующие понятия: ",
				ConceptSigns = "Понятие #CONCEPT# имеет следующие признаки{0}: {1}.",
				SignValue = "#CONCEPT# имеет значение признака #SIGN# равным #VALUE# (значение определено для понятия #DEFINED#).",
				RecursiveTrue = " (с учётом родительских понятий)",
				RecursiveFalse = " (без учёта родительских понятий)",
				IsPartOfTrue = "Да, #CHILD# является частью #PARENT#.",
				IsPartOfFalse = "Нет, #CHILD# не является частью #PARENT#.",
				EnumerateParts = "В #PARENT# входят следующие составные части: ",
				EnumerateContainers = "#CHILD# может выступать в качестве составной части для: ",
			};
		}
	}
}
