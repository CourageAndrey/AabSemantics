using System;
using System.Linq;

using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class EnumeratePartsProcessor : QuestionProcessor<EnumeratePartsQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<EnumeratePartsQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<HasPartStatement>(activeContexts).Where(c => c.Whole == question.Concept).ToList();
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Part).ToList().Enumerate(out format);
				parameters.Add("#PARENT#", question.Concept);
				return new Answer(
					statements.Select(s => s.Part),
					new FormattedText(() => context.Language.Answers.EnumerateParts + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
