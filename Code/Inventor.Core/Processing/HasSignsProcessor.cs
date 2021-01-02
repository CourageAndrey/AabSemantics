using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class HasSignsProcessor : QuestionProcessor<HasSignsQuestion>
	{
		protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, HasSignsQuestion question, ILanguageEx language)
		{
			bool yes = HasSignStatement.GetSigns(knowledgeBase.Statements, question.Concept, question.Recursive).Select(hs => hs.Sign).Any();
			return new FormattedText(
				() => string.Format(yes ? language.Answers.HasSignsTrue : language.Answers.HasSignsFalse, question.Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse),
				new Dictionary<string, INamed>
				{
					{ "#CONCEPT#", question.Concept },
				});
		}
	}
}
