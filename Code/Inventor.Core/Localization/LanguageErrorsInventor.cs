using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageErrorsInventor : ILanguageErrorsInventor
	{
		#region Properties

		[XmlElement]
		public String UnknownQuestion
		{ get; set; }

		#endregion

		internal static LanguageErrorsInventor CreateDefault()
		{
			return new LanguageErrorsInventor
			{
				UnknownQuestion = "Неизвестный тип вопроса.",
			};
		}
	}
}
