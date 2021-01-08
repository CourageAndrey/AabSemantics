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
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, IsValueQuestion question, ILanguage language)
		{
			bool yes = knowledgeBase.Statements.OfType<SignValueStatement>().FirstOrDefault(r => r.Value == question.Concept) != null;
			return new FormattedText(
				yes ? new Func<string>(() => language.Answers.ValueTrue) : () => language.Answers.ValueFalse,
				new Dictionary<string, INamed>
				{
					{ "#CONCEPT#", question.Concept },
				});
		}
	}
}
