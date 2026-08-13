using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Boolean.Localization
{
	/// <summary>Display names of the boolean module's questions.</summary>
	public interface ILanguageQuestionNames
	{
		/// <summary>Display name of the "is this statement true" question.</summary>
		String CheckStatementQuestion
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionNames"/>, loaded from a language file.</summary>
	[XmlType("BooleanQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		/// <summary>Display name of the "is this statement true" question.</summary>
		[XmlElement]
		public String CheckStatementQuestion
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				CheckStatementQuestion = "Is this true, that...",
			};
		}
	}
}
