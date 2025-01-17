using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AabSemantics.Serialization.Xml
{
	[XmlType]
	public class CustomStatementQuestion : Question<Questions.CustomStatementQuestion>
	{
		#region Properties

		[XmlElement]
		public String Type
		{ get; set; }

		[XmlElement]
		public List<KeyedConcept> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatementQuestion()
		{ }

		public CustomStatementQuestion(Questions.CustomStatementQuestion question)
			: base(question)
		{
			Type = question.Type;
			Concepts = question.Concepts.Select(c => new KeyedConcept(c.Key, c.Value.ID)).ToList();
		}

		#endregion

		protected override Questions.CustomStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CustomStatementQuestion(
				Type,
				Concepts.ToDictionary(
					c => c.Key,
					c => conceptIdResolver.GetConceptById(c.Concept)),
				preconditions);
		}
	}
}
