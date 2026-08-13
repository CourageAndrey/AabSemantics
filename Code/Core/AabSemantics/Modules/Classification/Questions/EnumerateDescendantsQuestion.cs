using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Questions
{
	/// <summary>Asks which concepts are a kind of a given concept.</summary>
	public class EnumerateDescendantsQuestion : Question
	{
		#region Properties

		/// <summary>The concept whose descendants are asked for.</summary>
		public IConcept Concept
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept">The concept whose descendants are asked for.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="concept"/> is <c>null</c>.</exception>
		public EnumerateDescendantsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		/// <summary>
		/// Lists the concept's direct descendants only — transitive lookup is not configured, so
		/// descendants of descendants are not included.
		/// </summary>
		/// <param name="context">Context to search.</param>
		/// <returns>A concept-list answer, or the "unknown" answer when the concept has no descendants.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<EnumerateDescendantsQuestion, IsStatement>()
				.Where(s => s.Ancestor == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Descendant,
					question => question.Concept,
					Strings.ParamParent,
					language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Answers.EnumerateDescendants);
		}
	}
}
