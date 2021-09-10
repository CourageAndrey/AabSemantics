using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Boolean
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
