using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		[XmlElement]
		public String True
		{ get; set; }

		[XmlElement]
		public String False
		{ get; set; }

		#endregion

		internal static LanguageSystemConcepts CreateDefaultNames()
		{
			return new LanguageSystemConcepts
			{
				True = "Да",
				False = "Нет",
			};
		}

		internal static LanguageSystemConcepts CreateDefaultHints()
		{
			return new LanguageSystemConcepts
			{
				True = "Логическое значение: истина.",
				False = "Логическое значение: ложь.",
			};
		}
	}
}
