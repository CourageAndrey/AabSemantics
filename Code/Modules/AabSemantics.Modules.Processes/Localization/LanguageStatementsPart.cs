using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Processes.Localization
{
	/// <summary>One field per statement type of the processes module; reused for names, hints and the three wordings.</summary>
	public interface ILanguageStatementsPart
	{
		/// <summary>Text for the process sequence statement.</summary>
		String Processes
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageStatementsPart"/>, loaded from a language file.</summary>
	[XmlType("ProcessesStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		/// <summary>Text for the process sequence statement.</summary>
		[XmlElement]
		public String Processes
		{ get; set; }

		#endregion

		/// <summary>Builds the built-in English display names.</summary>
		/// <returns>A populated part.</returns>
		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Processes = "Processes",
			};
		}

		/// <summary>Builds the built-in English tooltip texts.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Processes = "Statement declares, how two processes relate one to other on the time scale.",
			};
		}

		/// <summary>Builds the built-in English affirmative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Processes = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		/// <summary>Builds the built-in English negative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Processes = String.Format(CultureInfo.InvariantCulture, "It's false, that {0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		/// <summary>Builds the built-in English interrogative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Processes = String.Format(CultureInfo.InvariantCulture, "Is it true, that {0} {1} {2}?", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}
	}
}
