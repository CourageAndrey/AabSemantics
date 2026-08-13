using System;
using System.Globalization;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	/// <summary>
	/// One field per statement type of the classification module. The same type is reused for
	/// names, hints and the three wordings, which is why the defaults come from five separate
	/// factory methods.
	/// </summary>
	public interface ILanguageStatementsPart
	{
		/// <summary>Text for the "is a" statement.</summary>
		String Classification
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageStatementsPart"/>, loaded from a language file.</summary>
	[XmlType("ClassificationStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		/// <summary>Text for the "is a" statement.</summary>
		[XmlElement]
		public String Classification
		{ get; set; }

		#endregion

		/// <summary>Builds the built-in English display names.</summary>
		/// <returns>A populated part.</returns>
		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Classification = "Classification",
			};
		}

		/// <summary>Builds the built-in English tooltip texts.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Classification = "Statement declares, that one concept (descendant) is (implements, instantiates, subclass) another one (ancestor).",
			};
		}

		/// <summary>Builds the built-in English affirmative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Classification = String.Format(CultureInfo.InvariantCulture, "{0} is {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		/// <summary>Builds the built-in English negative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Classification = String.Format(CultureInfo.InvariantCulture, "{0} is not {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		/// <summary>Builds the built-in English interrogative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Classification = String.Format(CultureInfo.InvariantCulture, "Is {0} a {1}?", Strings.ParamChild, Strings.ParamParent),
			};
		}
	}
}
