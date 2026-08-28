using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks what value a concept's sign has.</summary>
	public class SignValueQuestion : Question
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		/// <summary>The sign concept.</summary>
		public IConcept Sign
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept">The concept in question.</param>
		/// <param name="sign">The sign concept.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public SignValueQuestion(IConcept concept, IConcept sign, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Sign = sign.EnsureNotNull(nameof(sign));
		}

		/// <summary>Derives the answer from the network's statements.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>The answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<SignValueQuestion, SignValueStatement>()
				.WithTransitives(
					statements => Task.FromResult(statements.Count == 0),
					question => question.Concept,
					newSubject => new SignValueQuestion(newSubject, Sign))
				.Where(s => s.Concept == Concept && s.Sign == Sign)
				.SelectFirstConceptAsync(
					statement => statement.Value,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.SignValue,
					statement => new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, Concept },
						{ Strings.ParamSign, statement.Sign },
						{ Strings.ParamValue, statement.Value },
						{ Strings.ParamDefined, statement.Concept },
					}).ConfigureAwait(false);
		}
	}
}
