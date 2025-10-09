using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public class EnumerateSignsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public System.Boolean Recursive
		{ get; }

		#endregion

		public EnumerateSignsQuestion(IConcept concept, System.Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Recursive = recursive;
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<EnumerateSignsQuestion, HasSignStatement>()
				.WithTransitives(
					statements => Task.FromResult(Recursive),
					question => question.Concept,
					newSubject => new EnumerateSignsQuestion(newSubject, true),
					needToAggregateTransitivesToStatements: true)
				.Where(s => s.Concept == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Sign,
					question => question.Concept,
					AabSemantics.Localization.Strings.ParamConcept,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.ConceptSigns + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ": ");
		}
	}
}
