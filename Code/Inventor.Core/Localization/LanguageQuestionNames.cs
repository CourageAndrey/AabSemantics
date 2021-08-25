using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public String EnumerateDescendantsQuestion
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
		public String ComparisonQuestion
		{ get; set; }

		[XmlElement]
		public String ProcessesQuestion
		{ get; set; }

		[XmlElement]
		public String GetCommonQuestion
		{ get; set; }

		[XmlElement]
		public String GetDifferencesQuestion
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

		[XmlElement]
		public String ParamStatement
		{ get; set; }

		[XmlElement]
		public String ParamConditions
		{ get; set; }

		[XmlElement]
		public String ParamQuestion
		{ get; set; }

		[XmlElement]
		public String ParamLeftValue
		{ get; set; }

		[XmlElement]
		public String ParamRightValue
		{ get; set; }

		[XmlElement]
		public String ParamProcessA
		{ get; set; }

		[XmlElement]
		public String ParamProcessB
		{ get; set; }

		[XmlElement]
		public String ParamConcept1
		{ get; set; }

		[XmlElement]
		public String ParamConcept2
		{ get; set; }

		#endregion

		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				EnumerateDescendantsQuestion = "What are CONCEPTs?",
				IsQuestion = "Is DESCENDANT the child of ANCESTOR parent?",
				WhatQuestion = "What CONCEPT is?",
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
				CheckStatementQuestion = "Is this true, that...",
				ComparisonQuestion = "Compare LEFT_VALUE and RIGHT_VALUE",
				ProcessesQuestion = "Compare mutual sequence of PROCESS_A and PROCESS_B",
				GetCommonQuestion = "What in common CONCEPT_1 and CONCEPT_2 have?",
				GetDifferencesQuestion = "What is the difference between CONCEPT_1 and CONCEPT_2?",
				ParamParent = "PARENT",
				ParamChild = "CHILD",
				ParamConcept = "CONCEPT",
				ParamSign = "SIGN",
				ParamRecursive = "Check \"parents\" recursively",
				ParamArea = "SUBJECT_AREA",
				ParamStatement = "Statement",
				ParamConditions = "Preconditions",
				ParamQuestion = "Question",
				ParamLeftValue = "Left value",
				ParamRightValue = "Right value",
				ParamProcessA = "Process A",
				ParamProcessB = "Process B",
				ParamConcept1 = "Concept 1",
				ParamConcept2 = "Concept 2",
			};
		}
	}
}
