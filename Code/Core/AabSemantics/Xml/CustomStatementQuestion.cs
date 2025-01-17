using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Xml
{
	[XmlType]
	public class CustomStatementQuestion : Question<Questions.CustomStatementQuestion>
	{
		#region Properties

		[XmlElement]
		public String Type
		{ get; set; }

		[XmlElement]
		public Dictionary<String, String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatementQuestion()
		{ }

		public CustomStatementQuestion(Questions.CustomStatementQuestion question)
			: base(question)
		{
			Type = question.Type;
			Concepts = question.Concepts.ToDictionary(
				c => c.Key,
				c => c.Value.ID);
		}

		#endregion

		protected override Questions.CustomStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CustomStatementQuestion(
				Type,
				Concepts.ToDictionary(
					c => c.Key,
					c => conceptIdResolver.GetConceptById(c.Value)),
				preconditions);
		}
	}
}
