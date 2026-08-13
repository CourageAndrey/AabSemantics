using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Boolean.Localization
{
	/// <summary>Question wordings contributed by the boolean module.</summary>
	public interface ILanguageQuestions : ILanguageExtensionQuestions
	{
		/// <summary>Display names of the module's questions.</summary>
		ILanguageQuestionNames Names
		{ get; }

		/// <summary>Captions of the module's question parameters.</summary>
		ILanguageQuestionParameters Parameters
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestions"/>, loaded from a language file.</summary>
	[XmlType("BooleanQuestions")]
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Xml Properties

		/// <summary>Question names, in serializable form.</summary>
		[XmlElement(nameof(Names))]
		public LanguageQuestionNames NamesXml
		{ get; set; }

		/// <summary>Parameter captions, in serializable form.</summary>
		[XmlElement(nameof(Parameters))]
		public LanguageQuestionParameters ParametersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Display names of the module's questions.</summary>
		[XmlIgnore]
		public ILanguageQuestionNames Names
		{ get { return NamesXml; } }

		/// <summary>Captions of the module's question parameters.</summary>
		[XmlIgnore]
		public ILanguageQuestionParameters Parameters
		{ get { return ParametersXml; } }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestions CreateDefault()
		{
			return new LanguageQuestions
			{
				NamesXml = LanguageQuestionNames.CreateDefault(),
				ParametersXml = LanguageQuestionParameters.CreateDefault(),
			};
		}
	}
}
