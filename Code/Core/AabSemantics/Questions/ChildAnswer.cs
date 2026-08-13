using System.Collections.Generic;

namespace AabSemantics.Questions
{
	/// <summary>The outcome of a <see cref="NestedQuestion"/>, ready to be folded into the parent answer.</summary>
	public class ChildAnswer
	{
		/// <summary>The follow-up question that was asked.</summary>
		public IQuestion Question
		{ get; }

		/// <summary>Its answer.</summary>
		public IAnswer Answer
		{ get; }

		/// <summary>Statements that led to the follow-up question, prepended to the merged explanation.</summary>
		public ICollection<IStatement> TransitiveStatements
		{ get; }

		/// <summary>Creates a nested answer.</summary>
		/// <param name="question">The follow-up question that was asked.</param>
		/// <param name="answer">Its answer.</param>
		/// <param name="transitiveStatements">Statements that led to the question.</param>
		public ChildAnswer(IQuestion question, IAnswer answer, ICollection<IStatement> transitiveStatements)
		{
			Question = question;
			Answer = answer;
			TransitiveStatements = transitiveStatements;
		}
	}
}