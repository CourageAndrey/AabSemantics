using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageAnswers
	{
		string Unknown
		{ get; }

		string IsTrue
		{ get; }

		string IsFalse
		{ get; }

		string IsSubjectAreaTrue
		{ get; }

		string IsSubjectAreaFalse
		{ get; }

		string SignTrue
		{ get; }

		string SignFalse
		{ get; }

		string ValueTrue
		{ get; }

		string ValueFalse
		{ get; }

		string HasSignTrue
		{ get; }

		string HasSignFalse
		{ get; }

		string HasSignsTrue
		{ get; }

		string HasSignsFalse
		{ get; }

		string IsDescription
		{ get; }

		string IsDescriptionWithSign
		{ get; }

		string IsDescriptionWithSignValue
		{ get; }

		string Enumerate
		{ get; }

		string SubjectArea
		{ get; }

		string SubjectAreaConcepts
		{ get; }

		string ConceptSigns
		{ get; }

		string SignValue
		{ get; }

		string RecursiveTrue
		{ get; }

		string RecursiveFalse
		{ get; }

		string IsPartOfTrue
		{ get; }

		string IsPartOfFalse
		{ get; }

		string EnumerateParts
		{ get; }

		string EnumerateContainers
		{ get; }
	}

	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		[XmlElement]
		public string Unknown
		{ get; set; }

		[XmlElement]
		public string IsTrue
		{ get; set; }

		[XmlElement]
		public string IsFalse
		{ get; set; }

		[XmlElement]
		public string IsSubjectAreaTrue
		{ get; set; }

		[XmlElement]
		public string IsSubjectAreaFalse
		{ get; set; }

		[XmlElement]
		public string SignTrue
		{ get; set; }

		[XmlElement]
		public string SignFalse
		{ get; set; }

		[XmlElement]
		public string ValueTrue
		{ get; set; }

		[XmlElement]
		public string ValueFalse
		{ get; set; }

		[XmlElement]
		public string HasSignTrue
		{ get; set; }

		[XmlElement]
		public string HasSignFalse
		{ get; set; }

		[XmlElement]
		public string HasSignsTrue
		{ get; set; }

		[XmlElement]
		public string HasSignsFalse
		{ get; set; }

		[XmlElement]
		public string IsDescription
		{ get; set; }

		[XmlElement]
		public string IsDescriptionWithSign
		{ get; set; }

		[XmlElement]
		public string IsDescriptionWithSignValue
		{ get; set; }

		[XmlElement]
		public string Enumerate
		{ get; set; }

		[XmlElement]
		public string SubjectArea
		{ get; set; }

		[XmlElement]
		public string SubjectAreaConcepts
		{ get; set; }

		[XmlElement]
		public string ConceptSigns
		{ get; set; }

		[XmlElement]
		public string SignValue
		{ get; set; }

		[XmlElement]
		public string RecursiveTrue
		{ get; set; }

		[XmlElement]
		public string RecursiveFalse
		{ get; set; }

		[XmlElement]
		public string IsPartOfTrue
		{ get; set; }

		[XmlElement]
		public string IsPartOfFalse
		{ get; set; }

		[XmlElement]
		public string EnumerateParts
		{ get; set; }

		[XmlElement]
		public string EnumerateContainers
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
