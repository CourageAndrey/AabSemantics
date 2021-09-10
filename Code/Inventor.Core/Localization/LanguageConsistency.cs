using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageConsistency
	{
		String CheckResult
		{ get; }

		String CheckOk
		{ get; }

		String ErrorDuplicate
		{ get; }
	}

	[XmlType("CommonConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		[XmlElement]
		public String CheckResult
		{ get; set; }

		[XmlElement]
		public String CheckOk
		{ get; set; }

		[XmlElement]
		public String ErrorDuplicate
		{ get; set; }

		#endregion

		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				CheckResult = "Check result",
				CheckOk = "There is no errors.",
				ErrorDuplicate = $"Statement {Strings.ParamStatement} is duplicated.",
			};
		}
	}
}
