using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class EnumerateContainersQuestion : Question<Questions.EnumerateContainersQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; }

		#endregion

		#region Constructors

		public EnumerateContainersQuestion()
		{ }

		public EnumerateContainersQuestion(Questions.EnumerateContainersQuestion question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateContainersQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateContainersQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
