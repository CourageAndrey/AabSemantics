using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Mathematics
{
	public interface ILanguageQuestionParameters
	{
		String ParamLeftValue
		{ get; }

		String ParamRightValue
		{ get; }
	}

	[XmlType("MathematicsQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String ParamLeftValue
		{ get; set; }

		[XmlElement]
		public String ParamRightValue
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				ParamLeftValue = "Left value",
				ParamRightValue = "Right value",
			};
		}
	}
}
