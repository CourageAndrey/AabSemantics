using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	[Serializable]
	public class LanguageCommon : ILanguageCommon
	{
		#region Properties

		#region Buttons

		[XmlElement]
		public String Close
		{ get; set; }

		[XmlElement]
		public String Ok
		{ get; set; }

		[XmlElement]
		public String Cancel
		{ get; set; }

		[XmlElement]
		public String Abort
		{ get; set; }

		[XmlElement]
		public String Ignore
		{ get; set; }

		[XmlElement]
		public String Save
		{ get; set; }

		[XmlElement]
		public String SaveFile
		{ get; set; }

		#endregion

		#region MainMenu

		[XmlElement]
		public String Exit
		{ get; set; }

		[XmlElement]
		public String SelectedLanguage
		{ get; set; }

		[XmlElement]
		public String Help
		{ get; set; }

		[XmlElement]
		public String About
		{ get; set; }

		[XmlElement]
		public String Configuration
		{ get; set; }

		#endregion

		[XmlElement]
		public String WaitPromt
		{ get; set; }

		[XmlElement]
		public String Question
		{ get; set; }

		[XmlElement]
		public String ViewPicture
		{ get; set; }

		#endregion

		internal static LanguageCommon CreateDefault()
		{
			return new LanguageCommon
			{
				Close = "Закрыть",
				Ok = "ОК",
				Cancel = "Отмена",
				Abort = "Прервать выполнение",
				Ignore = "Пропустить",
				Save = "Сохранить",
				SaveFile = "Выберите файл для сохранения...",

				Exit = "Выход",
				SelectedLanguage = "Выбранный язык",
				Help = "Справка",
				About = "О программе",
				Configuration = "Настройки",

				WaitPromt = "Пожалуйста, подождите",
				Question = "Вопрос",
				ViewPicture = "Просмотреть изображение...",
			};
		}
	}
}
