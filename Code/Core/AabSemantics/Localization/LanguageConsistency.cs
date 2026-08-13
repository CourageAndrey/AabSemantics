using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	/// <summary>Wordings used when reporting the result of a consistency check.</summary>
	public interface ILanguageConsistency
	{
		/// <summary>Caption introducing the check's findings.</summary>
		String CheckResult
		{ get; }

		/// <summary>Message shown when no problems were found.</summary>
		String CheckOk
		{ get; }

		/// <summary>Message reporting a duplicated statement; refers to <see cref="Strings.ParamStatement"/>.</summary>
		String ErrorDuplicate
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageConsistency"/>, loaded from a language file.</summary>
	[XmlType("CommonConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		/// <summary>Caption introducing the check's findings.</summary>
		[XmlElement]
		public String CheckResult
		{ get; set; }

		/// <summary>Message shown when no problems were found.</summary>
		[XmlElement]
		public String CheckOk
		{ get; set; }

		/// <summary>Message reporting a duplicated statement.</summary>
		[XmlElement]
		public String ErrorDuplicate
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				CheckResult = "Check result",
				CheckOk = "There is no errors.",
				ErrorDuplicate = $"Statement {Strings.ParamStatement} is duplicated.",
			};
		}
	}
}
