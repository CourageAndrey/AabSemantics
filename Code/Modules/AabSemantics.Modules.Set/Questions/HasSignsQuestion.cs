using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public class HasSignsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public System.Boolean Recursive
		{ get; }

		#endregion

		public HasSignsQuestion(IConcept concept, System.Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Recursive = recursive;
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<HasSignsQuestion, HasSignStatement>()
				.WithTransitives(
					statements => Task.FromResult(Recursive),
					question => question.Concept,
					newSubject => new HasSignsQuestion(newSubject, true))
				.Where(s => s.Concept == Concept)
				.SelectBooleanIncludingChildrenAsync(
					statements => statements.Count > 0,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.HasSignsTrue + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.HasSignsFalse + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, Concept },
					});
		}
	}
}
