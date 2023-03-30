using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class WhatQuestion : Serialization.Json.Question<Questions.WhatQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public WhatQuestion()
		{ }

		public WhatQuestion(Questions.WhatQuestion question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.WhatQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.WhatQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
}
}
