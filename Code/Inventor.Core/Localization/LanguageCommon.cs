using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageCommon
	{
		#region Data size

		String DataSizeB
		{ get; }

		String DataSizeKB
		{ get; }

		String DataSizeMB
		{ get; }

		String DataSizeGB
		{ get; }

		String DataSizeTB
		{ get; }

		#endregion

		#region Buttons

		String Close
		{ get; }

		String Ok
		{ get; }

		String Cancel
		{ get; }

		String Abort
		{ get; }

		String Ignore
		{ get; }

		String Save
		{ get; }

		String SaveFile
		{ get; }

		#endregion

		#region MainMenu

		String Exit
		{ get; }

		String SelectedLanguage
		{ get; }

		String Help
		{ get; }

		String About
		{ get; }

		String Configuration
		{ get; }

		#endregion

		String WaitPromt
		{ get; }

		String Question
		{ get; }

		String ViewPicture
		{ get; }
	}

	[Serializable]
	public sealed class LanguageCommon : ILanguageCommon
	{
		#region Properties

		#region Data size

		[XmlElement]
		public String DataSizeB
		{ get; set; }

		[XmlElement]
		public String DataSizeKB
		{ get; set; }

		[XmlElement]
		public String DataSizeMB
		{ get; set; }

		[XmlElement]
		public String DataSizeGB
		{ get; set; }

		[XmlElement]
		public String DataSizeTB
		{ get; set; }

		#endregion

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
				DataSizeB = "б",
				DataSizeKB = "Кб",
				DataSizeMB = "Мб",
				DataSizeGB = "Гб",
				DataSizeTB = "Тб",

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
