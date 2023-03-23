using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Classification.Json
{
	[Serializable]
	public class EnumerateDescendantsQuestion : Question<Questions.EnumerateDescendantsQuestion>
	{
		#region Properties

		public String Concept
		{ get; }

		#endregion

		#region Constructors

		public EnumerateDescendantsQuestion()
			: base()
		{ }

		public EnumerateDescendantsQuestion(Questions.EnumerateDescendantsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateDescendantsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateDescendantsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
