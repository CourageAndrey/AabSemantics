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
			bool yes = knowledgeBase.Statements.OfType<ConsistsOfStatement>().Any(c => c.Parent == question.Parent && c.Child == question.Child);
			return new FormattedText(yes ? new Func<string>(() => language.Answers.IsPartOfTrue) : () => language.Answers.IsPartOfFalse, new Dictionary<string, INamed>
			{
				{ "#PARENT#", question.Parent },
				{ "#CHILD#", question.Child },
			});
		}
	}
}
