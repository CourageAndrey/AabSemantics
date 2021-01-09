using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class SignValueProcessor : QuestionProcessor<SignValueQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, SignValueQuestion question, ILanguage language)
		{
			var signValues = knowledgeBase.Statements.OfType<SignValueStatement>();
			var result = getSignValue(signValues, question.Concept, question.Sign, question.Concept, language);
			if (result != null)
			{
				return result;
			}
			else
			{
				var parents = knowledgeBase.Statements.GetParentsTree<Concept, IsStatement>(question.Concept);
				foreach (var parent in parents)
				{
					result = getSignValue(signValues, parent, question.Sign, question.Concept, language);
					if (result != null)
					{
						return result;
					}
				}
			}
			return AnswerHelper.CreateUnknown(language);
		}

		private static FormattedText getSignValue(IEnumerable<SignValueStatement> statements, Concept concept, Concept sign, Concept original, ILanguage language)
		{
			var value = statements.FirstOrDefault(v => v.Concept == concept && v.Sign == sign);
			return value != null
				? new FormattedText(
					() => language.Answers.SignValue,
					new Dictionary<string, INamed>
					{
						{ "#CONCEPT#", original },
						{ "#SIGN#", sign },
						{ "#VALUE#", value.Value },
						{ "#DEFINED#", concept },
					})
				: null;
		}
	}
}
