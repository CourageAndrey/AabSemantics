using System;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class EnumerateContainersProcessor : QuestionProcessor<EnumerateContainersQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<EnumerateContainersQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<HasPartStatement>(activeContexts).Where(c => c.Part == question.Concept).ToList();
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Whole).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamChild, question.Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Whole).ToList(),
					new FormattedText(() => context.Language.Answers.EnumerateContainers + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
