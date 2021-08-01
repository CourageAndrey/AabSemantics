using System;
using System.Xml.Serialization;

namespace Inventor.Client.Localization
{
	public class LanguageMisc : ILanguageMisc
	{
		#region Properties

		[XmlElement]
		public String NameSemanticNetwork
		{ get; set; }

		[XmlElement]
		public String NameCategoryConcepts
		{ get; set; }

		[XmlElement]
		public String NameCategoryStatements
		{ get; set; }

		[XmlElement]
		public String StrictEnumeration
		{ get; set; }

		[XmlElement]
		public String ClasificationSign
		{ get; set; }

		[XmlElement]
		public String Rules
		{ get; set; }

		[XmlElement]
		public String Answer
		{ get; set; }

		[XmlElement]
		public String Required
		{ get; set; }

		[XmlElement]
		public String DialogKbOpenTitle
		{ get; set; }

		[XmlElement]
		public String DialogKbSaveTitle
		{ get; set; }

		[XmlElement]
		public String DialogKbFileFilter
		{ get; set; }

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		internal static LanguageMisc CreateDefault()
		{
			return new LanguageMisc
			{
				NameSemanticNetwork = "База знаний",
				NameCategoryConcepts = "Понятия",
				NameCategoryStatements = "Утверждения",
				StrictEnumeration = " и не может принимать другое значение",
				ClasificationSign = " по признаку \"{0}\"",
				Rules = "Все правила базы знаний:",
				Answer = "Ответ:",
				Required = "обязательный",
				DialogKbOpenTitle = "Загрузка базы знаний",
				DialogKbSaveTitle = "Сохранение базы знаний",
				DialogKbFileFilter = "XML с базой знаний|*.xml",
				Concept = "Понятие",
			};
		}
	}
}
