﻿using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class SignValueQuestion : Serialization.Json.Question<Questions.SignValueQuestion>
	{
		#region Properties

		public String Concept
		{ get; set; }

		public String Sign
		{ get; set; }

		#endregion

		#region Constructors

		public SignValueQuestion()
			: base()
		{ }

		public SignValueQuestion(Questions.SignValueQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Sign = question.Sign.ID;
		}

		#endregion

		protected override Questions.SignValueQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.SignValueQuestion(
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign),
				preconditions);
		}
	}
}
