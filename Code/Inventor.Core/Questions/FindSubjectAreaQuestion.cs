using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class FindSubjectAreaQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public FindSubjectAreaQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<FindSubjectAreaQuestion, GroupStatement>(DoesStatementMatch)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<FindSubjectAreaQuestion> context, ICollection<GroupStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			if (!NeedToCheckTransitives(statements))
			{
				if (statements.Any())
				{
					var result = new FormattedText();
					foreach (var statement in statements)
					{
						result.Add(() => context.Language.Answers.SubjectArea, new Dictionary<String, INamed>
						{
							{ Strings.ParamConcept, Concept },
							{ Strings.ParamArea, statement.Area },
						});
					}
					return new ConceptsAnswer(
						statements.Select(s => s.Area).ToList(),
						result,
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

		private Boolean DoesStatementMatch(GroupStatement statement)
		{
			return statement.Concept == Concept;
		}

		private Boolean NeedToCheckTransitives(ICollection<GroupStatement> statements)
		{
			return statements.Count == 0;
		}

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<FindSubjectAreaQuestion> context, ICollection<GroupStatement> statements, ICollection<ChildAnswer> childAnswers)
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
