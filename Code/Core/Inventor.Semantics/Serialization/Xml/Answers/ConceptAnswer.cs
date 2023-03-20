using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class ConceptAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public Concept Concept
		{ get; }

		#endregion

		#region Constructors

		public ConceptAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public ConceptAnswer(Semantics.Answers.ConceptAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concept = new Concept(answer.Result);
		}

		#endregion
	}
}