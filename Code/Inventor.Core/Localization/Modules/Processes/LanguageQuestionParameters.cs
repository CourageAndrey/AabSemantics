using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Processes
{
	public interface ILanguageQuestionParameters
	{
		String ParamProcessA
		{ get; }

		String ParamProcessB
		{ get; }
	}

	[XmlType("ProcessesQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String ParamProcessA
		{ get; set; }

		[XmlElement]
		public String ParamProcessB
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				ParamProcessA = "Process A",
				ParamProcessB = "Process B",
			};
		}
	}
}
