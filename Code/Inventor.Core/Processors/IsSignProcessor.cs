using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class IsSignProcessor : QuestionProcessor<IsSignQuestion, HasSignStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			bool isSign = context.Question.Concept.HasAttribute<IsSignAttribute>();
			return new BooleanAnswer(
				isSign,
				new FormattedText(
					isSign ? new Func<String>(() => context.Language.Answers.SignTrue) : () => context.Language.Answers.SignFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, context.Question.Concept },
					}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<IsSignQuestion> context, HasSignStatement statement)
		{
			return statement.Sign == context.Question.Concept;
		}
	}
}
