using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageEditing : ILanguageEditing
	{
		#region Properties

		[XmlElement]
		public String PropertyConcept
		{ get; set; }

		[XmlElement]
		public String PropertySign
		{ get; set; }

		[XmlElement]
		public String PropertyValue
		{ get; set; }

		[XmlElement]
		public String PropertyWhole
		{ get; set; }

		[XmlElement]
		public String PropertyPart
		{ get; set; }

		[XmlElement]
		public String PropertyAncestor
		{ get; set; }

		[XmlElement]
		public String PropertyDescendant
		{ get; set; }

		[XmlElement]
		public String PropertyArea
		{ get; set; }

		[XmlElement]
		public String PropertyName
		{ get; set; }

		[XmlElement]
		public String PropertyHint
		{ get; set; }

		[XmlElement]
		public String PropertyLeftValue
		{ get; set; }

		[XmlElement]
		public String PropertyRightValue
		{ get; set; }

		[XmlElement]
		public String PropertyComparisonSign
		{ get; set; }

		[XmlElement]
		public String PropertyProcessA
		{ get; set; }

		[XmlElement]
		public String PropertyProcessB
		{ get; set; }

		[XmlElement]
		public String PropertySequenceSign
		{ get; set; }

		[XmlElement]
		public String PropertyAttributes
		{ get; set; }

		[XmlElement]
		public String ColumnHeaderLanguage
		{ get; set; }

		[XmlElement]
		public String ColumnHeaderValue
		{ get; set; }

		#endregion

		internal static LanguageEditing CreateDefault()
		{
			return new LanguageEditing
			{
				PropertyConcept = "Понятие",
				PropertySign = "Признак",
				PropertyValue = "Значение",
				PropertyWhole = "Целое",
				PropertyPart = "Часть",
				PropertyAncestor = "Родительское понятие",
				PropertyDescendant = "Дочернее понятие",
				PropertyArea = "Предметная область",
				PropertyName = "Название",
				PropertyHint = "Подсказка",
				PropertyLeftValue = "Значение слева",
				PropertyRightValue = "Значение справа",
				PropertyComparisonSign = "Знак сравнения",
				PropertyProcessA = "Процесс A",
				PropertyProcessB = "Процесс B",
				PropertySequenceSign = "Последовательность",
				PropertyAttributes = "Атрибуты:",
				ColumnHeaderLanguage = "Язык",
				ColumnHeaderValue = "Значение",
			};
		}
	}
}
