using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks which signs a concept has.</summary>
	public class EnumerateSignsQuestion : Question
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		/// <summary>Whether inherited knowledge is taken into account.</summary>
		public System.Boolean Recursive
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept">The concept in question.</param>
		/// <param name="recursive">Whether inherited knowledge is taken into account.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public EnumerateSignsQuestion(IConcept concept, System.Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Recursive = recursive;
		}

		/// <summary>Derives the answer from the network's statements.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>The answer.</returns>
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
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.ConceptSigns + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ": ").ConfigureAwait(false);
		}
	}
}
