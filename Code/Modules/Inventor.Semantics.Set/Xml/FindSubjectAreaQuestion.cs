using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class FindSubjectAreaQuestion : Question<Questions.FindSubjectAreaQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; }

		#endregion

		#region Constructors

		public FindSubjectAreaQuestion()
		{ }

		public FindSubjectAreaQuestion(Questions.FindSubjectAreaQuestion question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.FindSubjectAreaQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.FindSubjectAreaQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
