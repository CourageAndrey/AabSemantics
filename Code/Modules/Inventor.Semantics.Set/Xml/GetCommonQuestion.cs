using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class GetCommonQuestion : Question<Questions.GetCommonQuestion>
	{
		#region Properties

		[XmlArray(nameof(Concepts))]
		[XmlArrayItem("Concept")]
		public List<String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public GetCommonQuestion()
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
