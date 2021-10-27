using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Inventor.Semantics.Localization.Modules.Classification
{
	public interface ILanguageStatementsPart
	{
		String Clasification
		{ get; }
	}

	[XmlType("ClassificationStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		[XmlElement]
		public String Clasification
		{ get; set; }

		#endregion

		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Clasification = "Clasification",
			};
		}

		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Clasification = "Statement declares, that one concept (descendant) is (implements, instantiates, subclass) another one (ancestor).",
			};
		}

		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Clasification = String.Format(CultureInfo.InvariantCulture, "{0} is {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Clasification = String.Format(CultureInfo.InvariantCulture, "{0} is not {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Clasification = String.Format(CultureInfo.InvariantCulture, "Is {0} a {1}?", Strings.ParamChild, Strings.ParamParent),
			};
		}
	}
}
