using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class EnumerateContainersQuestion : Serialization.Json.Question<Questions.EnumerateContainersQuestion>
	{
		#region Properties

		public String Concept
		{ get; }

		#endregion

		#region Constructors

		public EnumerateContainersQuestion()
			: base()
		{ }

		public EnumerateContainersQuestion(Questions.EnumerateContainersQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateContainersQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateContainersQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
