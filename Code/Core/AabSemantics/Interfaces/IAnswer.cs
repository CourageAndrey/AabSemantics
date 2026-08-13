using System;

using AabSemantics.Text.Containers;

namespace AabSemantics
{
	/// <summary>
	/// The outcome of asking an <see cref="IQuestion"/>: what the answer is, and why.
	/// </summary>
	public interface IAnswer
	{
		/// <summary>
		/// The answer itself, as localizable text ready to be shown to the user.
		/// </summary>
		IText Description
		{ get; }

		/// <summary>
		/// The statements the answer was derived from, forming its proof.
		/// </summary>
		IExplanation Explanation
		{ get; }

		/// <summary>
		/// <c>true</c> when the network holds no knowledge to answer the question.
		/// An empty answer is not a negative answer: it means "unknown", not "no".
		/// </summary>
		Boolean IsEmpty
		{ get; }
	}

	/// <summary>
	/// An answer that also exposes its outcome as a typed value, so callers can act on it
	/// programmatically instead of parsing the rendered text.
	/// </summary>
	/// <typeparam name="TResult">Type of the answer's value, e.g. <see cref="Boolean"/> or a concept collection.</typeparam>
	public interface IAnswer<out TResult> : IAnswer
	{
		/// <summary>
		/// The answer's value. Meaningless when <see cref="IAnswer.IsEmpty"/> is <c>true</c>.
		/// </summary>
		TResult Result
		{ get; }
	}

	/// <summary>
	/// Helpers for presenting an answer together with its proof.
	/// </summary>
	public static class AnswerExtensions
	{
		/// <summary>
		/// Builds the answer's text followed by the statements it was derived from.
		/// </summary>
		/// <param name="answer">Answer to render.</param>
		/// <returns>
		/// The description alone when the explanation is empty; otherwise the description,
		/// an "explanation" caption, and one affirmative sentence per supporting statement.
		/// </returns>
		public static IText GetDescriptionWithExplanation(this IAnswer answer)
		{
			if (answer.Explanation.Statements.Count > 0)
			{
				var explanedResult = new UnstructuredContainer(answer.Description);

				explanedResult.AppendLineBreak();
				explanedResult.Append(new Text.Primitives.FormattedText(language => language.Questions.Answers.Explanation));

				foreach (var statement in answer.Explanation.Statements)
				{
					explanedResult.Append(statement.DescribeTrue());
				}

				return explanedResult;
			}
			else
			{
				return answer.Description;
			}
		}
	}
}
