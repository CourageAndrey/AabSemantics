using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
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
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.IsSignQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsSignQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
