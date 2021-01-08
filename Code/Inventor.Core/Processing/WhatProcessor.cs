using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class WhatProcessor : QuestionProcessor<WhatQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, WhatQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<IsStatement>().Where(c => c.Child == question.Concept);
			if (statements.Any())
			{
				var result = new FormattedText();
				foreach (var statement in statements)
				{
					var difference = new List<SignValueStatement>();
					foreach (var sign in HasSignStatement.GetSigns(knowledgeBase.Statements, statement.Parent, false))
					{
						var diff = SignValueStatement.GetSignValue(knowledgeBase.Statements, question.Concept, sign.Sign);
						if (diff != null)
						{
							difference.Add(diff);
						}
					}
					if (difference.Count > 0)
					{
						result.Add(() => language.Answers.IsDescriptionWithSign, new Dictionary<string, INamed>
						{
							{ "#CHILD#", question.Concept },
							{ "#PARENT#", statement.Parent },
						});
						foreach (var diff in difference)
						{
							result.Add(() => language.Answers.IsDescriptionWithSignValue, new Dictionary<string, INamed>
							{
								{ "#SIGN#", diff.Sign },
								{ "#VALUE#", diff.Value },
							});
						}
					}
					else
					{
						result.Add(() => language.Answers.IsDescription, new Dictionary<string, INamed>
						{
							{ "#CHILD#", question.Concept },
							{ "#PARENT#", statement.Parent },
						});
					}
					result.Add(() => string.Empty, new Dictionary<string, INamed>());
				}
				return result;
			}
			else
			{
				return AnswerHelper.CreateUnknown(language);
			}
		}
	}
}
