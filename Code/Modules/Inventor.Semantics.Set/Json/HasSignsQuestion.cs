using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class HasSignsQuestion : Serialization.Json.Question<Questions.HasSignsQuestion>
	{
		#region Properties

		public String Concept
		{ get; set; }

		public Boolean Recursive
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignsQuestion()
			: base()
		{ }

		public HasSignsQuestion(Questions.HasSignsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Recursive = question.Recursive;
		}

		#endregion

		protected override Questions.HasSignsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.HasSignsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				Recursive,
				preconditions);
		}
	}
}
