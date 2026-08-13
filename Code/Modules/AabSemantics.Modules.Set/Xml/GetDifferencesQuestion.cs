using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Questions.GetDifferencesQuestion"/>.</summary>
	[XmlType]
	public class GetDifferencesQuestion : Question<Questions.GetDifferencesQuestion>
	{
		#region Properties

		/// <summary>Identifier of the first compared concept.</summary>
		[XmlElement]
		public String Concept1
		{ get; set; }

		/// <summary>Identifier of the second compared concept.</summary>
		[XmlElement]
		public String Concept2
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public GetDifferencesQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public GetDifferencesQuestion(Questions.GetDifferencesQuestion question)
			: base(question)
		{
			Concept1 = question.Concept1.ID;
			Concept2 = question.Concept2.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.GetDifferencesQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.GetDifferencesQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
