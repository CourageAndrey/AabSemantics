using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Classification.Localization
{
	/// <summary>Display names of the classification module's questions.</summary>
	public interface ILanguageQuestionNames
	{
		/// <summary>Display name of the "which ancestors" question.</summary>
		String EnumerateAncestorsQuestion
		{ get; }

		/// <summary>Display name of the "which descendants" question.</summary>
		String EnumerateDescendantsQuestion
		{ get; }

		/// <summary>Display name of the "is a" question.</summary>
		String IsQuestion
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionNames"/>, loaded from a language file.</summary>
	[XmlType("ClassificationQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		/// <summary>Display name of the "which ancestors" question.</summary>
		[XmlElement]
		public String EnumerateAncestorsQuestion
		{ get; set; }

		/// <summary>Display name of the "which descendants" question.</summary>
		[XmlElement]
		public String EnumerateDescendantsQuestion
		{ get; set; }

		/// <summary>Display name of the "is a" question.</summary>
		[XmlElement]
		public String IsQuestion
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
