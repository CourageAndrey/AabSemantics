using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Processes
{
	public interface ILanguageQuestionNames
	{
		String ProcessesQuestion
		{ get; }
	}

	[XmlType("ProcessesQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public String ProcessesQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				ProcessesQuestion = "Compare mutual sequence of PROCESS_A and PROCESS_B",
			};
		}
	}
}
