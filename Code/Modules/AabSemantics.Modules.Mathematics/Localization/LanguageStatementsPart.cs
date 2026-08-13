using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	/// <summary>One field per statement type of the mathematics module; reused for names, hints and the three wordings.</summary>
	public interface ILanguageStatementsPart
	{
		/// <summary>Text for the comparison statement.</summary>
		String Comparison
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageStatementsPart"/>, loaded from a language file.</summary>
	[XmlType("MathematicsStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		/// <summary>Text for the comparison statement.</summary>
		[XmlElement]
		public String Comparison
		{ get; set; }

		#endregion

		/// <summary>Builds the built-in English display names.</summary>
		/// <returns>A populated part.</returns>
		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Comparison = "Comparison",
			};
		}

		/// <summary>Builds the built-in English tooltip texts.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Comparison = "Statement declares, how two values can be compared with each other.",
			};
		}

		/// <summary>Builds the built-in English affirmative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Comparison = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
			};
		}

		/// <summary>Builds the built-in English negative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Comparison = String.Format(CultureInfo.InvariantCulture, "It's false, that {0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
			};
		}

		/// <summary>Builds the built-in English interrogative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Comparison = String.Format(CultureInfo.InvariantCulture, "Is it true, that {0} {1} {2}?", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
			};
		}
	}
}
