using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageQuestionParameters
	{
		String ParamParent
		{ get; }

		String ParamChild
		{ get; }

		String ParamConcept
		{ get; }

		String ParamRecursive
		{ get; }

		String ParamConditions
		{ get; }
	}

	[XmlType("CommonQuestionParameters")]
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
		public String ParamRecursive
		{ get; set; }

		[XmlElement]
		public String ParamStatement
		{ get; set; }

		[XmlElement]
		public String ParamConditions
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				ParamParent = "PARENT",
				ParamChild = "CHILD",
				ParamConcept = "CONCEPT",
				ParamRecursive = "Check \"parents\" recursively",
				ParamStatement = "Statement",
				ParamConditions = "Preconditions",
			};
		}
	}
}
