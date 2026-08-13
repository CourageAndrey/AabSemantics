using System.Collections.Generic;

namespace AabSemantics.Questions
{
	/// <summary>
	/// A follow-up question a processor asks when the direct statements do not settle the matter,
	/// together with the statements that justify asking it.
	/// </summary>
	public class NestedQuestion
	{
		/// <summary>The question to ask next.</summary>
		public IQuestion Question
		{ get; }

		/// <summary>Statements that led to this question; they become part of the final explanation.</summary>
		public ICollection<IStatement> TransitiveStatements
		{ get; }

		/// <summary>Creates a follow-up question.</summary>
		/// <param name="question">The question to ask next.</param>
		/// <param name="transitiveStatements">Statements that led to it.</param>
		public NestedQuestion(IQuestion question, ICollection<IStatement> transitiveStatements)
		{
			Question = question;
			TransitiveStatements = transitiveStatements;
		}
	}
}