using System.Xml.Serialization;

namespace AabSemantics.Modules.Boolean.Localization
{
	public interface ILanguageQuestions
	{
		ILanguageQuestionNames Names
		{ get; }

		ILanguageQuestionParameters Parameters
		{ get; }
	}

	[XmlType("BooleanQuestions")]
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Xml Properties

		[XmlElement(nameof(Names))]
		public LanguageQuestionNames NamesXml
		{ get; set; }

		[XmlElement(nameof(Parameters))]
		public LanguageQuestionParameters ParametersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageQuestionNames Names
		{ get { return NamesXml; } }

		[XmlIgnore]
		public ILanguageQuestionParameters Parameters
		{ get { return ParametersXml; } }

		#endregion

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
