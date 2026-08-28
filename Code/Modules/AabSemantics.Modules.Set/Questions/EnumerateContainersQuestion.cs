using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks which concepts a concept is part of.</summary>
	public class EnumerateContainersQuestion : Question
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept">The concept in question.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public EnumerateContainersQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		/// <summary>Derives the answer from the network's statements.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>The answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<EnumerateContainersQuestion, HasPartStatement>()
				.Where(s => s.Part == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Whole,
					question => question.Concept,
					AabSemantics.Localization.Strings.ParamChild,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.EnumerateContainers).ConfigureAwait(false);
		}
	}
}
