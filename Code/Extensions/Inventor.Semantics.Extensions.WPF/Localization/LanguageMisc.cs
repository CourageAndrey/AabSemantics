using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Extensions.WPF.Localization
{
	public interface ILanguageMisc
	{
		String NameSemanticNetwork
		{ get; }

		String NameCategoryConcepts
		{ get; }

		String NameCategoryStatements
		{ get; }

		String Rules
		{ get; }

		String Answer
		{ get; }

		String Required
		{ get; }

		String DialogKbOpenTitle
		{ get; }

		String DialogKbSaveTitle
		{ get; }

		String DialogKbFileFilter
		{ get; }

		String Concept
		{ get; }
	}

	[XmlType]
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
				NameSemanticNetwork = "Semantic network",
				NameCategoryConcepts = "Concepts",
				NameCategoryStatements = "Statements",
				Rules = "All semantic network rules:",
				Answer = "Answer:",
				Required = "required",
				DialogKbOpenTitle = "Open semantic network",
				DialogKbSaveTitle = "Save semantic network",
				DialogKbFileFilter = "Semantic network XML|*.xml",
				Concept = "Concept",
			};
		}
	}
}
