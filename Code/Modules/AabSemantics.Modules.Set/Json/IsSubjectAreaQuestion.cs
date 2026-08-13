using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	/// <summary>JSON surrogate of a <see cref="Questions.IsSubjectAreaQuestion"/>.</summary>
	[DataContract]
	public class IsSubjectAreaQuestion : Serialization.Json.Question<Questions.IsSubjectAreaQuestion>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[DataMember]
		public String Concept
		{ get; set; }

		/// <summary>Identifier of the subject area concept.</summary>
		[DataMember]
		public String Area
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public IsSubjectAreaQuestion()
			: base()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public IsSubjectAreaQuestion(Questions.IsSubjectAreaQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Area = question.Area.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.IsSubjectAreaQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsSubjectAreaQuestion(
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Area),
				preconditions);
		}
	}
}
