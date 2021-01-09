using System;
using System.Collections.Generic;

using Inventor.Core.Statements;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsProcessor : QuestionProcessor<IsQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, IsQuestion question, ILanguage language)
		{
			var explanation = new List<IsStatement>();
			bool yes = knowledgeBase.Statements.GetParentsAllLevels(question.ChildConcept, explanation).Contains(question.ParentConcept);
			return new FormattedText(
				yes ? new Func<string>(() => language.Answers.IsTrue) : () => language.Answers.IsFalse,
				new Dictionary<string, INamed>
				{
					{ "#CHILD#", question.ChildConcept },
					{ "#PARENT#", question.ParentConcept },
				});
		}
	}
}
