using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	/// <summary>Display names of the mathematics module's questions.</summary>
	public interface ILanguageQuestionNames
	{
		/// <summary>Display name of the comparison question.</summary>
		String ComparisonQuestion
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionNames"/>, loaded from a language file.</summary>
	[XmlType("MathematicsQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		/// <summary>Display name of the comparison question.</summary>
		[XmlElement]
		public String ComparisonQuestion
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				ComparisonQuestion = "Compare LEFT_VALUE and RIGHT_VALUE",
			};
		}
	}
}
