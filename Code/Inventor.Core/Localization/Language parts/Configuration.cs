using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageConfiguration
	{
		string IncludeExplaining
		{ get; }

		string AutoValidate
		{ get; }
	}

	public class LanguageConfiguration : ILanguageConfiguration
	{
		#region Properties

		[XmlElement]
		public string IncludeExplaining
		{ get; set; }

		[XmlElement]
		public string AutoValidate
		{ get; set; }

		#endregion

		internal static LanguageConfiguration CreateDefault()
		{
			return new LanguageConfiguration
			{
				IncludeExplaining = "Включать объяснение в ответ.",
				AutoValidate = "Автоматическая проверка вносимых знаний.",
			};
		}
	}
}
