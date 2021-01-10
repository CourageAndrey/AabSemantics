using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguageStatements
	{
		String SubjectArea
		{ get; }

		String Clasification
		{ get; }

		String HasSign
		{ get; }

		String SignValue
		{ get; }

		String Composition
		{ get; }
	}

	public class LanguageStatements : ILanguageStatements
	{
		#region Properties

		[XmlElement]
		public String SubjectArea
		{ get; set; }

		[XmlElement]
		public String Clasification
		{ get; set; }

		[XmlElement]
		public String HasSign
		{ get; set; }

		[XmlElement]
		public String SignValue
		{ get; set; }

		[XmlElement]
		public String Composition
		{ get; set; }

		#endregion

		public static LanguageStatements CreateDefaultNames()
		{
			return new LanguageStatements
			{
				SubjectArea = "Предметная область",
				Clasification = "Классификация",
				HasSign = "Признак",
				SignValue = "Значение признака",
				Composition = "Часть-целое",
			};
		}

		internal static LanguageStatements CreateDefaultHints()
		{
			return new LanguageStatements
			{
				SubjectArea = "Отношение описывает вхождение терминов в некоторую предметную область.",
				Clasification = "Отношение описывает связь между родительским и дочерним (подчинённым) понятиями.",
				HasSign = "Отношение описывает наличие некоторого признака, описывающего свойства понятия.",
				SignValue = "Отношение описывает значение признака, свойственного данному понятию.",
				Composition = "Отношение описывает вхождение одного понятия в другое качестве элемента структуры последнего.",
			};
		}
	}
}
