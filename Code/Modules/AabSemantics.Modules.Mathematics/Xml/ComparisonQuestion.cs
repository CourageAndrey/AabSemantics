using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Mathematics.Xml
{
	/// <summary>XML surrogate of a <see cref="Questions.ComparisonQuestion"/>.</summary>
	[XmlType]
	public class ComparisonQuestion : Question<Questions.ComparisonQuestion>
	{
		#region Properties

		/// <summary>Identifier of the left-hand value.</summary>
		[XmlElement]
		public String LeftValue
		{ get; set; }

		/// <summary>Identifier of the right-hand value.</summary>
		[XmlElement]
		public String RightValue
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public ComparisonQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public ComparisonQuestion(Questions.ComparisonQuestion question)
			: base(question)
		{
			LeftValue = question.LeftValue.ID;
			RightValue = question.RightValue.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.ComparisonQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.ComparisonQuestion(
				conceptIdResolver.GetConceptById(LeftValue),
				conceptIdResolver.GetConceptById(RightValue),
				preconditions);
		}
	}
}
