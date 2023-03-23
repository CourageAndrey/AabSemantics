using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Samples.Semantics.Sample07.CustomModule.Json
{
	[Serializable]
	public class CustomQuestion : Question<Sample07.CustomModule.CustomQuestion>
	{
		#region Properties

		public String Concept1
		{ get; set; }

		public String Concept2
		{ get; set; }

		#endregion

		#region Constructors

		public CustomQuestion()
		{ }

		public CustomQuestion(Sample07.CustomModule.CustomQuestion question)
			: base(question)
		{
			Concept1 = question.Concept1.ID;
			Concept2 = question.Concept2.ID;
		}

		#endregion

		protected override Sample07.CustomModule.CustomQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Sample07.CustomModule.CustomQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
