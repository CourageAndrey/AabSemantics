﻿using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Classification.Json
{
	[Serializable]
	public class EnumerateAncestorsQuestion : Question<Questions.EnumerateAncestorsQuestion>
	{
		#region Properties

		public String Concept
		{ get; }

		#endregion

		#region Constructors

		public EnumerateAncestorsQuestion()
			: base()
		{ }

		public EnumerateAncestorsQuestion(Questions.EnumerateAncestorsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateAncestorsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateAncestorsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
