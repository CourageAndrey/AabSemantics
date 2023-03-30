using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Classification.Json
{
	[DataContract]
	public class IsQuestion : Question<Questions.IsQuestion>
	{
		#region Properties

		[DataMember]
		public String Child
		{ get; }

		[DataMember]
		public String Parent
		{ get; }

		#endregion

		#region Constructors

		public IsQuestion()
			: base()
		{ }

		public IsQuestion(Questions.IsQuestion question)
			: base(question)
		{
			Parent = question.Parent.ID;
			Child = question.Child.ID;
		}

		#endregion

		protected override Questions.IsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
