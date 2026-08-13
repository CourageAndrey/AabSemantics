using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Boolean.Localization
{
	/// <summary>Captions of the parameters the boolean module's questions ask for.</summary>
	public interface ILanguageQuestionParameters
	{
		/// <summary>Caption of the statement being checked.</summary>
		String Statement
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionParameters"/>, loaded from a language file.</summary>
	[XmlType("BooleanQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		/// <summary>Caption of the statement being checked.</summary>
		[XmlElement]
		public String Statement
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				Statement = "Statement",
			};
		}
	}
}
