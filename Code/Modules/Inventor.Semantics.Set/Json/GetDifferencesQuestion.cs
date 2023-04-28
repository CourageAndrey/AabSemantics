using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class GetDifferencesQuestion : Serialization.Json.Question<Questions.GetDifferencesQuestion>
	{
		#region Properties

		[DataMember]
		public List<String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public GetDifferencesQuestion()
			: base()
		{
			Concepts = new List<String>();
		}

		public GetDifferencesQuestion(Questions.GetDifferencesQuestion question)
			: base(question)
		{
			Concepts = question.Concepts.Select(concept => concept.ID).ToList();
		}

		#endregion

		protected override Questions.GetDifferencesQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetDifferencesQuestion(
				Concepts.Select(concept => conceptIdResolver.GetConceptById(concept)).ToList(),
				preconditions);
		}
	}
}
