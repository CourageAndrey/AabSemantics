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
	public sealed class IsValueProcessor : QuestionProcessor<IsValueQuestion, SignValueStatement>
	{
		public override IAnswer Process(IQuestionProcessingContext<IsValueQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<SignValueStatement>(activeContexts).Where(r => r.Value == question.Concept).ToList();
			return CreateAnswer(context, statements);
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsValueQuestion> context, ICollection<SignValueStatement> statements)
		{
			bool isValue = context.Question.Concept.HasAttribute<IsValueAttribute>();
			return new BooleanAnswer(
				isValue,
				new FormattedText(
					isValue ? new Func<String>(() => context.Language.Answers.ValueTrue) : () => context.Language.Answers.ValueFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, context.Question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
