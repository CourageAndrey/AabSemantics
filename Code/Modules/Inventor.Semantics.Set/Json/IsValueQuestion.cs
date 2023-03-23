using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class IsValueQuestion : Serialization.Json.Question<Questions.IsValueQuestion>
	{
		#region Properties

		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public IsValueQuestion()
			: base()
		{ }

		public IsValueQuestion(Questions.IsValueQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.IsValueQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsValueQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
