using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AabSemantics.Serialization.Json
{
	/// <summary>JSON surrogate of the custom-statement lookup question.</summary>
	[DataContract]
	public class CustomStatementQuestion : Question<Questions.CustomStatementQuestion>
	{
		#region Properties

		/// <summary>Identifier of the declared statement kind.</summary>
		[DataMember]
		public String Type
		{ get; set; }

		/// <summary>Concept identifiers filling the kind's roles, keyed by role name.</summary>
		[DataMember]
		public Dictionary<String, String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public CustomStatementQuestion()
			: base()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public CustomStatementQuestion(Questions.CustomStatementQuestion question)
			: base(question)
		{
			Type = question.Type;
			Concepts = question.Concepts.ToDictionary(
				c => c.Key,
				c => c.Value.ID);
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
					c => conceptIdResolver.GetConceptById(c.Value)),
				preconditions);
		}
	}
}
