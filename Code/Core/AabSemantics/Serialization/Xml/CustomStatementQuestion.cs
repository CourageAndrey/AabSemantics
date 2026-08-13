using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>XML surrogate of the custom-statement lookup question.</summary>
	[XmlType]
	public class CustomStatementQuestion : Question<Questions.CustomStatementQuestion>
	{
		#region Properties

		/// <summary>Identifier of the declared statement kind.</summary>
		[XmlElement]
		public String Type
		{ get; set; }

		/// <summary>Concept identifiers filling the kind's roles, keyed by role name.</summary>
		[XmlElement]
		public List<KeyedConcept> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public CustomStatementQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public CustomStatementQuestion(Questions.CustomStatementQuestion question)
			: base(question)
		{
			Type = question.Type;
			Concepts = question.Concepts.Select(c => new KeyedConcept(c.Key, c.Value.ID)).ToList();
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.CustomStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CustomStatementQuestion(
				Type,
				Concepts.ToDictionary(
					c => c.Key,
					c => conceptIdResolver.GetConceptById(c.Concept)),
				preconditions);
		}
	}
}
