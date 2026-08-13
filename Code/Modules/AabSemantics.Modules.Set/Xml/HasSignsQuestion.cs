using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Questions.HasSignsQuestion"/>.</summary>
	[XmlType]
	public class HasSignsQuestion : Question<Questions.HasSignsQuestion>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[XmlElement]
		public String Concept
		{ get; set; }

		/// <summary>Whether inherited knowledge is taken into account.</summary>
		[XmlElement]
		public System.Boolean Recursive
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public HasSignsQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public HasSignsQuestion(Questions.HasSignsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Recursive = question.Recursive;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.HasSignsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.HasSignsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				Recursive,
				preconditions);
		}
	}
}
