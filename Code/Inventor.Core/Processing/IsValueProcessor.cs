using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsValueProcessor : QuestionProcessor<IsValueQuestion>
	{
		protected override Answer ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, IsValueQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<SignValueStatement>().Where(r => r.Value == question.Concept).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<string>(() => language.Answers.ValueTrue) : () => language.Answers.ValueFalse,
					new Dictionary<string, INamed>
					{
						{ "#CONCEPT#", question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
