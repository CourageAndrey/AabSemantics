using System.Xml.Serialization;

namespace Samples._07.CustomModule.Localization
{
	public interface ILanguageQuestionNames
	{
		string Custom
		{ get; }
	}

	[XmlType]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{
		#region Properties

		[XmlElement]
		public string Custom
		{ get; set; }

		#endregion

		public static LanguageQuestionNames CreateDefault()
		{
			return new LanguageQuestionNames
			{
				Custom = "Custom",
			};
		}
	}
}
