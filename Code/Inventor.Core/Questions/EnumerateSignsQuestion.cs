using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class EnumerateSignsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public EnumerateSignsQuestion(IConcept concept, Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
			Recursive = recursive;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateSignsQuestion, HasSignStatement>(s => s.Concept == Concept)
				.WithTransitives(s => Recursive, GetNestedQuestions)
				.AggregateTransitivesToStatements()
				.SelectAllConcepts(
					statement => statement.Sign,
					question => question.Concept,
					Strings.ParamConcept,
					language => language.Answers.ConceptSigns + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse))
				.AppendAdditionalTransitives()
				.Answer;
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<EnumerateSignsQuestion> context)
		{
			if (!Recursive) yield break;

			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<EnumerateSignsQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var question = context.Question;
			var transitiveStatements = context.SemanticNetwork.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new EnumerateSignsQuestion(parent, true), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
