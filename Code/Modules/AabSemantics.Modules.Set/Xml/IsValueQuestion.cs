using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	[XmlType]
	public class IsValueQuestion : Question<Questions.IsValueQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public IsValueQuestion()
		{ }

		public IsValueQuestion(Questions.IsValueQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.IsValueQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsValueQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
