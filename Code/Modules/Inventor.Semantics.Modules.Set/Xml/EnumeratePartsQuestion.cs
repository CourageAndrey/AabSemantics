using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class EnumeratePartsQuestion : Question<Questions.EnumeratePartsQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public EnumeratePartsQuestion()
		{ }

		public EnumeratePartsQuestion(Questions.EnumeratePartsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumeratePartsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumeratePartsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
