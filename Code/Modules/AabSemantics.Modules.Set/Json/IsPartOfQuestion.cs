using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	/// <summary>JSON surrogate of a <see cref="Questions.IsPartOfQuestion"/>.</summary>
	[DataContract]
	public class IsPartOfQuestion : Serialization.Json.Question<Questions.IsPartOfQuestion>
	{
		#region Properties

		/// <summary>Identifier of the containing concept.</summary>
		[DataMember]
		public String Parent
		{ get; set; }

		/// <summary>Identifier of the contained concept.</summary>
		[DataMember]
		public String Child
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public IsPartOfQuestion()
			: base()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public IsPartOfQuestion(Questions.IsPartOfQuestion question)
			: base(question)
		{
			Parent = question.Parent.ID;
			Child = question.Child.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.IsPartOfQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsPartOfQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
