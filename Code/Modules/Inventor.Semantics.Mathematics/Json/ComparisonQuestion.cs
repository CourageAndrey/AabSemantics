using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Mathematics.Json
{
	[Serializable]
	public class ComparisonQuestion : Serialization.Json.Question<Questions.ComparisonQuestion>
	{
		#region Properties

		public String LeftValue
		{ get; set; }

		public String RightValue
		{ get; set; }

		#endregion

		#region Constructors

		public ComparisonQuestion()
			: base()
		{ }

		public ComparisonQuestion(Questions.ComparisonQuestion question)
			: base(question)
		{
			LeftValue = question.LeftValue.ID;
			RightValue = question.RightValue.ID;
		}

		#endregion

		protected override Questions.ComparisonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.ComparisonQuestion(
				conceptIdResolver.GetConceptById(LeftValue),
				conceptIdResolver.GetConceptById(RightValue),
				preconditions);
		}
	}
}
