using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Questions.WhatQuestion"/>.</summary>
	[XmlType]
	public class WhatQuestion : Question<Questions.WhatQuestion>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public WhatQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public WhatQuestion(Questions.WhatQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.WhatQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.WhatQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
