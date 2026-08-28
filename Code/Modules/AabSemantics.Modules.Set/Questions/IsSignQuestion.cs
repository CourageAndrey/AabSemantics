using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks whether a concept is used as a sign.</summary>
	public class IsSignQuestion : Question
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
		public IsSignQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
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
				.From<IsSignQuestion, HasSignStatement>()
				.Where(s => s.Sign == Concept)
				.SelectBooleanAsync(
					statement => Concept.HasAttribute<IsSignAttribute>(),
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.SignTrue,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.SignFalse,
					new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, Concept },
					}).ConfigureAwait(false);
		}
	}
}
