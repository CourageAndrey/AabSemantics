using System.Xml.Serialization;

using Inventor.Semantics;

namespace Samples.Semantics.Sample07.CustomModule.Xml
{
	[XmlType("IsCustom")]
	public class CustomAnswer : Inventor.Semantics.Serialization.Xml.Answer
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
