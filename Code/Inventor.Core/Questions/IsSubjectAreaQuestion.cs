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
	public sealed class IsSubjectAreaQuestion : Question<IsSubjectAreaQuestion, GroupStatement>
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

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsSubjectAreaQuestion> context, ICollection<GroupStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<String>(() => context.Language.Answers.IsSubjectAreaTrue) : () => context.Language.Answers.IsSubjectAreaFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamArea, context.Question.Area },
						{ Strings.ParamConcept, context.Question.Concept },
					}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<IsSubjectAreaQuestion> context, GroupStatement statement)
		{
			return statement.Area == context.Question.Area && statement.Concept == context.Question.Concept;
		}

		protected override bool NeedToCheckTransitives(IQuestionProcessingContext<IsSubjectAreaQuestion> context, ICollection<GroupStatement> statements)
		{
			return false;
		}
	}
}
