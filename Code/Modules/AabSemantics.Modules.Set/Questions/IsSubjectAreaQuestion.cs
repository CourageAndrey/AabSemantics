using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks whether a concept belongs to a subject area.</summary>
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public class IsSubjectAreaQuestion : Question
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		/// <summary>The subject area concept.</summary>
		public IConcept Area
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept">The concept in question.</param>
		/// <param name="area">The subject area concept.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public IsSubjectAreaQuestion(IConcept concept, IConcept area, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Area = area.EnsureNotNull(nameof(area));
		}

		/// <summary>Derives the answer from the network's statements.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>The answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<IsSubjectAreaQuestion, GroupStatement>()
				.Where(s => s.Area == Area && s.Concept == Concept)
				.SelectBooleanAsync(
					statements => statements.Any(),
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.IsSubjectAreaTrue,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.IsSubjectAreaFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamArea, Area },
						{ AabSemantics.Localization.Strings.ParamConcept, Concept },
					}).ConfigureAwait(false);
		}
	}
}
