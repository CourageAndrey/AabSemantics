using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	[DataContract]
	public class IsValueQuestion : Serialization.Json.Question<Questions.IsValueQuestion>
	{
		#region Properties

		[DataMember]
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

		protected override Questions.IsValueQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsValueQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
