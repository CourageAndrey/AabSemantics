﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class EnumerateContainersQuestion : Serialization.Json.Question<Questions.EnumerateContainersQuestion>
	{
		#region Properties

		[DataMember]
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
