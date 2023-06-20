using System.Xml.Serialization;

namespace AabSemantics.Sample07.CustomModule.Localization
{
	public interface ILanguageQuestionParameters
	{
		string ParamConcept1
		{ get; }

		string ParamConcept2
		{ get; }
	}

	[XmlType("CustomQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public string ParamConcept1
		{ get; set; }

		[XmlElement]
		public string ParamConcept2
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				ParamConcept1 = "CONCEPT1",
				ParamConcept2 = "CONCEPT2",
			};
		}
	}
}
