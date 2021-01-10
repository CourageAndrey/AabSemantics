using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageConfiguration : ILanguageConfiguration
	{
		#region Properties

		[XmlElement]
		public String AutoValidate
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
