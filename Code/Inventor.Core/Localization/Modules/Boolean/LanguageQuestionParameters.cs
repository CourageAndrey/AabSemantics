using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Boolean
{
	public interface ILanguageQuestionParameters
	{
		String ParamStatement
		{ get; }
	}

	[XmlType("BooleanQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String ParamStatement
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				ParamStatement = "Statement",
			};
		}
	}
}
