using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageQuestionParameters
	{
		String Parent
		{ get; }

		String Child
		{ get; }

		String Concept
		{ get; }

		String Recursive
		{ get; }

		String Conditions
		{ get; }
	}

	[XmlType("CommonQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String Parent
		{ get; set; }

		[XmlElement]
		public String Child
		{ get; set; }

		[XmlElement]
		public String Concept
		{ get; set; }

		[XmlElement]
		public String Recursive
		{ get; set; }

		[XmlElement]
		public String Statement
		{ get; set; }

		[XmlElement]
		public String Conditions
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				Parent = "PARENT",
				Child = "CHILD",
				Concept = "CONCEPT",
				Recursive = "Check \"parents\" recursively",
				Statement = "Statement",
				Conditions = "Preconditions",
			};
		}
	}
}
