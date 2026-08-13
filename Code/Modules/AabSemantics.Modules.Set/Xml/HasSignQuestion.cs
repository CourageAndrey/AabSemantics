using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Questions.HasSignQuestion"/>.</summary>
	[XmlType]
	public class HasSignQuestion : Question<Questions.HasSignQuestion>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[XmlElement]
		public String Concept
		{ get; set; }

		/// <summary>Identifier of the sign concept.</summary>
		[XmlElement]
		public String Sign
		{ get; set; }

		/// <summary>Whether inherited knowledge is taken into account.</summary>
		[XmlElement]
		public System.Boolean Recursive
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public HasSignQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public HasSignQuestion(Questions.HasSignQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Sign = question.Sign.ID;
			Recursive = question.Recursive;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.HasSignQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.HasSignQuestion(
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign),
				Recursive,
				preconditions);
		}
	}
}
