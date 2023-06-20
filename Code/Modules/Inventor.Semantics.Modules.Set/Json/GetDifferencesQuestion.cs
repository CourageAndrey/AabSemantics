using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Modules.Set.Json
{
	[DataContract]
	public class GetDifferencesQuestion : Serialization.Json.Question<Questions.GetDifferencesQuestion>
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

		public GetDifferencesQuestion()
			: base()
		{ }

		public GetDifferencesQuestion(Questions.GetDifferencesQuestion question)
			: base(question)
		{
			Concept1 = question.Concept1.ID;
			Concept2 = question.Concept2.ID;
		}

		#endregion

		protected override Questions.GetDifferencesQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetDifferencesQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
