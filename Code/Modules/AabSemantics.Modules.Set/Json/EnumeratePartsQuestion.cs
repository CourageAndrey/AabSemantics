using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	[DataContract]
	public class EnumeratePartsQuestion : Serialization.Json.Question<Questions.EnumeratePartsQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public EnumeratePartsQuestion()
			: base()
		{ }

		public EnumeratePartsQuestion(Questions.EnumeratePartsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumeratePartsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumeratePartsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
