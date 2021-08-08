using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public class IsQuestion : Question
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

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsQuestion, IsStatement>(s => s.Parent == Parent && s.Child == Child)
				.WithTransitives(s => s.Count == 0, GetNestedQuestions)
				.SelectBooleanIncludingChildren(
					statements => statements.Count > 0,
					language => language.Answers.IsTrue,
					language => language.Answers.IsFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamParent, Child },
						{ Strings.ParamChild, Parent },
					})
				.Answer;
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<IsQuestion> context)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<IsQuestion>>().Select(questionContext => questionContext.Question.Child));

			var question = context.Question;
			var transitiveStatements = context.SemanticNetwork.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Child);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new IsQuestion(parent, question.Parent), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
