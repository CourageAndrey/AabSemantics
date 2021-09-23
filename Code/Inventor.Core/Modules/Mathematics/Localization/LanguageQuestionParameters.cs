using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Mathematics
{
	public interface ILanguageQuestionParameters
	{
		String LeftValue
		{ get; }

		String RightValue
		{ get; }
	}

	[XmlType("MathematicsQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String LeftValue
		{ get; set; }

		[XmlElement]
		public String RightValue
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				LeftValue = "Left value",
				RightValue = "Right value",
			};
		}
	}
}
