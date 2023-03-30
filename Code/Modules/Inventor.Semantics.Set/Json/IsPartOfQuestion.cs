using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class IsPartOfQuestion : Serialization.Json.Question<Questions.IsPartOfQuestion>
	{
		#region Properties

		[DataMember]
		public String Parent
		{ get; set; }

		[DataMember]
		public String Child
		{ get; set; }

		#endregion

		#region Constructors

		public IsPartOfQuestion()
			: base()
		{ }

		public IsPartOfQuestion(Questions.IsPartOfQuestion question)
			: base(question)
		{
			Parent = question.Parent.ID;
			Child = question.Child.ID;
		}

		#endregion

		protected override Questions.IsPartOfQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsPartOfQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
