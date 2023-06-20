using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	public interface ILanguageStatementsPart
	{
		String Comparison
		{ get; }
	}

	[XmlType("MathematicsStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		[XmlElement]
		public String Comparison
		{ get; set; }

		#endregion

		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Comparison = "Comparison",
			};
		}

		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Comparison = "Statement declares, how two values can be compared with each other.",
			};
		}

		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Comparison = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
			};
		}

		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Comparison = String.Format(CultureInfo.InvariantCulture, "It's false, that {0} {1} {2}.", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
			};
		}

		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Comparison = String.Format(CultureInfo.InvariantCulture, "Is it true, that {0} {1} {2}?", Strings.ParamLeftValue, Strings.ParamComparisonSign, Strings.ParamRightValue),
			};
		}
	}
}
