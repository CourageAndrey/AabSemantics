using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageConfiguration
	{
		string AutoValidate
		{ get; }
	}

	public class LanguageConfiguration : ILanguageConfiguration
	{
		#region Properties

		[XmlElement]
		public string AutoValidate
		{ get; set; }

		#endregion

		internal static LanguageConfiguration CreateDefault()
		{
			return new LanguageConfiguration
			{
				AutoValidate = "Автоматическая проверка вносимых знаний.",
			};
		}
	}
}
