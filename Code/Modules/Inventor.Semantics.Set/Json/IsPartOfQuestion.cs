using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class IsPartOfQuestion : Serialization.Json.Question<Questions.IsPartOfQuestion>
	{
		#region Properties

		public String Parent
		{ get; set; }

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

		protected override Questions.IsPartOfQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsPartOfQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
