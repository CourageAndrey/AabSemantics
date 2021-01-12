using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public String EnumerateChildrenQuestion
		{ get; set; }

		[XmlElement]
		public String IsQuestion
		{ get; set; }

		[XmlElement]
		public String WhatQuestion
		{ get; set; }

		[XmlElement]
		public String FindSubjectAreaQuestion
		{ get; set; }

		[XmlElement]
		public String DescribeSubjectAreaQuestion
		{ get; set; }

		[XmlElement]
		public String SignValueQuestion
		{ get; set; }

		[XmlElement]
		public String EnumerateSignsQuestion
		{ get; set; }

		[XmlElement]
		public String HasSignQuestion
		{ get; set; }

		[XmlElement]
		public String HasSignsQuestion
		{ get; set; }

		[XmlElement]
		public String IsSignQuestion
		{ get; set; }

		[XmlElement]
		public String IsValueQuestion
		{ get; set; }

		[XmlElement]
		public String IsPartOfQuestion
		{ get; set; }

		[XmlElement]
		public String EnumeratePartsQuestion
		{ get; set; }

		[XmlElement]
		public String EnumerateContainersQuestion
		{ get; set; }

		[XmlElement]
		public String IsSubjectAreaQuestion
		{ get; set; }

		[XmlElement]
		public String CheckStatementQuestion
		{ get; set; }

		[XmlElement]
		public String QuestionWithCondition
		{ get; set; }

		[XmlElement]
		public String ParamParent
		{ get; set; }

		[XmlElement]
		public String ParamChild
		{ get; set; }

		[XmlElement]
		public String ParamConcept
		{ get; set; }

		[XmlElement]
		public String ParamSign
		{ get; set; }

		[XmlElement]
		public String ParamRecursive
		{ get; set; }

		[XmlElement]
		public String ParamArea
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
				QuestionWithCondition = "При условии...",
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
