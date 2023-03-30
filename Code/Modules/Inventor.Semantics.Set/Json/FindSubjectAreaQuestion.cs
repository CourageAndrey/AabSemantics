using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class FindSubjectAreaQuestion : Serialization.Json.Question<Questions.FindSubjectAreaQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; }

		#endregion

		#region Constructors

		public FindSubjectAreaQuestion()
			: base()
		{ }

		public FindSubjectAreaQuestion(Questions.FindSubjectAreaQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.FindSubjectAreaQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.FindSubjectAreaQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
