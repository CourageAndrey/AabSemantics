using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Captions of the main window: its title, commands and context menu.</summary>
	public interface ILanguageMainForm
	{
		/// <summary>Window title.</summary>
		String Title
		{ get; }

		/// <summary>Caption of the command creating a new knowledge base.</summary>
		String CreateNew
		{ get; }

		/// <summary>Caption of the command opening a knowledge base.</summary>
		String Load
		{ get; }

		/// <summary>Caption of the save button.</summary>
		String Save
		{ get; }

		/// <summary>Caption of the command saving under a new name.</summary>
		String SaveAs
		{ get; }

		/// <summary>Caption of the command filling the network with sample knowledge.</summary>
		String CreateTest
		{ get; }

		/// <summary>Caption of the command listing everything the network knows.</summary>
		String DescribeKnowledge
		{ get; }

		/// <summary>Caption of the command validating the network.</summary>
		String CheckKnowledge
		{ get; }

		/// <summary>Caption of the command opening the question dialog.</summary>
		String AskQuestion
		{ get; }

		/// <summary>Caption of the language selection command.</summary>
		String SelectLanguage
		{ get; }

		/// <summary>Context menu item renaming the selected item.</summary>
		String ContextMenuRename
		{ get; }

		/// <summary>Context menu item adding a knowledge item.</summary>
		String ContextMenuKnowledgeAdd
		{ get; }

		/// <summary>Context menu item editing the selected knowledge item.</summary>
		String ContextMenuKnowledgeEdit
		{ get; }

		/// <summary>Context menu item deleting the selected knowledge item.</summary>
		String ContextMenuKnowledgeDelete
		{ get; }

		/// <summary>Prompt asking whether to save unsaved changes.</summary>
		String SavePromt
		{ get; }

		/// <summary>Title of the unsaved-changes prompt.</summary>
		String SaveTitle
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageMainForm"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageMainForm : ILanguageMainForm
	{
		#region Properties

		/// <summary>Window title.</summary>
		[XmlElement]
		public String Title
		{ get; set; }

		/// <summary>Caption of the command creating a new knowledge base.</summary>
		[XmlElement]
		public String CreateNew
		{ get; set; }

		/// <summary>Caption of the command opening a knowledge base.</summary>
		[XmlElement]
		public String Load
		{ get; set; }

		/// <summary>Caption of the save button.</summary>
		[XmlElement]
		public String Save
		{ get; set; }

		/// <summary>Caption of the command saving under a new name.</summary>
		[XmlElement]
		public String SaveAs
		{ get; set; }

		/// <summary>Caption of the command filling the network with sample knowledge.</summary>
		[XmlElement]
		public String CreateTest
		{ get; set; }

		/// <summary>Caption of the command listing everything the network knows.</summary>
		[XmlElement]
		public String DescribeKnowledge
		{ get; set; }

		/// <summary>Caption of the command validating the network.</summary>
		[XmlElement]
		public String CheckKnowledge
		{ get; set; }

		/// <summary>Caption of the command opening the question dialog.</summary>
		[XmlElement]
		public String AskQuestion
		{ get; set; }

		/// <summary>Caption of the language selection command.</summary>
		[XmlElement]
		public String SelectLanguage
		{ get; set; }

		/// <summary>Context menu item renaming the selected item.</summary>
		[XmlElement]
		public String ContextMenuRename
		{ get; set; }

		/// <summary>Context menu item adding a knowledge item.</summary>
		[XmlElement]
		public String ContextMenuKnowledgeAdd
		{ get; set; }

		/// <summary>Context menu item editing the selected knowledge item.</summary>
		[XmlElement]
		public String ContextMenuKnowledgeEdit
		{ get; set; }

		/// <summary>Context menu item deleting the selected knowledge item.</summary>
		[XmlElement]
		public String ContextMenuKnowledgeDelete
		{ get; set; }

		/// <summary>Prompt asking whether to save unsaved changes.</summary>
		[XmlElement]
		public String SavePromt
		{ get; set; }

		/// <summary>Title of the unsaved-changes prompt.</summary>
		[XmlElement]
		public String SaveTitle
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageMainForm CreateDefault()
		{
			return new LanguageMainForm
			{
				Title = "Auxiliary tool \"Inventor\"",
				CreateNew = "Create new semantic network",
				Load = "Open...",
				Save = "Save",
				SaveAs = "Save As...",
				CreateTest = "Create test semantic network",
				DescribeKnowledge = "Describe all knowledge...",
				CheckKnowledge = "Check consistency of knowledge...",
				AskQuestion = "Ask question...",
				SelectLanguage = "Language:",
				ContextMenuRename = "Rename...",
				ContextMenuKnowledgeAdd = "Add...",
				ContextMenuKnowledgeEdit = "Edit...",
				ContextMenuKnowledgeDelete = "Delete",
				SavePromt = "File has been modified. Save changes?",
				SaveTitle = "Saving changes",
			};
		}
	}
}