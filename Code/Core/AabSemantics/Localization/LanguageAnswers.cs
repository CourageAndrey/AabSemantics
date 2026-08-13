using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	/// <summary>Wordings used when rendering answers.</summary>
	public interface ILanguageAnswers
	{
		/// <summary>Text of the "unknown" answer, used when the network holds no relevant knowledge.</summary>
		String Unknown
		{ get; }

		/// <summary>Suffix noting that inherited knowledge was taken into account.</summary>
		String RecursiveTrue
		{ get; }

		/// <summary>Suffix noting that inherited knowledge was ignored.</summary>
		String RecursiveFalse
		{ get; }

		/// <summary>Caption introducing the statements an answer was derived from.</summary>
		String Explanation
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAnswers"/>, loaded from a language file.</summary>
	[XmlType("CommonAnswers")]
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		/// <summary>Text of the "unknown" answer.</summary>
		[XmlElement]
		public String Unknown
		{ get; set; }

		/// <summary>Suffix noting that inherited knowledge was taken into account.</summary>
		[XmlElement]
		public String RecursiveTrue
		{ get; set; }

		/// <summary>Suffix noting that inherited knowledge was ignored.</summary>
		[XmlElement]
		public String RecursiveFalse
		{ get; set; }

		/// <summary>Caption introducing an answer's explanation.</summary>
		[XmlElement]
		public String Explanation
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				Unknown = "Impossible to answer (there is no corresponding information).",
				RecursiveTrue = " (including parents)",
				RecursiveFalse = " (without parents)",
				Explanation = "Explanation:",
			};
		}
	}
}
