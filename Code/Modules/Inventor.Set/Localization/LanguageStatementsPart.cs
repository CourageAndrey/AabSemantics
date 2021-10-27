using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Inventor.Set.Localization
{
	public interface ILanguageStatementsPart
	{
		String SubjectArea
		{ get; }

		String HasSign
		{ get; }

		String SignValue
		{ get; }

		String Composition
		{ get; }
	}

	[XmlType("SetsStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		[XmlElement]
		public String SubjectArea
		{ get; set; }

		[XmlElement]
		public String HasSign
		{ get; set; }

		[XmlElement]
		public String SignValue
		{ get; set; }

		[XmlElement]
		public String Composition
		{ get; set; }

		#endregion

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

		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Concept {0} belongs to {1} subject area.", Semantics.Localization.Strings.ParamConcept, Strings.ParamArea),
				HasSign = String.Format(CultureInfo.InvariantCulture, "{0} has {1} sign.", Semantics.Localization.Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "{1} sign value of {0} is equal to {2}.", Semantics.Localization.Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "{0} is part of {1}.", Semantics.Localization.Strings.ParamChild, Semantics.Localization.Strings.ParamParent),
			};
		}

		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Concept {0} does not belong to {1} subject area.", Semantics.Localization.Strings.ParamConcept, Strings.ParamArea),
				HasSign = String.Format(CultureInfo.InvariantCulture, "{0} has not {1} sign.", Semantics.Localization.Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "{1} sign value of {0} is not equal to {2}.", Semantics.Localization.Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "{0} is not part of {1}.", Semantics.Localization.Strings.ParamChild, Semantics.Localization.Strings.ParamParent),
			};
		}

		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Does {0} belong to {1} subject area?", Semantics.Localization.Strings.ParamConcept, Strings.ParamArea),
				HasSign = String.Format(CultureInfo.InvariantCulture, "Has {0} got {1} sign?", Semantics.Localization.Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "Is {2} the value of {0}s {1} sign?", Semantics.Localization.Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "Is {0} part of {1}?", Semantics.Localization.Strings.ParamChild, Semantics.Localization.Strings.ParamParent),
			};
		}
	}
}
