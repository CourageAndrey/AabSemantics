using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class ProcessesQuestionProcessor : QuestionProcessor<ProcessesQuestion, ProcessesStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<ProcessesQuestion> context, ICollection<ProcessesStatement> statements)
		{
			if (statements.Any())
			{
				var statement = statements.First();
				return new ConceptAnswer(
					statement.SequenceSign,
					new FormattedText(() => context.Language.Answers.ProcessesSequence, new Dictionary<String, INamed>
					{
						{ Strings.ParamProcessA, statement.ProcessA },
						{ Strings.ParamSequenceSign, statement.SequenceSign },
						{ Strings.ParamProcessB, statement.ProcessB },
					}),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected override bool DoesStatementMatch(IQuestionProcessingContext<ProcessesQuestion> context, ProcessesStatement statement)
		{
			return	(statement.ProcessA == context.Question.ProcessA && statement.ProcessB == context.Question.ProcessB) ||
					(statement.ProcessB == context.Question.ProcessA && statement.ProcessA == context.Question.ProcessB);
		}
	}
}
