using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageQuestionDialog
	{
		String Title
		{ get; }

		String SelectQuestion
		{ get; }
	}

	public class LanguageQuestionDialog : ILanguageQuestionDialog
	{
		#region Properties

		[XmlElement]
		public String Title
		{ get; set; }

		[XmlElement]
		public String SelectQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionDialog CreateDefault()
		{
			return new LanguageQuestionDialog
			{
				Title = "Формирование вопроса",
				SelectQuestion = "Выберите вопрос: ",
			};
		}
	}
}