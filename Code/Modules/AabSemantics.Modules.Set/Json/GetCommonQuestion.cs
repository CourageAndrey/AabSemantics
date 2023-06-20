using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	[DataContract]
	public class GetCommonQuestion : Serialization.Json.Question<Questions.GetCommonQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept1
		{ get; set; }

		[DataMember]
		public String Concept2
		{ get; set; }

		#endregion

		#region Constructors

		public GetCommonQuestion()
			: base()
		{ }

		public GetCommonQuestion(Questions.GetCommonQuestion question)
			: base(question)
		{
			Concept1 = question.Concept1.ID;
			Concept2 = question.Concept2.ID;
		}

		#endregion

		protected override Questions.GetCommonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetCommonQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
