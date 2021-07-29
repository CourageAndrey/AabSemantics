﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateChildrenQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateChildrenQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateChildrenQuestion, IsStatement>(DoesStatementMatch)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateChildrenQuestion> context, ICollection<IsStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			if (!NeedToCheckTransitives(statements))
			{
				if (statements.Any())
				{
					String format;
					var parameters = statements.Select(r => r.Descendant).ToList().Enumerate(out format);
					parameters.Add(Strings.ParamParent, Concept);
					return new ConceptsAnswer(
						statements.Select(s => s.Descendant).ToList(),
						new FormattedText(() => context.Language.Answers.Enumerate + format + ".", parameters),
						new Explanation(statements));
				}
				else
				{
					return Answer.CreateUnknown(context.Language);
				}
			}
			else
			{
				return ProcessChildAnswers(context, statements, childAnswers);
			}
		}

		private Boolean DoesStatementMatch(IsStatement statement)
		{
			return statement.Ancestor == Concept;
		}

		private Boolean NeedToCheckTransitives(ICollection<IsStatement> statements)
		{
			return statements.Count == 0;
		}

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<EnumerateChildrenQuestion> context, ICollection<IsStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			if (childAnswers.Count > 0)
			{
				var answer = childAnswers.First();
				answer.Answer.Explanation.Expand(answer.TransitiveStatements);
				return answer.Answer;
			}
			else
			{
				return Answers.Answer.CreateUnknown(context.Language);
			}
		}
	}
}
