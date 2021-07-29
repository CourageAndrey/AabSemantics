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
	public sealed class IsQuestion : Question<IsQuestion, IsStatement>
	{
		#region Properties

		public IConcept Child
		{ get; }

		public IConcept Parent
		{ get; }

		#endregion

		public IsQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsQuestion> context, ICollection<IsStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			if (!NeedToCheckTransitives(statements))
			{
				Boolean yes = statements.Any();
				return new BooleanAnswer(
					yes,
					new FormattedText(
						yes ? new Func<String>(() => context.Language.Answers.IsTrue) : () => context.Language.Answers.IsFalse,
						new Dictionary<String, INamed>
						{
							{ Strings.ParamParent, Child },
							{ Strings.ParamChild, Parent },
						}),
					new Explanation(statements));
			}
			else
			{
				return ProcessChildAnswers(context, statements, childAnswers);
			}
		}

		protected override Boolean DoesStatementMatch(IsStatement statement)
		{
			return statement.Parent == Parent && statement.Child == Child;
		}

		protected override Boolean NeedToCheckTransitives(ICollection<IsStatement> statements)
		{
			return statements.Count == 0;
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<IsQuestion> context)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<IsQuestion>>().Select(questionContext => questionContext.Question.Child));

			var question = context.Question;
			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Child);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new IsQuestion(parent, question.Parent), new IStatement[] { transitiveStatement });
				}
			}
		}

		protected override IAnswer ProcessChildAnswers(IQuestionProcessingContext<IsQuestion> questionProcessingContext, ICollection<IsStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			var resultStatements = new List<IsStatement>(statements);
			var additionalStatements = new List<IStatement>();

			foreach (var childAnswer in childAnswers)
			{
				if (((BooleanAnswer) childAnswer.Answer).Result)
				{
					var answerStatements = childAnswer.Answer.Explanation.Statements.OfType<IsStatement>().ToList();
					resultStatements.AddRange(answerStatements);
					additionalStatements.AddRange(childAnswer.Answer.Explanation.Statements.Except(answerStatements));
					additionalStatements.AddRange(childAnswer.TransitiveStatements);
				}
			}

			var result = CreateAnswer(questionProcessingContext, resultStatements, new ChildAnswer[0]);
			result.Explanation.Expand(additionalStatements);
			return result;
		}
	}
}
