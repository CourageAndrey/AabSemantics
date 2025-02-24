using System;
using System.Globalization;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	public interface ILanguageStatementsPart
	{
		String Classification
		{ get; }
	}

	[XmlType("ClassificationStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		[XmlElement]
		public String Classification
		{ get; set; }

		#endregion

		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Classification = "Classification",
			};
		}

		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Classification = "Statement declares, that one concept (descendant) is (implements, instantiates, subclass) another one (ancestor).",
			};
		}

		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Classification = String.Format(CultureInfo.InvariantCulture, "{0} is {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Classification = String.Format(CultureInfo.InvariantCulture, "{0} is not {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Classification = String.Format(CultureInfo.InvariantCulture, "Is {0} a {1}?", Strings.ParamChild, Strings.ParamParent),
			};
		}
	}
}
