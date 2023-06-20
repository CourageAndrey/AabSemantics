using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Modules.Mathematics.Json
{
	[DataContract]
	public class ComparisonQuestion : Serialization.Json.Question<Questions.ComparisonQuestion>
	{
		#region Properties

		[DataMember]
		public String LeftValue
		{ get; set; }

		[DataMember]
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

		protected override Questions.ComparisonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.ComparisonQuestion(
				conceptIdResolver.GetConceptById(LeftValue),
				conceptIdResolver.GetConceptById(RightValue),
				preconditions);
		}
	}
}
