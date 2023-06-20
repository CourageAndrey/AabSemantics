using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Modules.Set.Json
{
	[DataContract]
	public class IsSignQuestion : Serialization.Json.Question<Questions.IsSignQuestion>
	{
		#region Properties

		[DataMember]
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

		protected override Questions.IsSignQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsSignQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
