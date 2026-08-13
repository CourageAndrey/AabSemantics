using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	/// <summary>JSON surrogate of a <see cref="Questions.HasSignQuestion"/>.</summary>
	[DataContract]
	public class HasSignQuestion : Serialization.Json.Question<Questions.HasSignQuestion>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[DataMember]
		public String Concept
		{ get; set; }

		/// <summary>Identifier of the sign concept.</summary>
		[DataMember]
		public String Sign
		{ get; set; }

		/// <summary>Whether inherited knowledge is taken into account.</summary>
		[DataMember]
		public System.Boolean Recursive
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public HasSignQuestion()
			: base()
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
