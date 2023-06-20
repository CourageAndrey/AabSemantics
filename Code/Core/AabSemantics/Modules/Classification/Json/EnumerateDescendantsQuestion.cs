using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;

namespace AabSemantics.Modules.Classification.Json
{
	[DataContract]
	public class EnumerateDescendantsQuestion : Question<Questions.EnumerateDescendantsQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

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

		protected override Questions.EnumerateDescendantsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateDescendantsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
