using System;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class EnumeratePartsProcessor : QuestionProcessor<EnumeratePartsQuestion>
	{
		public override IAnswer Process(IProcessingContext<EnumeratePartsQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<ConsistsOfStatement>().Where(c => c.Parent == question.Concept).ToList();
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Child).ToList().Enumerate(out format);
				parameters.Add("#PARENT#", question.Concept);
				return new Answer(
					statements.Select(s => s.Child),
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
