using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Classification.Json
{
	[Serializable]
	public class IsQuestion : Question<Questions.IsQuestion>
	{
		#region Properties

		public String Child
		{ get; }

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

		protected override Questions.IsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
