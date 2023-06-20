using System;
using System.Collections.Generic;

using Inventor.Semantics.Questions;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Set.Questions
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

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<SignValueQuestion, SignValueStatement>()
				.WithTransitives(
					statements => statements.Count == 0,
					question => question.Concept,
					newSubject => new SignValueQuestion(newSubject, Sign))
				.Where(s => s.Concept == Concept && s.Sign == Sign)
				.SelectFirstConcept(
					statement => statement.Value,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.SignValue,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Semantics.Localization.Strings.ParamConcept, Concept },
						{ Strings.ParamSign, statement.Sign },
						{ Strings.ParamValue, statement.Value },
						{ Strings.ParamDefined, statement.Concept },
					});
		}
	}
}
