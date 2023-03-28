using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class EnumerateContainersQuestion : Question<Questions.EnumerateContainersQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public EnumerateContainersQuestion()
		{ }

		public EnumerateContainersQuestion(Questions.EnumerateContainersQuestion question)
			: base(question)
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
