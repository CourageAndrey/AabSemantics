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
	public sealed class IsPartOfQuestion : Question<IsPartOfQuestion, HasPartStatement>
	{
		#region Properties

		public IConcept Parent
		{ get; }

		public IConcept Child
		{ get; }

		#endregion

		public IsPartOfQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsPartOfQuestion> context, ICollection<HasPartStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(statements.Any() ? new Func<String>(() => context.Language.Answers.IsPartOfTrue) : () => context.Language.Answers.IsPartOfFalse, new Dictionary<String, INamed>
				{
					{ Strings.ParamParent, Parent },
					{ Strings.ParamChild, Child },
				}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(HasPartStatement statement)
		{
			return statement.Whole == Parent && statement.Part == Child;
		}

		protected override bool NeedToCheckTransitives(ICollection<HasPartStatement> statements)
		{
			return false;
		}

		protected override IAnswer ProcessChildAnswers(IQuestionProcessingContext<IsPartOfQuestion> context, ICollection<HasPartStatement> statements, ICollection<ChildAnswer> childAnswers)
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
