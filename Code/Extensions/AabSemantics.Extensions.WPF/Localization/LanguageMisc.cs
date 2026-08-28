using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Captions that do not belong to any single dialog.</summary>
	public interface ILanguageMisc
	{
		/// <summary>Display name of the semantic network node.</summary>
		String NameSemanticNetwork
		{ get; }

		/// <summary>Display name of the concepts category node.</summary>
		String NameCategoryConcepts
		{ get; }

		/// <summary>Display name of the statements category node.</summary>
		String NameCategoryStatements
		{ get; }

		/// <summary>Caption of the rules listing.</summary>
		String Rules
		{ get; }

		/// <summary>Caption shown while the rules listing is being collected.</summary>
		String DescribingRules
		{ get; }

		/// <summary>Caption shown while the knowledge base is being validated.</summary>
		String CheckingConsistency
		{ get; }

		/// <summary>Caption of an answer.</summary>
		String Answer
		{ get; }

		/// <summary>Marker shown next to a mandatory field.</summary>
		String Required
		{ get; }

		/// <summary>Title of the open-knowledge-base dialog.</summary>
		String DialogKbOpenTitle
		{ get; }

		/// <summary>Title of the save-knowledge-base dialog.</summary>
		String DialogKbSaveTitle
		{ get; }

		/// <summary>File dialog filter for knowledge base files.</summary>
		String DialogKbFileFilter
		{ get; }

		/// <summary>Caption of a concept.</summary>
		String Concept
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageMisc"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageMisc : ILanguageMisc
	{
		#region Properties

		/// <summary>Display name of the semantic network node.</summary>
		[XmlElement]
		public String NameSemanticNetwork
		{ get; set; }

		/// <summary>Display name of the concepts category node.</summary>
		[XmlElement]
		public String NameCategoryConcepts
		{ get; set; }

		/// <summary>Display name of the statements category node.</summary>
		[XmlElement]
		public String NameCategoryStatements
		{ get; set; }

		/// <summary>Caption of the rules listing.</summary>
		[XmlElement]
		public String Rules
		{ get; set; }

		/// <summary>Caption shown while the rules listing is being collected.</summary>
		[XmlElement]
		public String DescribingRules
		{ get; set; }

		/// <summary>Caption shown while the knowledge base is being validated.</summary>
		[XmlElement]
		public String CheckingConsistency
		{ get; set; }

		/// <summary>Caption of an answer.</summary>
		[XmlElement]
		public String Answer
		{ get; set; }

		/// <summary>Marker shown next to a mandatory field.</summary>
		[XmlElement]
		public String Required
		{ get; set; }

		/// <summary>Title of the open-knowledge-base dialog.</summary>
		[XmlElement]
		public String DialogKbOpenTitle
		{ get; set; }

		/// <summary>Title of the save-knowledge-base dialog.</summary>
		[XmlElement]
		public String DialogKbSaveTitle
		{ get; set; }

		/// <summary>File dialog filter for knowledge base files.</summary>
		[XmlElement]
		public String DialogKbFileFilter
		{ get; set; }

		/// <summary>Caption of a concept.</summary>
		/// <summary>The concept in question.</summary>
		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageMisc CreateDefault()
		{
			return new LanguageMisc
			{
				NameSemanticNetwork = "Semantic network",
				NameCategoryConcepts = "Concepts",
				NameCategoryStatements = "Statements",
				Rules = "All semantic network rules:",
				DescribingRules = "Collecting the rules...",
				CheckingConsistency = "Checking the knowledge base...",
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
