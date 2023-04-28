using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class GetDifferencesQuestion : Question<Questions.GetDifferencesQuestion>
	{
		#region Properties

		[XmlArray(nameof(Concepts))]
		[XmlArrayItem("Concept")]
		public List<String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public GetDifferencesQuestion()
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
