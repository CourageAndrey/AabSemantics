using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public class HasSignQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		public System.Boolean Recursive
		{ get; }

		#endregion

		public HasSignQuestion(IConcept concept, IConcept sign, System.Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Sign = sign.EnsureNotNull(nameof(sign));
			Recursive = recursive;
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<HasSignQuestion, HasSignStatement>()
				.WithTransitives(
					statements => Task.FromResult(statements.Count == 0 && Recursive),
					question => question.Concept,
					newSubject => new HasSignQuestion(newSubject, Sign, true))
				.Where(s => s.Concept == Concept && s.Sign == Sign)
				.SelectBooleanIncludingChildrenAsync(
					statements => statements.Count > 0,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.HasSignTrue + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.HasSignFalse + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, Concept },
						{ Strings.ParamSign, Sign },
					});
		}
	}
}
