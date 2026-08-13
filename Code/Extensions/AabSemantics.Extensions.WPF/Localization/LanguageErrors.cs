using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Wordings of the unhandled-exception dialog.</summary>
	public interface ILanguageErrors
	{
		/// <summary>Caption introducing a nested exception.</summary>
		String InnerException
		{ get; }

		/// <summary>Header of the error dialog.</summary>
		String DialogHeader
		{ get; }

		/// <summary>Message shown for an ordinary error.</summary>
		String DialogMessageCommon
		{ get; }

		/// <summary>Message shown for an error the application cannot recover from.</summary>
		String DialogMessageFatal
		{ get; }

		/// <summary>Message shown for an error reported by a nested exception.</summary>
		String DialogMessageInner
		{ get; }

		/// <summary>Message shown for an error raised while rendering the interface.</summary>
		String DialogMessageView
		{ get; }

		/// <summary>Caption of the exception type field.</summary>
		String Class
		{ get; }

		/// <summary>Caption of the exception message field.</summary>
		String Message
		{ get; }

		/// <summary>Caption of the stack trace field.</summary>
		String Stack
		{ get; }

		/// <summary>File dialog filter used when saving an error report.</summary>
		String SaveFilter
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageErrors"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageErrors : ILanguageErrors
	{
		#region Properties

		/// <summary>Caption introducing a nested exception.</summary>
		[XmlElement]
		public String InnerException
		{ get; set; }

		/// <summary>Header of the error dialog.</summary>
		[XmlElement]
		public String DialogHeader
		{ get; set; }

		/// <summary>Message shown for an ordinary error.</summary>
		[XmlElement]
		public String DialogMessageCommon
		{ get; set; }

		/// <summary>Message shown for an error the application cannot recover from.</summary>
		[XmlElement]
		public String DialogMessageFatal
		{ get; set; }

		/// <summary>Message shown for an error reported by a nested exception.</summary>
		[XmlElement]
		public String DialogMessageInner
		{ get; set; }

		/// <summary>Message shown for an error raised while rendering the interface.</summary>
		[XmlElement]
		public String DialogMessageView
		{ get; set; }

		/// <summary>Caption of the exception type field.</summary>
		/// <summary>Full name of the exception type.</summary>
		[XmlElement]
		public String Class
		{ get; set; }

		/// <summary>Caption of the exception message field.</summary>
		/// <summary>The exception message.</summary>
		[XmlElement]
		public String Message
		{ get; set; }

		/// <summary>Caption of the stack trace field.</summary>
		[XmlElement]
		public String Stack
		{ get; set; }

		/// <summary>File dialog filter used when saving an error report.</summary>
		[XmlElement]
		public String SaveFilter
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
