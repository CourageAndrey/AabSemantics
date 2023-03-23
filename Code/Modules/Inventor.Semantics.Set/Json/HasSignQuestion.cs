using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class HasSignQuestion : Serialization.Json.Question<Questions.HasSignQuestion>
	{
		#region Properties

		public String Concept
		{ get; set; }

		public String Sign
		{ get; set; }

		public Boolean Recursive
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignQuestion()
			: base()
		{ }

		public HasSignQuestion(Questions.HasSignQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Sign = question.Sign.ID;
			Recursive = question.Recursive;
		}

		#endregion

		protected override Questions.HasSignQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.HasSignQuestion(
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign),
				Recursive,
				preconditions);
		}
	}
}
