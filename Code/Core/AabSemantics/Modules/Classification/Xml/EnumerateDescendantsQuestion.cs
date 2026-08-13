using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Classification.Xml
{
	/// <summary>XML surrogate of the <see cref="Questions.EnumerateDescendantsQuestion"/> question.</summary>
	[XmlType]
	public class EnumerateDescendantsQuestion : Question<Questions.EnumerateDescendantsQuestion>
	{
		#region Properties

		/// <summary>Identifier of the concept concept.</summary>
		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public EnumerateDescendantsQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public EnumerateDescendantsQuestion(Questions.EnumerateDescendantsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		/// <summary>Rebuilds the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already rebuilt by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.EnumerateDescendantsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateDescendantsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
