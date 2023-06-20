using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Modules.WPF.Localization
{
	public interface ILanguageEditing
	{
		String PropertyConcept
		{ get; }

		String PropertySign
		{ get; }

		String PropertyValue
		{ get; }

		String PropertyWhole
		{ get; }

		String PropertyPart
		{ get; }

		String PropertyAncestor
		{ get; }

		String PropertyDescendant
		{ get; }

		String PropertyArea
		{ get; }

		String PropertyID
		{ get; }

		String PropertyName
		{ get; }

		String PropertyHint
		{ get; }

		String PropertyLeftValue
		{ get; }

		String PropertyRightValue
		{ get; }

		String PropertyComparisonSign
		{ get; }

		String PropertyProcessA
		{ get; }

		String PropertyProcessB
		{ get; }

		String PropertySequenceSign
		{ get; }

		String PropertyAttributes
		{ get; }

		String ColumnHeaderLanguage
		{ get; }

		String ColumnHeaderValue
		{ get; }
	}

	[XmlType]
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
		public String PropertyID
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
				PropertyConcept = "Concept",
				PropertySign = "Sign",
				PropertyValue = "Value",
				PropertyWhole = "Whole",
				PropertyPart = "Part",
				PropertyAncestor = "Ancestor",
				PropertyDescendant = "Descendant",
				PropertyArea = "Subject Area",
				PropertyID = "ID",
				PropertyName = "Name",
				PropertyHint = "Hint",
				PropertyLeftValue = "Left Value",
				PropertyRightValue = "Right Value",
				PropertyComparisonSign = "ComparisonSign",
				PropertyProcessA = "Process A",
				PropertyProcessB = ".Process B",
				PropertySequenceSign = "Sequence Sign",
				PropertyAttributes = "Attributes:",
				ColumnHeaderLanguage = "Language",
				ColumnHeaderValue = "Value",
			};
		}
	}
}
