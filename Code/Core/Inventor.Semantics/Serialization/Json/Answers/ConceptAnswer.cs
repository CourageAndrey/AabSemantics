using System;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[Serializable]
	public class ConceptAnswer : Answer
	{
		#region Properties

		public Concept Concept
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptAnswer()
			: base()
		{ }

		public ConceptAnswer(Semantics.Answers.ConceptAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concept = new Concept(answer.Result);
		}

		#endregion
	}
}