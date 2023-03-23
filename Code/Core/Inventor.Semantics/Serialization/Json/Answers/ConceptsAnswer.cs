using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[Serializable]
	public class ConceptsAnswer : Answer
	{
		#region Properties

		public List<Concept> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptsAnswer()
			: base()
		{ }

		public ConceptsAnswer(Semantics.Answers.ConceptsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concepts = answer.Result.Select(concept => new Concept(concept)).ToList();
		}

		#endregion
	}
}