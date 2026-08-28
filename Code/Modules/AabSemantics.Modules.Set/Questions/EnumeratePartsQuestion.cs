using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks which concepts a concept consists of.</summary>
	public class EnumeratePartsQuestion : Question
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
		public EnumeratePartsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
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
				.From<EnumeratePartsQuestion, HasPartStatement>()
				.Where(s => s.Whole == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Part,
					question => question.Concept,
					AabSemantics.Localization.Strings.ParamParent,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.EnumerateParts).ConfigureAwait(false);
		}
	}
}
