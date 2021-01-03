using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageErrors
	{
		String Warning
		{ get; }

		String InnerException
		{ get; }

		String DialogHeader
		{ get; }

		String DialogMessageCommon
		{ get; }

		String DialogMessageFatal
		{ get; }

		String DialogMessageInner
		{ get; }

		String DialogMessageView
		{ get; }

		String Class
		{ get; }

		String Message
		{ get; }

		String Stack
		{ get; }

		String SaveFilter
		{ get; }

		String LocalizationError
		{ get; }

		String TypeIsntEnumerable
		{ get; }
	}

	[Serializable]
	public sealed class LanguageErrors : ILanguageErrors
	{
		#region Properties

		[XmlElement]
		public String Warning
		{ get; set; }

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

		[XmlElement]
		public String LocalizationError
		{ get; set; }

		[XmlElement]
		public String TypeIsntEnumerable
		{ get; set; }

		#endregion

		internal static LanguageErrors CreateDefault()
		{
			return new LanguageErrors
			{
				Warning = "Внимание",
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
				LocalizationError = "[ Ошибка локализации ]",
				TypeIsntEnumerable = "Тип {0} не является перечислимым (enum)!",
			};
		}
	}
}
