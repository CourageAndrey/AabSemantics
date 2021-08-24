﻿using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
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
					language => language.Answers.SignValue,
					statement => new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
						{ Strings.ParamSign, statement.Sign },
						{ Strings.ParamValue, statement.Value },
						{ Strings.ParamDefined, statement.Concept },
					})
				.Answer;
		}
	}
}
