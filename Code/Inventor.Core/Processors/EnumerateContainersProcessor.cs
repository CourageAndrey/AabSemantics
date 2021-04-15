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
	public sealed class EnumerateContainersProcessor : QuestionProcessor<EnumerateContainersQuestion, HasPartStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateContainersQuestion> context, ICollection<HasPartStatement> statements)
		{
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Whole).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamChild, context.Question.Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Whole).ToList(),
					new FormattedText(() => context.Language.Answers.EnumerateContainers + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateContainersQuestion> context, HasPartStatement statement)
		{
			return statement.Part == context.Question.Concept;
		}
	}
}
