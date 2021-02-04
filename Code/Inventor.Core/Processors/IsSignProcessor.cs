using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class IsSignProcessor : QuestionProcessor<IsSignQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<IsSignQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<HasSignStatement>(activeContexts).Where(r => r.Sign == question.Concept).ToList();
			bool isSign = question.Concept.HasAttribute<IsSignAttribute>();
			return new BooleanAnswer(
				isSign,
				new FormattedText(
					isSign ? new Func<String>(() => context.Language.Answers.SignTrue) : () => context.Language.Answers.SignFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
