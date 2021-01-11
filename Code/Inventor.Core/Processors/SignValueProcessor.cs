using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class SignValueProcessor : QuestionProcessor<SignValueQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<SignValueQuestion> context)
		{
			var question = context.QuestionX;
			var signValues = context.KnowledgeBase.Statements.OfType<SignValueStatement>().ToList();
			var statement = getSignValue(signValues, question.Concept, question.Sign);
			FormattedText description = null;
			if (statement != null)
			{
				description = formatSignValue(statement, question.Concept, context.Language);
			}
			else
			{
				var parents = context.KnowledgeBase.Statements.GetParentsAllLevels<IConcept, IsStatement>(question.Concept);
				foreach (var parent in parents)
				{
					statement = getSignValue(signValues, parent, question.Sign);
					if (statement != null)
					{
						description = formatSignValue(statement, question.Concept, context.Language);
						break;
					}
				}
			}
			return description != null
				? new Answer(null, description, new Explanation(statement))
				: Answer.CreateUnknown(context.Language);
		}

		private static SignValueStatement getSignValue(IEnumerable<SignValueStatement> statements, IConcept concept, IConcept sign)
		{
			return statements.FirstOrDefault(v => v.Concept == concept && v.Sign == sign);
		}

		private static FormattedText formatSignValue(SignValueStatement value, IConcept original, ILanguage language)
		{
			return value != null
				? new FormattedText(
					() => language.Answers.SignValue,
					new Dictionary<String, INamed>
					{
						{ "#CONCEPT#", original },
						{ "#SIGN#", value.Sign },
						{ "#VALUE#", value.Value },
						{ "#DEFINED#", value.Concept },
					})
				: null;
		}
	}
}
