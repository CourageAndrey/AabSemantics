using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>Captions of the parameters the set module's questions ask for.</summary>
	public interface ILanguageQuestionParameters
	{
		/// <summary>Caption of the sign parameter.</summary>
		String Sign
		{ get; }

		/// <summary>Caption of the subject area parameter.</summary>
		String Area
		{ get; }

		/// <summary>Caption of the first compared concept.</summary>
		String Concept1
		{ get; }

		/// <summary>Caption of the second compared concept.</summary>
		String Concept2
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionParameters"/>, loaded from a language file.</summary>
	[XmlType("SetsQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		/// <summary>Caption of the sign parameter.</summary>
		[XmlElement]
		public String Sign
		{ get; set; }

		/// <summary>Caption of the subject area parameter.</summary>
		[XmlElement]
		public String Area
		{ get; set; }

		/// <summary>Caption of the first compared concept.</summary>
		[XmlElement]
		public String Concept1
		{ get; set; }

		/// <summary>Caption of the second compared concept.</summary>
		[XmlElement]
		public String Concept2
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				Sign = "SIGN",
				Area = "SUBJECT_AREA",
				Concept1 = "CONCEPT 1",
				Concept2 = "CONCEPT 2",
			};
		}
	}
}
