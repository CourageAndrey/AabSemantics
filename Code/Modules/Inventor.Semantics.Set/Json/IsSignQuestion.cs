using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class IsSignQuestion : Serialization.Json.Question<Questions.IsSignQuestion>
	{
		#region Properties

		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public IsSignQuestion()
			: base()
		{ }

		public IsSignQuestion(Questions.IsSignQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.IsSignQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsSignQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
