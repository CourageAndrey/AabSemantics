using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Modules.Set.Json
{
	[DataContract]
	public class HasSignQuestion : Serialization.Json.Question<Questions.HasSignQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		[DataMember]
		public String Sign
		{ get; set; }

		[DataMember]
		public System.Boolean Recursive
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

		protected override Questions.HasSignQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.HasSignQuestion(
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign),
				Recursive,
				preconditions);
		}
	}
}
