using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsPartOfProcessor : QuestionProcessor<IsPartOfQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, IsPartOfQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<ConsistsOfStatement>().Where(c => c.Parent == question.Parent && c.Child == question.Child).ToList();
			return new FormattedText(statements.Any() ? new Func<string>(() => language.Answers.IsPartOfTrue) : () => language.Answers.IsPartOfFalse, new Dictionary<string, INamed>
			{
				{ "#PARENT#", question.Parent },
				{ "#CHILD#", question.Child },
			});
		}
	}
}
