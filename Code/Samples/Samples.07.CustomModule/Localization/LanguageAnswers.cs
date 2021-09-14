using System.Xml.Serialization;

namespace Samples._07.CustomModule.Localization
{
	public interface ILanguageAnswers
	{
		string CustomTrue
		{ get; }

		string CustomFalse
		{ get; }
	}

	[XmlType("CustomAnswers")]
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		[XmlElement]
		public string CustomTrue
		{ get; set; }

		[XmlElement]
		public string CustomFalse
		{ get; set; }

		#endregion

		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				CustomTrue = $"Yes, {CustomStatement.ParamConcept1} has got custom relationship with {CustomStatement.ParamConcept2}.",
				CustomFalse = $"No, {CustomStatement.ParamConcept1} has not got custom relationship with {CustomStatement.ParamConcept2}.",
			};
		}
	}
}
