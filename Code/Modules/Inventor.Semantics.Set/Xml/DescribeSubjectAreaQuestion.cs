using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class DescribeSubjectAreaQuestion : Question<Questions.DescribeSubjectAreaQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; }

		#endregion

		#region Constructors

		public DescribeSubjectAreaQuestion()
		{ }

		public DescribeSubjectAreaQuestion(Questions.DescribeSubjectAreaQuestion question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.DescribeSubjectAreaQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.DescribeSubjectAreaQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
