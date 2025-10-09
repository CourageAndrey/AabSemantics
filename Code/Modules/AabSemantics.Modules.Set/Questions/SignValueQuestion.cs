using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public class SignValueQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		#endregion

		public SignValueQuestion(IConcept concept, IConcept sign, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Sign = sign.EnsureNotNull(nameof(sign));
		}

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
					});
		}
	}
}
