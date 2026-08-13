using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Processes.Localization
{
	/// <summary>Display names of the processes module's questions.</summary>
	public interface ILanguageQuestionNames
	{
		/// <summary>Display name of the process sequence question.</summary>
		String ProcessesQuestion
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionNames"/>, loaded from a language file.</summary>
	[XmlType("ProcessesQuestionNames")]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		/// <summary>Display name of the process sequence question.</summary>
		[XmlElement]
		public String ProcessesQuestion
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				ProcessesQuestion = "Compare mutual sequence of PROCESS_A and PROCESS_B",
			};
		}
	}
}
