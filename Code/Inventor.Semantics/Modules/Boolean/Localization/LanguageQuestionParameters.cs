using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Localization.Modules.Boolean
{
	public interface ILanguageQuestionParameters
	{
		String Statement
		{ get; }
	}

	[XmlType("BooleanQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String Statement
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				Statement = "Statement",
			};
		}
	}
}
