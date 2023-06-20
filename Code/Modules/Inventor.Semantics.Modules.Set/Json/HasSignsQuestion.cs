using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class HasSignsQuestion : Serialization.Json.Question<Questions.HasSignsQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		[DataMember]
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

		protected override Questions.HasSignsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.HasSignsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				Recursive,
				preconditions);
		}
	}
}
