using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;

namespace AabSemantics.Modules.Classification.Json
{
	/// <summary>JSON surrogate of the <see cref="Questions.IsQuestion"/> question.</summary>
	[DataContract]
	public class IsQuestion : Question<Questions.IsQuestion>
	{
		#region Properties

		/// <summary>Identifier of the child concept.</summary>
		[DataMember]
		public String Child
		{ get; set; }

		/// <summary>Identifier of the parent concept.</summary>
		[DataMember]
		public String Parent
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public IsQuestion()
			: base()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public IsQuestion(Questions.IsQuestion question)
			: base(question)
		{
			Parent = question.Parent.ID;
			Child = question.Child.ID;
		}

		#endregion

		/// <summary>Rebuilds the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already rebuilt by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.IsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
