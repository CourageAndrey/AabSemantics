﻿using System;
using System.Collections.Generic;

using Inventor.Core.Statements;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsProcessor : QuestionProcessor<IsQuestion>
	{
		public override Answer Process(ProcessingContext<IsQuestion> context)
		{
			var question = context.QuestionX;
			var explanation = new List<IsStatement>();
			bool yes = context.KnowledgeBase.Statements.GetParentsAllLevels(question.ChildConcept, explanation).Contains(question.ParentConcept);
			return new Answer(
				yes,
				new FormattedText(
					yes ? new Func<string>(() => context.Language.Answers.IsTrue) : () => context.Language.Answers.IsFalse,
					new Dictionary<string, INamed>
					{
						{ "#CHILD#", question.ChildConcept },
						{ "#PARENT#", question.ParentConcept },
					}),
				new Explanation(explanation));
		}
	}
}
