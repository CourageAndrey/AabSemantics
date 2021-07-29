﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateSignsQuestion : Question
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
				.ProcessTransitives(s => Recursive, GetNestedQuestions)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateSignsQuestion> context, ICollection<HasSignStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			var allSigns = new HashSet<IConcept>(statements.Select(hs => hs.Sign));
			var allStatements = new HashSet<IStatement>(statements);

			foreach (var answer in childAnswers)
			{
				var conceptsAnswer = answer.Answer as ConceptsAnswer;
				if (conceptsAnswer != null)
				{
					foreach (var sign in conceptsAnswer.Result)
					{
						allSigns.Add(sign);
					}
					foreach (var statement in conceptsAnswer.Explanation.Statements)
					{
						allStatements.Add(statement);
					}
					foreach (var statement in answer.TransitiveStatements)
					{
						allStatements.Add(statement);
					}
				}
			}

			return allSigns.Count > 0
				? formatAnswer(context, allSigns, allStatements)
				: Answer.CreateUnknown(context.Language);
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<EnumerateSignsQuestion> context)
		{
			if (!Recursive) yield break;

			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<EnumerateSignsQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var question = context.Question;
			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new EnumerateSignsQuestion(parent, true), new IStatement[] { transitiveStatement });
				}
			}
		}

		private IAnswer formatAnswer(IQuestionProcessingContext<EnumerateSignsQuestion> context, ICollection<IConcept> signs, ICollection<IStatement> statements)
		{
			String format;
			var parameters = signs.Enumerate(out format);
			parameters[Strings.ParamConcept] = Concept;
			return new ConceptsAnswer(
				signs,
				new FormattedText(
					() => String.Format(context.Language.Answers.ConceptSigns, Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse, format),
					parameters),
				new Explanation(statements));
		}
	}
}
