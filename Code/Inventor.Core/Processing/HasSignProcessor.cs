using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class HasSignProcessor : QuestionProcessor<HasSignQuestion>
	{
		protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, HasSignQuestion question, ILanguage language)
		{
			bool yes = HasSignStatement.GetSigns(knowledgeBase.Statements, question.Concept, question.Recursive).Select(hs => hs.Sign).Contains(question.Sign);
			return new FormattedText(
				() => string.Format(yes ? language.Answers.HasSignTrue : language.Answers.HasSignFalse, question.Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse),
				new Dictionary<string, INamed>
				{
					{ "#CONCEPT#", question.Concept },
					{ "#SIGN#", question.Sign },
				});
		}
	}
}
