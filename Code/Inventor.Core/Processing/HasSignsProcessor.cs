﻿using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class HasSignsProcessor : QuestionProcessor<HasSignsQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, HasSignsQuestion question, ILanguage language)
		{
			var statements = HasSignStatement.GetSigns(knowledgeBase.Statements, question.Concept, question.Recursive);
			return new FormattedText(
				() => string.Format(statements.Any() ? language.Answers.HasSignsTrue : language.Answers.HasSignsFalse, question.Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse),
				new Dictionary<string, INamed>
				{
					{ "#CONCEPT#", question.Concept },
				});
		}
	}
}
