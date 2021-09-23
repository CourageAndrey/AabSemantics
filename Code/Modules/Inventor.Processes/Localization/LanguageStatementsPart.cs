using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Inventor.Processes.Localization
{
	public interface ILanguageStatementsPart
	{
		String Processes
		{ get; }
	}

	[XmlType("ProcessesStatementsPart")]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		[XmlElement]
		public String Processes
		{ get; set; }

		#endregion

		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Processes = "Processes",
			};
		}

		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Processes = "Statement declares, how two processes relate one to other on the time scale.",
			};
		}

		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Processes = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Processes = String.Format(CultureInfo.InvariantCulture, "It's false, that {0} {1} {2}.", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}

		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Processes = String.Format(CultureInfo.InvariantCulture, "Is it true, that {0} {1} {2}?", Strings.ParamProcessA, Strings.ParamSequenceSign, Strings.ParamProcessB),
			};
		}
	}
}
