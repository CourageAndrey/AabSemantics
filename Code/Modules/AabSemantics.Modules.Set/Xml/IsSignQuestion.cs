using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	[XmlType]
	public class IsSignQuestion : Question<Questions.IsSignQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public IsSignQuestion()
		{ }

		public IsSignQuestion(Questions.IsSignQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.IsSignQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsSignQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
