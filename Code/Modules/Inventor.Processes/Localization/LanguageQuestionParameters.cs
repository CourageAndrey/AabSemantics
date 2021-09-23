using System;
using System.Xml.Serialization;

namespace Inventor.Processes.Localization
{
	public interface ILanguageQuestionParameters
	{
		String ProcessA
		{ get; }

		String ProcessB
		{ get; }
	}

	[XmlType("ProcessesQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String ProcessA
		{ get; set; }

		[XmlElement]
		public String ProcessB
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				ProcessA = "Process A",
				ProcessB = "Process B",
			};
		}
	}
}
