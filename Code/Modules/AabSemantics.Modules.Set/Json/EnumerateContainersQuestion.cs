using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	/// <summary>JSON surrogate of a <see cref="Questions.EnumerateContainersQuestion"/>.</summary>
	[DataContract]
	public class EnumerateContainersQuestion : Serialization.Json.Question<Questions.EnumerateContainersQuestion>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[DataMember]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public EnumerateContainersQuestion()
			: base()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public EnumerateContainersQuestion(Questions.EnumerateContainersQuestion question)
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
		protected override Questions.EnumerateContainersQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateContainersQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
