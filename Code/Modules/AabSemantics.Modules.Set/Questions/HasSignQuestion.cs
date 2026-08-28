using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks whether a concept has a given sign.</summary>
	public class HasSignQuestion : Question
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		/// <summary>The sign concept.</summary>
		public IConcept Sign
		{ get; }

		/// <summary>Whether inherited knowledge is taken into account.</summary>
		public System.Boolean Recursive
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept">The concept in question.</param>
		/// <param name="sign">The sign concept.</param>
		/// <param name="recursive">Whether inherited knowledge is taken into account.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public HasSignQuestion(IConcept concept, IConcept sign, System.Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Sign = sign.EnsureNotNull(nameof(sign));
			Recursive = recursive;
		}

		/// <summary>Derives the answer from the network's statements.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>The answer.</returns>
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
					}).ConfigureAwait(false);
		}
	}
}
