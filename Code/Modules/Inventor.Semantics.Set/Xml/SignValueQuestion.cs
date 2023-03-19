using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class SignValueQuestion : Question<Questions.SignValueQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		[XmlElement]
		public String Sign
		{ get; set; }

		#endregion

		#region Constructors

		public SignValueQuestion()
		{ }

		public SignValueQuestion(Questions.SignValueQuestion question)
		{
			Concept = question.Concept.ID;
			Sign = question.Sign.ID;
		}

		#endregion

		protected override Questions.SignValueQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.SignValueQuestion(
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign),
				preconditions);
		}
	}
}
