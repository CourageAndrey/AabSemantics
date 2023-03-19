using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class IsSubjectAreaQuestion : Question<Questions.IsSubjectAreaQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		[XmlElement]
		public String Area
		{ get; set; }

		#endregion

		#region Constructors

		public IsSubjectAreaQuestion()
		{ }

		public IsSubjectAreaQuestion(Questions.IsSubjectAreaQuestion question)
		{
			Concept = question.Concept.ID;
			Area = question.Area.ID;
		}

		#endregion

		protected override Questions.IsSubjectAreaQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsSubjectAreaQuestion(
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Area),
				preconditions);
		}
	}
}
