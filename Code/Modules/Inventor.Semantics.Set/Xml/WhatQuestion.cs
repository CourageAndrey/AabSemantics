using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class WhatQuestion : Question<Questions.WhatQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public WhatQuestion()
		{ }

		public WhatQuestion(Questions.WhatQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.WhatQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.WhatQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
}
}
