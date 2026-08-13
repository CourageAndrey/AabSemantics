using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	/// <summary>Captions of the parameters a question asks the user for.</summary>
	public interface ILanguageQuestionParameters
	{
		/// <summary>Caption of the "more general side" parameter.</summary>
		String Parent
		{ get; }

		/// <summary>Caption of the "more specific side" parameter.</summary>
		String Child
		{ get; }

		/// <summary>Caption of the "concept" parameter.</summary>
		String Concept
		{ get; }

		/// <summary>Caption of the flag that includes inherited knowledge.</summary>
		String Recursive
		{ get; }

		/// <summary>Caption of the hypothetical preconditions parameter.</summary>
		String Conditions
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionParameters"/>, loaded from a language file.</summary>
	[XmlType("CommonQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		/// <summary>Caption of the "more general side" parameter.</summary>
		[XmlElement]
		public String Parent
		{ get; set; }

		/// <summary>Caption of the "more specific side" parameter.</summary>
		[XmlElement]
		public String Child
		{ get; set; }

		/// <summary>Caption of the "concept" parameter.</summary>
		[XmlElement]
		public String Concept
		{ get; set; }

		/// <summary>Caption of the flag that includes inherited knowledge.</summary>
		[XmlElement]
		public String Recursive
		{ get; set; }

		/// <summary>
		/// Caption of the "statement" parameter. Serialized and defaulted, but absent from
		/// <see cref="ILanguageQuestionParameters"/>, so it is reachable only through this class.
		/// </summary>
		[XmlElement]
		public String Statement
		{ get; set; }

		/// <summary>Caption of the hypothetical preconditions parameter.</summary>
		[XmlElement]
		public String Conditions
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
