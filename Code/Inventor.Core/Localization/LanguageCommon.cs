using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	[Serializable]
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
