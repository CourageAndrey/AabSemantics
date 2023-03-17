using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Mathematics.Xml
{
	[XmlType]
	public class ComparisonQuestion : Question<Questions.ComparisonQuestion>
	{
		#region Properties

		[XmlElement]
		public String LeftValue
		{ get; set; }

		[XmlElement]
		public String RightValue
		{ get; set; }

		#endregion

		#region Constructors

		public ComparisonQuestion()
		{ }

		public ComparisonQuestion(Questions.ComparisonQuestion question)
		{
			LeftValue = question.LeftValue.ID;
			RightValue = question.RightValue.ID;
		}

		#endregion

		protected override Questions.ComparisonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.ComparisonQuestion(
				conceptIdResolver.GetConceptById(LeftValue),
				conceptIdResolver.GetConceptById(RightValue),
				preconditions);
		}
	}
}
