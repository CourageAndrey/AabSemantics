using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>One field per statement type of the set module; reused for names, hints and the three wordings.</summary>
	public interface ILanguageStatementsPart
	{
		/// <summary>Text for the "subject area" statement.</summary>
		String SubjectArea
		{ get; }

		/// <summary>Text for the "has sign" statement.</summary>
		String HasSign
		{ get; }

		/// <summary>Text for the "sign value" statement.</summary>
		String SignValue
		{ get; }

		/// <summary>Text for the "has part" statement.</summary>
		String Composition
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageStatementsPart"/>, loaded from a language file.</summary>
	[XmlType("SetsStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		/// <summary>Text for the "subject area" statement.</summary>
		[XmlElement]
		public String SubjectArea
		{ get; set; }

		/// <summary>Text for the "has sign" statement.</summary>
		[XmlElement]
		public String HasSign
		{ get; set; }

		/// <summary>Text for the "sign value" statement.</summary>
		[XmlElement]
		public String SignValue
		{ get; set; }

		/// <summary>Text for the "has part" statement.</summary>
		[XmlElement]
		public String Composition
		{ get; set; }

		#endregion

		/// <summary>Builds the built-in English display names.</summary>
		/// <returns>A populated part.</returns>
		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = "Subject Area",
				HasSign = "Has Sign",
				SignValue = "Sign Value",
				Composition = "Composition",
			};
		}

		/// <summary>Builds the built-in English tooltip texts.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = "Statement declares, that concept belongs to some subject area.",
				HasSign = "Statement declares, that concept has certain sign.",
				SignValue = "Statement declares, that concept has defined sign value.",
				Composition = "Statement declares, that one concept is a part of another one.",
			};
		}

		/// <summary>Builds the built-in English affirmative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Concept {0} belongs to {1} subject area.", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamArea),
				HasSign = String.Format(CultureInfo.InvariantCulture, "{0} has {1} sign.", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "{1} sign value of {0} is equal to {2}.", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "{0} is part of {1}.", AabSemantics.Localization.Strings.ParamChild, AabSemantics.Localization.Strings.ParamParent),
			};
		}

		/// <summary>Builds the built-in English negative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Concept {0} does not belong to {1} subject area.", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamArea),
				HasSign = String.Format(CultureInfo.InvariantCulture, "{0} has not {1} sign.", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "{1} sign value of {0} is not equal to {2}.", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "{0} is not part of {1}.", AabSemantics.Localization.Strings.ParamChild, AabSemantics.Localization.Strings.ParamParent),
			};
		}

		/// <summary>Builds the built-in English interrogative wordings.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Does {0} belong to {1} subject area?", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamArea),
				HasSign = String.Format(CultureInfo.InvariantCulture, "Has {0} got {1} sign?", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "Is {2} the value of {0}s {1} sign?", AabSemantics.Localization.Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "Is {0} part of {1}?", AabSemantics.Localization.Strings.ParamChild, AabSemantics.Localization.Strings.ParamParent),
			};
		}
	}
}
