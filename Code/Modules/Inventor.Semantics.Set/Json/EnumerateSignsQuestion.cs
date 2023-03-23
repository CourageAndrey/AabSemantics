using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class EnumerateSignsQuestion : Serialization.Json.Question<Questions.EnumerateSignsQuestion>
	{
		#region Properties

		public String Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		#region Constructors

		public EnumerateSignsQuestion()
			: base()
		{ }

		public EnumerateSignsQuestion(Questions.EnumerateSignsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Recursive = question.Recursive;
		}

		#endregion

		protected override Questions.EnumerateSignsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateSignsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				Recursive,
				preconditions);
		}
	}
}
