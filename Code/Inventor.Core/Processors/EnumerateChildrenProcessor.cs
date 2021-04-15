﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class EnumerateChildrenProcessor : QuestionProcessor<EnumerateChildrenQuestion, IsStatement>
	{
		public override IAnswer Process(IQuestionProcessingContext<EnumerateChildrenQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(activeContexts).Where(c => c.Ancestor == question.Concept).ToList();
			return CreateAnswer(context, statements);
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateChildrenQuestion> context, ICollection<IsStatement> statements)
		{
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Descendant).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamParent, context.Question.Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Descendant).ToList(),
					new FormattedText(() => context.Language.Answers.Enumerate + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
