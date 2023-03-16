using System.Xml.Serialization;

namespace Inventor.Semantics.Xml.Answers
{
	[XmlType]
	public class ConceptAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public Concept Concept
		{ get; }

		#endregion

		public ConceptAnswer(Semantics.Answers.ConceptAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concept = new Concept(answer.Result);
		}
	}
}