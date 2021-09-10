using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageAnswers
	{
		String Unknown
		{ get; }

		String RecursiveTrue
		{ get; }

		String RecursiveFalse
		{ get; }

		String Explanation
		{ get; }
	}

	[XmlType("CommonAnswers")]
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		[XmlElement]
		public String Unknown
		{ get; set; }

		[XmlElement]
		public String RecursiveTrue
		{ get; set; }

		[XmlElement]
		public String RecursiveFalse
		{ get; set; }

		[XmlElement]
		public String Explanation
		{ get; set; }

		#endregion

		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				Unknown = "Impossible to answer (there is no corresponding information).",
				RecursiveTrue = " (including parents)",
				RecursiveFalse = " (without parents)",
				Explanation = "Explanation:",
			};
		}
	}
}
