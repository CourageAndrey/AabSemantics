using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	public interface ILanguageQuestionNames
	{
		String WhatQuestion
		{ get; }

		String FindSubjectAreaQuestion
		{ get; }

		String DescribeSubjectAreaQuestion
		{ get; }

		String SignValueQuestion
		{ get; }

		String EnumerateSignsQuestion
		{ get; }

		String HasSignQuestion
		{ get; }

		String HasSignsQuestion
		{ get; }

		String IsSignQuestion
		{ get; }

		String IsValueQuestion
		{ get; }

		String IsPartOfQuestion
		{ get; }

		String EnumeratePartsQuestion
		{ get; }

		String EnumerateContainersQuestion
		{ get; }

		String IsSubjectAreaQuestion
		{ get; }

		String GetCommonQuestion
		{ get; }

		String GetDifferencesQuestion
		{ get; }
	}

	[XmlType("SetsQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

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
		public String GetCommonQuestion
		{ get; set; }

		[XmlElement]
		public String GetDifferencesQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				WhatQuestion = "What CONCEPT is (details)?",
				FindSubjectAreaQuestion = "What subject area does CONCEPT belong to?",
				DescribeSubjectAreaQuestion = "What concepts are included in the subject area CONCEPT?",
				SignValueQuestion = "What is the SIGN value of the CONCEPT?",
				EnumerateSignsQuestion = "What signs does Concept have?",
				HasSignQuestion = "Does CONCEPT have SIGN?",
				HasSignsQuestion = "Does CONCEPT have signs?",
				IsSignQuestion = "Is CONCEPT a sign?",
				IsValueQuestion = "Is CONCEPT a sign value?",
				IsPartOfQuestion = "Is CHILD a part of PARENT?",
				EnumeratePartsQuestion = "What are the constituent parts of the CONCEPT?",
				EnumerateContainersQuestion = "What can a CONCEPT be an part of?",
				IsSubjectAreaQuestion = "Does CONCEPT belong to SUBJECT_AREA?",
				GetCommonQuestion = "What in common CONCEPT_1 and CONCEPT_2 have?",
				GetDifferencesQuestion = "What is the difference between CONCEPT_1 and CONCEPT_2?",
			};
		}
	}
}
