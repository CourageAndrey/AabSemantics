using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Captions of the fields shown in the knowledge editing dialogs.</summary>
	public interface ILanguageEditing
	{
		/// <summary>Caption of the "concept" field.</summary>
		String PropertyConcept
		{ get; }

		/// <summary>Caption of the "concepts" field.</summary>
		String PropertyConcepts
		{ get; }

		/// <summary>Caption of the "type" field.</summary>
		String PropertyType
		{ get; }

		/// <summary>Caption of the "sign" field.</summary>
		String PropertySign
		{ get; }

		/// <summary>Caption of the "value" field.</summary>
		String PropertyValue
		{ get; }

		/// <summary>Caption of the "whole" field.</summary>
		String PropertyWhole
		{ get; }

		/// <summary>Caption of the "part" field.</summary>
		String PropertyPart
		{ get; }

		/// <summary>Caption of the "ancestor" field.</summary>
		String PropertyAncestor
		{ get; }

		/// <summary>Caption of the "descendant" field.</summary>
		String PropertyDescendant
		{ get; }

		/// <summary>Caption of the "area" field.</summary>
		String PropertyArea
		{ get; }

		/// <summary>Caption of the "i d" field.</summary>
		String PropertyID
		{ get; }

		/// <summary>Caption of the "name" field.</summary>
		String PropertyName
		{ get; }

		/// <summary>Caption of the "hint" field.</summary>
		String PropertyHint
		{ get; }

		/// <summary>Caption of the "left value" field.</summary>
		String PropertyLeftValue
		{ get; }

		/// <summary>Caption of the "right value" field.</summary>
		String PropertyRightValue
		{ get; }

		/// <summary>Caption of the "comparison sign" field.</summary>
		String PropertyComparisonSign
		{ get; }

		/// <summary>Caption of the "process a" field.</summary>
		String PropertyProcessA
		{ get; }

		/// <summary>Caption of the "process b" field.</summary>
		String PropertyProcessB
		{ get; }

		/// <summary>Caption of the "sequence sign" field.</summary>
		String PropertySequenceSign
		{ get; }

		/// <summary>Caption of the "attributes" field.</summary>
		String PropertyAttributes
		{ get; }

		/// <summary>Caption of the "key" field.</summary>
		String PropertyKey
		{ get; }

		/// <summary>Header of the language column.</summary>
		String ColumnHeaderLanguage
		{ get; }

		/// <summary>Header of the value column.</summary>
		String ColumnHeaderValue
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageEditing"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageEditing : ILanguageEditing
	{
		#region Properties

		/// <summary>Caption of the "concept" field.</summary>
		[XmlElement]
		public String PropertyConcept
		{ get; set; }

		/// <summary>Caption of the "concepts" field.</summary>
		[XmlElement]
		public String PropertyConcepts
		{ get; set; }

		/// <summary>Caption of the "type" field.</summary>
		[XmlElement]
		public String PropertyType
		{ get; set; }

		/// <summary>Caption of the "sign" field.</summary>
		[XmlElement]
		public String PropertySign
		{ get; set; }

		/// <summary>Caption of the "value" field.</summary>
		[XmlElement]
		public String PropertyValue
		{ get; set; }

		/// <summary>Caption of the "whole" field.</summary>
		[XmlElement]
		public String PropertyWhole
		{ get; set; }

		/// <summary>Caption of the "part" field.</summary>
		[XmlElement]
		public String PropertyPart
		{ get; set; }

		/// <summary>Caption of the "ancestor" field.</summary>
		[XmlElement]
		public String PropertyAncestor
		{ get; set; }

		/// <summary>Caption of the "descendant" field.</summary>
		[XmlElement]
		public String PropertyDescendant
		{ get; set; }

		/// <summary>Caption of the "area" field.</summary>
		[XmlElement]
		public String PropertyArea
		{ get; set; }

		/// <summary>Caption of the "i d" field.</summary>
		[XmlElement]
		public String PropertyID
		{ get; set; }

		/// <summary>Caption of the "name" field.</summary>
		[XmlElement]
		public String PropertyName
		{ get; set; }

		/// <summary>Caption of the "hint" field.</summary>
		[XmlElement]
		public String PropertyHint
		{ get; set; }

		/// <summary>Caption of the "left value" field.</summary>
		[XmlElement]
		public String PropertyLeftValue
		{ get; set; }

		/// <summary>Caption of the "right value" field.</summary>
		[XmlElement]
		public String PropertyRightValue
		{ get; set; }

		/// <summary>Caption of the "comparison sign" field.</summary>
		[XmlElement]
		public String PropertyComparisonSign
		{ get; set; }

		/// <summary>Caption of the "process a" field.</summary>
		[XmlElement]
		public String PropertyProcessA
		{ get; set; }

		/// <summary>Caption of the "process b" field.</summary>
		[XmlElement]
		public String PropertyProcessB
		{ get; set; }

		/// <summary>Caption of the "sequence sign" field.</summary>
		[XmlElement]
		public String PropertySequenceSign
		{ get; set; }

		/// <summary>Caption of the "attributes" field.</summary>
		[XmlElement]
		public String PropertyAttributes
		{ get; set; }

		/// <summary>Caption of the "key" field.</summary>
		[XmlElement]
		public String PropertyKey
		{ get; set; }

		/// <summary>Header of the language column.</summary>
		[XmlElement]
		public String ColumnHeaderLanguage
		{ get; set; }

		/// <summary>Header of the value column.</summary>
		[XmlElement]
		public String ColumnHeaderValue
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageEditing CreateDefault()
		{
			return new LanguageEditing
			{
				PropertyConcept = "Concept",
				PropertyConcepts = "Concepts",
				PropertyType = "Kind",
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
				PropertyKey = "Key",
				ColumnHeaderLanguage = "Language",
				ColumnHeaderValue = "Value",
			};
		}
	}
}
