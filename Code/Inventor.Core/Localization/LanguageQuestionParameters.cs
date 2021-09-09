using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

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

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
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
