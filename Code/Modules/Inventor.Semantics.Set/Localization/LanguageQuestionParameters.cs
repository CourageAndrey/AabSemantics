using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Set.Localization
{
	public interface ILanguageQuestionParameters
	{
		String Sign
		{ get; }

		String Area
		{ get; }
	}

	[XmlType("SetsQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String Sign
		{ get; set; }

		[XmlElement]
		public String Area
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				Sign = "SIGN",
				Area = "SUBJECT_AREA",
			};
		}
	}
}
