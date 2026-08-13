using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Captions of the buttons shared by every dialog.</summary>
	public interface ILanguageCommon
	{
		#region Buttons

		/// <summary>Caption of the close button.</summary>
		String Close
		{ get; }

		/// <summary>Caption of the confirm button.</summary>
		String Ok
		{ get; }

		/// <summary>Caption of the cancel button.</summary>
		String Cancel
		{ get; }

		/// <summary>Caption of the abort button.</summary>
		String Abort
		{ get; }

		/// <summary>Caption of the ignore button.</summary>
		String Ignore
		{ get; }

		/// <summary>Caption of the save button.</summary>
		String Save
		{ get; }

		/// <summary>Caption of the save-to-file button.</summary>
		String SaveFile
		{ get; }

		#endregion

		/// <summary>Caption of a question prompt.</summary>
		String Question
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageCommon"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageCommon : ILanguageCommon
	{
		#region Properties

		#region Buttons

		/// <summary>Caption of the close button.</summary>
		[XmlElement]
		public String Close
		{ get; set; }

		/// <summary>Caption of the confirm button.</summary>
		[XmlElement]
		public String Ok
		{ get; set; }

		/// <summary>Caption of the cancel button.</summary>
		[XmlElement]
		public String Cancel
		{ get; set; }

		/// <summary>Caption of the abort button.</summary>
		[XmlElement]
		public String Abort
		{ get; set; }

		/// <summary>Caption of the ignore button.</summary>
		[XmlElement]
		public String Ignore
		{ get; set; }

		/// <summary>Caption of the save button.</summary>
		[XmlElement]
		public String Save
		{ get; set; }

		/// <summary>Caption of the save-to-file button.</summary>
		[XmlElement]
		public String SaveFile
		{ get; set; }

		#endregion

		/// <summary>Caption of a question prompt.</summary>
		[XmlElement]
		public String Question
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageCommon CreateDefault()
		{
			return new LanguageCommon
			{
				Close = "Close",
				Ok = "OK",
				Cancel = "Cancel",
				Abort = "Abort",
				Ignore = "Ignore",
				Save = "Save",
				SaveFile = "Please, chose save file...",

				Question = "Question",
			};
		}
	}
}
