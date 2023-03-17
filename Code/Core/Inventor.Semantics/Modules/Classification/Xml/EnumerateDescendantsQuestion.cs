using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Modules.Classification.Xml
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
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateDescendantsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateDescendantsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
