using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	public interface ILanguageCommon
	{
		#region Buttons

		String Close
		{ get; }

		String Ok
		{ get; }

		String Cancel
		{ get; }

		String Abort
		{ get; }

		String Ignore
		{ get; }

		String Save
		{ get; }

		String SaveFile
		{ get; }

		#endregion

		String Question
		{ get; }
	}

	[XmlType]
	public class LanguageCommon : ILanguageCommon
	{
		#region Properties

		#region Buttons

		[XmlElement]
		public String Close
		{ get; set; }

		[XmlElement]
		public String Ok
		{ get; set; }

		[XmlElement]
		public String Cancel
		{ get; set; }

		[XmlElement]
		public String Abort
		{ get; set; }

		[XmlElement]
		public String Ignore
		{ get; set; }

		[XmlElement]
		public String Save
		{ get; set; }

		[XmlElement]
		public String SaveFile
		{ get; set; }

		#endregion

		[XmlElement]
		public String Question
		{ get; set; }

		#endregion

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
