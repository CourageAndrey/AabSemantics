using System;
using System.Xml.Serialization;

namespace Inventor.Client.Localization
{
	[Serializable]
	public class LanguageErrors : ILanguageErrors
	{
		#region Properties

		[XmlElement]
		public String InnerException
		{ get; set; }

		[XmlElement]
		public String DialogHeader
		{ get; set; }

		[XmlElement]
		public String DialogMessageCommon
		{ get; set; }

		[XmlElement]
		public String DialogMessageFatal
		{ get; set; }

		[XmlElement]
		public String DialogMessageInner
		{ get; set; }

		[XmlElement]
		public String DialogMessageView
		{ get; set; }

		[XmlElement]
		public String Class
		{ get; set; }

		[XmlElement]
		public String Message
		{ get; set; }

		[XmlElement]
		public String Stack
		{ get; set; }

		[XmlElement]
		public String SaveFilter
		{ get; set; }

		#endregion

		internal static LanguageErrors CreateDefault()
		{
			return new LanguageErrors
			{
				InnerException = "Вложенное исключение",
				DialogHeader = "Во время выполнения программы произошла ошибка",
				DialogMessageCommon = "Пожалуйста, свяжитесь с разработчиком и передайте ему файл с описанием ошибки (формируется при нажатии на кнопке \"Сохранить\").",
				DialogMessageFatal = "Критическая ошибка не была обработана. Приложение вероятнее всего будет принудительно закрыто. Пожалуйста, свяжитесь с разработчиком и передайте ему файл с описанием ошибки (формируется при нажатии на кнопке \"Сохранить\").",
				DialogMessageInner = "Описание вложенного исключения",
				DialogMessageView = "Просмотр свойств ошибки",
				Class = "Класс:",
				Message = "Сообщение:",
				Stack = "Стек вызовов:",
				SaveFilter = "XML-файл|*.xml",
			};
		}
	}
}
