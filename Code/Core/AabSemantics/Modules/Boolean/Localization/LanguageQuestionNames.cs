using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Boolean.Localization
{
	public interface ILanguageQuestionNames
	{
		String CheckStatementQuestion
		{ get; }
	}

	[XmlType("BooleanQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public String CheckStatementQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				CheckStatementQuestion = "Is this true, that...",
			};
		}
	}
}
