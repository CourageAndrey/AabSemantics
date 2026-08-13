using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Processes.Json
{
	/// <summary>JSON surrogate of a <see cref="Questions.ProcessesQuestion"/>.</summary>
	[DataContract]
	public class ProcessesQuestion : Serialization.Json.Question<Questions.ProcessesQuestion>
	{
		#region Properties

		/// <summary>Identifier of the first process.</summary>
		[DataMember]
		public String ProcessA
		{ get; set; }

		/// <summary>Identifier of the second process.</summary>
		[DataMember]
		public String ProcessB
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public ProcessesQuestion()
			: base()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public ProcessesQuestion(Questions.ProcessesQuestion question)
			: base(question)
		{
			ProcessA = question.ProcessA.ID;
			ProcessB = question.ProcessB.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.ProcessesQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.ProcessesQuestion(
				conceptIdResolver.GetConceptById(ProcessA),
				conceptIdResolver.GetConceptById(ProcessB),
				preconditions);
		}
	}
}
