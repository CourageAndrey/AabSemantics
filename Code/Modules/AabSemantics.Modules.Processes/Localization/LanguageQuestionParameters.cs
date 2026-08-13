using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Processes.Localization
{
	/// <summary>Captions of the parameters the processes module's questions ask for.</summary>
	public interface ILanguageQuestionParameters
	{
		/// <summary>Caption of the first process parameter.</summary>
		String ProcessA
		{ get; }

		/// <summary>Caption of the second process parameter.</summary>
		String ProcessB
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionParameters"/>, loaded from a language file.</summary>
	[XmlType("ProcessesQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		/// <summary>Caption of the first process parameter.</summary>
		[XmlElement]
		public String ProcessA
		{ get; set; }

		/// <summary>Caption of the second process parameter.</summary>
		[XmlElement]
		public String ProcessB
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
