using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class GetCommonQuestion : Serialization.Json.Question<Questions.GetCommonQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept1
		{ get; }

		[DataMember]
		public String Concept2
		{ get; }

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

		protected override Questions.GetCommonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetCommonQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
