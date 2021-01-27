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
		public String PropertyParent
		{ get; set; }

		[XmlElement]
		public String PropertyChild
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
				PropertyParent = "Родительское понятие",
				PropertyChild = "Дочернее понятие",
				PropertyArea = "Предметная область",
				PropertyName = "Название",
				PropertyHint = "Подсказка",
				ColumnHeaderLanguage = "Язык",
				ColumnHeaderValue = "Значение",
			};
		}
	}
}
