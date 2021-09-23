using System;

using Inventor.Core.Text.Containers;

namespace Inventor.Core
{
	public interface IAnswer
	{
		IText Description
		{ get; }

		IExplanation Explanation
		{ get; }

		Boolean IsEmpty
		{ get; }
	}

	public interface IAnswer<out TResult> : IAnswer
	{
		TResult Result
		{ get; }
	}

	public static class AnswerExtensions
	{
		public static IText GetDescriptionWithExplanation(this IAnswer answer)
		{
			if (answer.Explanation.Statements.Count > 0)
			{
				var explanedResult = new UnstructuredContainer(answer.Description);

				explanedResult.AppendLineBreak();
				explanedResult.Append(new Core.Text.Primitives.FormattedText(language => language.Questions.Answers.Explanation));

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
