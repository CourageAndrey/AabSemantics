using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Captions of the question dialog.</summary>
	public interface ILanguageQuestionDialog
	{
		/// <summary>Window title.</summary>
		String Title
		{ get; }

		/// <summary>Caption prompting the user to pick a question.</summary>
		String SelectQuestion
		{ get; }

		/// <summary>Caption shown while the answer is being inferred.</summary>
		String Processing
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionDialog"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageQuestionDialog : ILanguageQuestionDialog
	{
		#region Properties

		/// <summary>Window title.</summary>
		[XmlElement]
		public String Title
		{ get; set; }

		/// <summary>Caption prompting the user to pick a question.</summary>
		[XmlElement]
		public String SelectQuestion
		{ get; set; }

		/// <summary>Caption shown while the answer is being inferred.</summary>
		[XmlElement]
		public String Processing
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestionDialog CreateDefault()
		{
			return new LanguageQuestionDialog
			{
				Title = "New question",
				SelectQuestion = "Chose question: ",
				Processing = "Looking for the answer...",
			};
		}
	}
}