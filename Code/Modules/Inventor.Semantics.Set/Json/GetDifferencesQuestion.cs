using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	public class GetDifferencesQuestion : Serialization.Json.Question<Questions.GetDifferencesQuestion>
	{
		#region Properties

		public String Concept1
		{ get; }

		public String Concept2
		{ get; }

		#endregion

		#region Constructors

		public GetDifferencesQuestion()
			: base()
		{ }

		public GetDifferencesQuestion(Questions.GetDifferencesQuestion question)
			: base(question)
		{
			Concept1 = question.Concept1.ID;
			Concept2 = question.Concept2.ID;
		}

		#endregion

		protected override Questions.GetDifferencesQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetDifferencesQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
