using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public sealed class IsSubjectAreaQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Area
		{ get; }

		#endregion

		public IsSubjectAreaQuestion(IConcept concept, IConcept area, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (area == null) throw new ArgumentNullException(nameof(area));

			Concept = concept;
			Area = area;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsSubjectAreaQuestion, GroupStatement>(DoesStatementMatch)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<IsSubjectAreaQuestion> context, ICollection<GroupStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<String>(() => context.Language.Answers.IsSubjectAreaTrue) : () => context.Language.Answers.IsSubjectAreaFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamArea, Area },
						{ Strings.ParamConcept, Concept },
					}),
				new Explanation(statements));
		}

		private Boolean DoesStatementMatch(GroupStatement statement)
		{
			return statement.Area == Area && statement.Concept == Concept;
		}

		private bool NeedToCheckTransitives(ICollection<GroupStatement> statements)
		{
			return false;
		}

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<IsSubjectAreaQuestion> context, ICollection<GroupStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			throw new NotSupportedException();
		}
	}
}
