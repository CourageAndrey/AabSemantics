using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Modules.Classification.Xml
{
	[XmlType]
	public class EnumerateAncestorsQuestion : Question<Questions.EnumerateAncestorsQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public EnumerateAncestorsQuestion()
		{ }

		public EnumerateAncestorsQuestion(Questions.EnumerateAncestorsQuestion question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateAncestorsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateAncestorsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
