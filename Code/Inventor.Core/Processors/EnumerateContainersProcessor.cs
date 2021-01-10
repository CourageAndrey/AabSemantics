using System;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class EnumerateContainersProcessor : QuestionProcessor<EnumerateContainersQuestion>
	{
		public override IAnswer Process(IProcessingContext<EnumerateContainersQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<ConsistsOfStatement>().Where(c => c.Child == question.Concept).ToList();
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Parent).ToList().Enumerate(out format);
				parameters.Add("#CHILD#", question.Concept);
				return new Answer(
					statements.Select(s => s.Parent),
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
