using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF.Localization
{
	public interface ILanguageQuestionDialog
	{
		String Title
		{ get; }

		String SelectQuestion
		{ get; }
	}

	[XmlType]
	public class LanguageQuestionDialog : ILanguageQuestionDialog
	{
		#region Properties

		[XmlElement]
		public String Title
		{ get; set; }

		[XmlElement]
		public String SelectQuestion
		{ get; set; }

		#endregion

		internal static LanguageQuestionDialog CreateDefault()
		{
			return new LanguageQuestionDialog
			{
				Title = "New question",
				SelectQuestion = "Chose question: ",
			};
		}
	}
}