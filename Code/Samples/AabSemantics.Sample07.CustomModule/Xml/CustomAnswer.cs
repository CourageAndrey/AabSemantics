using System.Xml.Serialization;

namespace AabSemantics.Sample07.CustomModule.Xml
{
	[XmlType("IsCustom")]
	public class CustomAnswer : AabSemantics.Serialization.Xml.Answer
	{
		#region Constructors

		public CustomAnswer()
		{ }

		public CustomAnswer(Sample07.CustomModule.CustomAnswer answer, ILanguage language)
			: base(answer, language)
		{ }

		#endregion
	}
}
