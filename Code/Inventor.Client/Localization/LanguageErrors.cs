using System;
using System.Xml.Serialization;

namespace Inventor.Client.Localization
{
	public interface ILanguageErrors
	{
		String InnerException
		{ get; }

		String DialogHeader
		{ get; }

		String DialogMessageCommon
		{ get; }

		String DialogMessageFatal
		{ get; }

		String DialogMessageInner
		{ get; }

		String DialogMessageView
		{ get; }

		String Class
		{ get; }

		String Message
		{ get; }

		String Stack
		{ get; }

		String SaveFilter
		{ get; }
	}

	[XmlType]
	public class LanguageErrors : ILanguageErrors
	{
		#region Properties

		[XmlElement]
		public String InnerException
		{ get; set; }

		[XmlElement]
		public String DialogHeader
		{ get; set; }

		[XmlElement]
		public String DialogMessageCommon
		{ get; set; }

		[XmlElement]
		public String DialogMessageFatal
		{ get; set; }

		[XmlElement]
		public String DialogMessageInner
		{ get; set; }

		[XmlElement]
		public String DialogMessageView
		{ get; set; }

		[XmlElement]
		public String Class
		{ get; set; }

		[XmlElement]
		public String Message
		{ get; set; }

		[XmlElement]
		public String Stack
		{ get; set; }

		[XmlElement]
		public String SaveFilter
		{ get; set; }

		#endregion

		internal static LanguageErrors CreateDefault()
		{
			return new LanguageErrors
			{
				InnerException = "Inner Exception",
				DialogHeader = "An error occured",
				DialogMessageCommon = "Please, contact program developer and forward exception details file (press \"Save\" to create it).",
				DialogMessageFatal = "Critcial error occured. Application will be forcibly terminated. Please, contact program developer and forward exception details file (press \"Save\" to create it).",
				DialogMessageInner = "Inner error details",
				DialogMessageView = "Error details",
				Class = "Class:",
				Message = "Message:",
				Stack = "Stack trace:",
				SaveFilter = "XML-file|*.xml",
			};
		}
	}
}
