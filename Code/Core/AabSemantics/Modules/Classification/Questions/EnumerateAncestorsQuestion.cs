using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Questions
{
	/// <summary>Asks which concepts a given concept is a kind of.</summary>
	public class EnumerateAncestorsQuestion : Question
	{
		#region Properties

		/// <summary>The concept whose ancestors are asked for.</summary>
		public IConcept Concept
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept">The concept whose ancestors are asked for.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="concept"/> is <c>null</c>.</exception>
		public EnumerateAncestorsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		/// <summary>
		/// Lists the concept's direct ancestors only — transitive lookup is not configured, so
		/// ancestors of ancestors are not included.
		/// </summary>
		/// <param name="context">Context to search.</param>
		/// <returns>A concept-list answer, or the "unknown" answer when the concept has no ancestors.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<EnumerateAncestorsQuestion, IsStatement>()
				.Where(s => s.Descendant == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Ancestor,
					question => question.Concept,
					Strings.ParamChild,
					language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Answers.EnumerateAncestors);
		}
	}
}
