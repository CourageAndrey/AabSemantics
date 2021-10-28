using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Localization.Modules.Classification
{
	public interface ILanguageQuestionNames
	{
		String EnumerateAncestorsQuestion
		{ get; }

		String EnumerateDescendantsQuestion
		{ get; }

		String IsQuestion
		{ get; }
	}

	[XmlType("ClassificationQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public String EnumerateAncestorsQuestion
		{ get; set; }

		[XmlElement]
		public String EnumerateDescendantsQuestion
		{ get; set; }

		[XmlElement]
		public String IsQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				EnumerateAncestorsQuestion = "What CONCEPT is?",
				EnumerateDescendantsQuestion = "What are CONCEPTs?",
				IsQuestion = "Is DESCENDANT the child of ANCESTOR parent?",
			};
		}
	}
}
