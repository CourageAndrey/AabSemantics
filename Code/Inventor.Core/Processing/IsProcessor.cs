using System;
using System.Collections.Generic;

using Inventor.Core.Statements;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsProcessor : QuestionProcessor<IsQuestion>
	{
		protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, IsQuestion question)
		{
			bool yes = IsStatement.GetParentsTree(knowledgeBase.Statements, question.ChildConcept).Contains(question.ParentConcept);
			var language = LanguageEx.CurrentEx.Answers;
			return new FormattedText(
				yes ? new Func<string>(() => language.IsTrue) : () => language.IsFalse,
				new Dictionary<string, INamed>
				{
					{ "#CHILD#", question.ChildConcept },
					{ "#PARENT#", question.ParentConcept },
				});
		}
	}
}
