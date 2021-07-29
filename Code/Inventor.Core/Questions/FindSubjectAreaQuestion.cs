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
				.From<FindSubjectAreaQuestion, GroupStatement>(s => s.Concept == Concept)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<FindSubjectAreaQuestion> context, ICollection<GroupStatement> statements, ICollection<ChildAnswer> childAnswers)
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

			var childAnswer = childAnswers.FirstOrDefault();
			if (childAnswer != null)
			{
				childAnswer.Answer.Explanation.Expand(childAnswer.TransitiveStatements);
				return childAnswer.Answer;
			}

			return Answer.CreateUnknown(context.Language);
		}
	}
}
