using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageStatements : ILanguageStatements
	{
		#region Properties

		[XmlElement]
		public String SubjectArea
		{ get; set; }

		[XmlElement]
		public String Clasification
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

		[XmlElement]
		public String Comparison
		{ get; set; }

		[XmlElement]
		public String Processes
		{ get; set; }

		#endregion

		public static LanguageStatements CreateDefaultNames()
		{
			return new LanguageStatements
			{
				SubjectArea = "Subject Area",
				Clasification = "Clasification",
				HasSign = "Has Sign",
				SignValue = "Sign Value",
				Composition = "Composition",
				Comparison = "Comparison",
				Processes = "Processes",
			};
		}

		internal static LanguageStatements CreateDefaultHints()
		{
			return new LanguageStatements
			{
				SubjectArea = "Statement declares, that concept belongs to some subject area.",
				Clasification = "Statement declares, that one concept (descendant) is (implements, instantiates, subclass) another one (ancestor).",
				HasSign = "Statement declares, that concept has certain sign.",
				SignValue = "Statement declares, that concept has defined sign value.",
				Composition = "Statement declares, that one concept is a part of another one.",
				Comparison = "Statement declares, how two values can be compared with each other.",
				Processes = "Statement declares, how two processes relate one to other on the time scale.",
			};
		}

		internal static LanguageStatements CreateDefaultTrue()
		{
			return new LanguageStatements
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Concept {0} belongs to {1} subject area.", Strings.ParamConcept, Strings.ParamArea),
				Clasification = String.Format(CultureInfo.InvariantCulture, "{0} is {1}.", Strings.ParamChild, Strings.ParamParent),
				HasSign = String.Format(CultureInfo.InvariantCulture, "{0} has {1} sign.", Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "{1} sign value of {0} is equal to {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "{0} is part of {1}.", Strings.ParamChild, Strings.ParamParent),
				Comparison = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
				Processes = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		internal static LanguageStatements CreateDefaultFalse()
		{
			return new LanguageStatements
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Concept {0} does not belong to {1} subject area.", Strings.ParamConcept, Strings.ParamArea),
				Clasification = String.Format(CultureInfo.InvariantCulture, "{0} is not {1}.", Strings.ParamChild, Strings.ParamParent),
				HasSign = String.Format(CultureInfo.InvariantCulture, "{0} has not {1} sign.", Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "{1} sign value of {0} is not equal to {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "{0} is not part of {1}.", Strings.ParamChild, Strings.ParamParent),
				Comparison = String.Format(CultureInfo.InvariantCulture, "It's false, that {0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
				Processes = String.Format(CultureInfo.InvariantCulture, "It's false, that {0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		internal static LanguageStatements CreateDefaultQuestion()
		{
			return new LanguageStatements
			{
				SubjectArea = String.Format(CultureInfo.InvariantCulture, "Does {0} belong to {1} subject area?", Strings.ParamConcept, Strings.ParamArea),
				Clasification = String.Format(CultureInfo.InvariantCulture, "Is {0} a {1}?", Strings.ParamChild, Strings.ParamParent),
				HasSign = String.Format(CultureInfo.InvariantCulture, "Has {0} got {1} sign?", Strings.ParamConcept, Strings.ParamSign),
				SignValue = String.Format(CultureInfo.InvariantCulture, "Is {2} the value of {0}s {1} sign?", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = String.Format(CultureInfo.InvariantCulture, "Is {0} part of {1}?", Strings.ParamChild, Strings.ParamParent),
				Comparison = String.Format(CultureInfo.InvariantCulture, "Is it true, that {0} {1} {2}?", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
				Processes = String.Format(CultureInfo.InvariantCulture, "Is it true, that {0} {1} {2}?", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}
	}
}
