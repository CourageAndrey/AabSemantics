using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	/// <summary>Wordings of the classification module's answers.</summary>
	public interface ILanguageAnswers
	{
		/// <summary>Affirmative wording of the "is a" answer.</summary>
		String IsTrue
		{ get; }

		/// <summary>Negative wording of the "is a" answer.</summary>
		String IsFalse
		{ get; }

		/// <summary>Caption of the ancestor list.</summary>
		String EnumerateAncestors
		{ get; }

		/// <summary>Caption of the descendant list.</summary>
		String EnumerateDescendants
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAnswers"/>, loaded from a language file.</summary>
	[XmlType("ClassificationAnswers")]
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		/// <summary>Affirmative wording of the "is a" answer.</summary>
		[XmlElement]
		public String IsTrue
		{ get; set; }

		/// <summary>Negative wording of the "is a" answer.</summary>
		[XmlElement]
		public String IsFalse
		{ get; set; }

		/// <summary>Caption of the ancestor list.</summary>
		[XmlElement]
		public String EnumerateAncestors
		{ get; set; }

		/// <summary>Caption of the descendant list.</summary>
		[XmlElement]
		public String EnumerateDescendants
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				IsTrue = $"Yes, {Strings.ParamChild} is {Strings.ParamParent}.",
				IsFalse = $"No, {Strings.ParamChild} is not {Strings.ParamParent}.",
				EnumerateAncestors = $"{Strings.ParamChild} is:",
				EnumerateDescendants = $"{Strings.ParamParent} can be following:",
			};
		}
	}
}
