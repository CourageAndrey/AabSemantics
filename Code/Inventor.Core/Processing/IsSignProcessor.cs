using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsSignProcessor : QuestionProcessor<IsSignQuestion>
	{
		protected override Answer ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, IsSignQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<HasSignStatement>().Where(r => r.Sign == question.Concept).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<string>(() => language.Answers.SignTrue) : () => language.Answers.SignFalse,
					new Dictionary<string, INamed>
					{
						{ "#CONCEPT#", question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
