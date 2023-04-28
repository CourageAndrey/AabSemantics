using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class GetCommonQuestion : Serialization.Json.Question<Questions.GetCommonQuestion>
	{
		#region Properties

		[DataMember]
		public List<String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public GetCommonQuestion()
			: base()
		{
			Concepts = new List<String>();
		}

		public GetCommonQuestion(Questions.GetCommonQuestion question)
			: base(question)
		{
			Concepts = question.Concepts.Select(concept => concept.ID).ToList();
		}

		#endregion

		protected override Questions.GetCommonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetCommonQuestion(
				Concepts.Select(concept => conceptIdResolver.GetConceptById(concept)).ToList(),
				preconditions);
		}
	}
}
