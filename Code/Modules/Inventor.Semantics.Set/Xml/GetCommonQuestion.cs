using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class GetCommonQuestion : Question<Questions.GetCommonQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept1
		{ get; }

		[XmlElement]
		public String Concept2
		{ get; }

		#endregion

		#region Constructors

		public GetCommonQuestion()
		{ }

		public GetCommonQuestion(Questions.GetCommonQuestion question)
			: base(question)
		{
			Concept1 = question.Concept1.ID;
			Concept2 = question.Concept2.ID;
		}

		#endregion

		protected override Questions.GetCommonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetCommonQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
