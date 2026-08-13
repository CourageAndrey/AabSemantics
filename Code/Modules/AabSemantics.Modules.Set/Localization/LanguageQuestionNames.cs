using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>Display names of the set module's questions.</summary>
	public interface ILanguageQuestionNames
	{
		/// <summary>Display name of the "what" question.</summary>
		String WhatQuestion
		{ get; }

		/// <summary>Display name of the "find subject area" question.</summary>
		String FindSubjectAreaQuestion
		{ get; }

		/// <summary>Display name of the "describe subject area" question.</summary>
		String DescribeSubjectAreaQuestion
		{ get; }

		/// <summary>Display name of the "sign value" question.</summary>
		String SignValueQuestion
		{ get; }

		/// <summary>Display name of the "enumerate signs" question.</summary>
		String EnumerateSignsQuestion
		{ get; }

		/// <summary>Display name of the "has sign" question.</summary>
		String HasSignQuestion
		{ get; }

		/// <summary>Display name of the "has signs" question.</summary>
		String HasSignsQuestion
		{ get; }

		/// <summary>Display name of the "is sign" question.</summary>
		String IsSignQuestion
		{ get; }

		/// <summary>Display name of the "is value" question.</summary>
		String IsValueQuestion
		{ get; }

		/// <summary>Display name of the "is part of" question.</summary>
		String IsPartOfQuestion
		{ get; }

		/// <summary>Display name of the "enumerate parts" question.</summary>
		String EnumeratePartsQuestion
		{ get; }

		/// <summary>Display name of the "enumerate containers" question.</summary>
		String EnumerateContainersQuestion
		{ get; }

		/// <summary>Display name of the "is subject area" question.</summary>
		String IsSubjectAreaQuestion
		{ get; }

		/// <summary>Display name of the "get common" question.</summary>
		String GetCommonQuestion
		{ get; }

		/// <summary>Display name of the "get differences" question.</summary>
		String GetDifferencesQuestion
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionNames"/>, loaded from a language file.</summary>
	[XmlType("SetsQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		/// <summary>Display name of the "what" question.</summary>
		[XmlElement]
		public String WhatQuestion
		{ get; set; }

		/// <summary>Display name of the "find subject area" question.</summary>
		[XmlElement]
		public String FindSubjectAreaQuestion
		{ get; set; }

		/// <summary>Display name of the "describe subject area" question.</summary>
		[XmlElement]
		public String DescribeSubjectAreaQuestion
		{ get; set; }

		/// <summary>Display name of the "sign value" question.</summary>
		[XmlElement]
		public String SignValueQuestion
		{ get; set; }

		/// <summary>Display name of the "enumerate signs" question.</summary>
		[XmlElement]
		public String EnumerateSignsQuestion
		{ get; set; }

		/// <summary>Display name of the "has sign" question.</summary>
		[XmlElement]
		public String HasSignQuestion
		{ get; set; }

		/// <summary>Display name of the "has signs" question.</summary>
		[XmlElement]
		public String HasSignsQuestion
		{ get; set; }

		/// <summary>Display name of the "is sign" question.</summary>
		[XmlElement]
		public String IsSignQuestion
		{ get; set; }

		/// <summary>Display name of the "is value" question.</summary>
		[XmlElement]
		public String IsValueQuestion
		{ get; set; }

		/// <summary>Display name of the "is part of" question.</summary>
		[XmlElement]
		public String IsPartOfQuestion
		{ get; set; }

		/// <summary>Display name of the "enumerate parts" question.</summary>
		[XmlElement]
		public String EnumeratePartsQuestion
		{ get; set; }

		/// <summary>Display name of the "enumerate containers" question.</summary>
		[XmlElement]
		public String EnumerateContainersQuestion
		{ get; set; }

		/// <summary>Display name of the "is subject area" question.</summary>
		[XmlElement]
		public String IsSubjectAreaQuestion
		{ get; set; }

		/// <summary>Display name of the "get common" question.</summary>
		[XmlElement]
		public String GetCommonQuestion
		{ get; set; }

		/// <summary>Display name of the "get differences" question.</summary>
		[XmlElement]
		public String GetDifferencesQuestion
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
