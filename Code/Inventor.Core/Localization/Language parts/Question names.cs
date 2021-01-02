using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageQuestionNames
	{
		string EnumerateChildrenQuestion
		{ get; }

		string IsQuestion
		{ get; }

		string WhatQuestion
		{ get; }

		string FindSubjectAreaQuestion
		{ get; }

		string DescribeSubjectAreaQuestion
		{ get; }

		string SignValueQuestion
		{ get; }

		string EnumerateSignsQuestion
		{ get; }

		string HasSignQuestion
		{ get; }

		string HasSignsQuestion
		{ get; }

		string IsSignQuestion
		{ get; }

		string IsValueQuestion
		{ get; }

		string IsPartOfQuestion
		{ get; }

		string EnumeratePartsQuestion
		{ get; }

		string EnumerateContainersQuestion
		{ get; }

		string IsSubjectAreaQuestion
		{ get; }

		string CheckStatementQuestion
		{ get; }

		string ParamParent
		{ get; }

		string ParamChild
		{ get; }

		string ParamConcept
		{ get; }

		string ParamSign
		{ get; }

		string ParamRecursive
		{ get; }

		string ParamArea
		{ get; }
	}

	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public string EnumerateChildrenQuestion
		{ get; set; }

		[XmlElement]
		public string IsQuestion
		{ get; set; }

		[XmlElement]
		public string WhatQuestion
		{ get; set; }

		[XmlElement]
		public string FindSubjectAreaQuestion
		{ get; set; }

		[XmlElement]
		public string DescribeSubjectAreaQuestion
		{ get; set; }

		[XmlElement]
		public string SignValueQuestion
		{ get; set; }

		[XmlElement]
		public string EnumerateSignsQuestion
		{ get; set; }

		[XmlElement]
		public string HasSignQuestion
		{ get; set; }

		[XmlElement]
		public string HasSignsQuestion
		{ get; set; }

		[XmlElement]
		public string IsSignQuestion
		{ get; set; }

		[XmlElement]
		public string IsValueQuestion
		{ get; set; }

		[XmlElement]
		public string IsPartOfQuestion
		{ get; set; }

		[XmlElement]
		public string EnumeratePartsQuestion
		{ get; set; }

		[XmlElement]
		public string EnumerateContainersQuestion
		{ get; set; }

		[XmlElement]
		public string IsSubjectAreaQuestion
		{ get; set; }

		[XmlElement]
		public string CheckStatementQuestion
		{ get; set; }

		[XmlElement]
		public string ParamParent
		{ get; set; }

		[XmlElement]
		public string ParamChild
		{ get; set; }

		[XmlElement]
		public string ParamConcept
		{ get; set; }

		[XmlElement]
		public string ParamSign
		{ get; set; }

		[XmlElement]
		public string ParamRecursive
		{ get; set; }

		[XmlElement]
		public string ParamArea
		{ get; set; }

		#endregion

		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				EnumerateChildrenQuestion = "Какие бывают ПОНЯТИЕ?",
				IsQuestion = "Является ли понятие ДОЧЕРНЕЕ_ПОНЯТИЕ дочерним по отношению к понятию РОДИТЕЛЬСКОЕ_ПОНЯТИЕ?",
				WhatQuestion = "Что такое ПОНЯТИЕ?",
				FindSubjectAreaQuestion = "В какую предметную область входит ПОНЯТИЕ?",
				DescribeSubjectAreaQuestion = "Какие понятия входят в предметную область ПОНЯТИЕ?",
				SignValueQuestion = "Каково значение признака ПРИЗНАК у понятия ПОНЯТИЕ?",
				EnumerateSignsQuestion = "Какие признаки есть у понятия ПОНЯТИЕ?",
				HasSignQuestion = "Есть ли у понятия ПОНЯТИЕ признак ПРИЗНАК?",
				HasSignsQuestion = "Есть ли у понятия ПОНЯТИЕ признаки?",
				IsSignQuestion = "Выступает ли ПОНЯТИЕ в качестве признака?",
				IsValueQuestion = "Выступает ли ПОНЯТИЕ в качестве значения признака?",
				IsPartOfQuestion = "Может ли ДОЧЕРНЕЕ_ПОНЯТИЕ являть составной частью РОДИТЕЛЬСКОЕ_ПОНЯТИЕ?",
				EnumeratePartsQuestion = "Из каких составных частей состоит ПОНЯТИЕ?",
				EnumerateContainersQuestion = "Составной частью чего может являться ПОНЯТИЕ?",
				IsSubjectAreaQuestion = "Входит ли ПОНЯТИЕ в ПРЕДМЕТНАЯ_ОБЛАСТЬ?",
				CheckStatementQuestion = "Верно ли, что...",
				ParamParent = "РОДИТЕЛЬСКОЕ_ПОНЯТИЕ",
				ParamChild = "ДОЧЕРНЕЕ_ПОНЯТИЕ",
				ParamConcept = "ПОНЯТИЕ",
				ParamSign = "ПРИЗНАК",
				ParamRecursive = "Рекурсивно просмотреть \"родителей\"",
				ParamArea = "ПРЕДМЕТНАЯ_ОБЛАСТЬ",
			};
		}
	}
}
