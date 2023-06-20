using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Modules.Mathematics.Localization
{
	public interface ILanguageQuestionNames
	{
		String ComparisonQuestion
		{ get; }
	}

	[XmlType("MathematicsQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public String ComparisonQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				ComparisonQuestion = "Compare LEFT_VALUE and RIGHT_VALUE",
			};
		}
	}
}
