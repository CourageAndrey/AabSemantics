using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;

namespace AabSemantics.Modules.Classification.Json
{
	[DataContract]
	public class EnumerateAncestorsQuestion : Question<Questions.EnumerateAncestorsQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

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

		protected override Questions.EnumerateAncestorsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateAncestorsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
