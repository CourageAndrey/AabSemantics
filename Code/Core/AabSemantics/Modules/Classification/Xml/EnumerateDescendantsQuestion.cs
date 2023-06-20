using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Classification.Xml
{
	[XmlType]
	public class EnumerateDescendantsQuestion : Question<Questions.EnumerateDescendantsQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public EnumerateDescendantsQuestion()
		{ }

		public EnumerateDescendantsQuestion(Questions.EnumerateDescendantsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateDescendantsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateDescendantsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
