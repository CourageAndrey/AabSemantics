﻿using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;

namespace AabSemantics.Modules.Set.Questions
{
	public class GetCommonQuestion : CompareConceptPropertiesQuestion
	{
		public GetCommonQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(concept1, concept2, preconditions)
		{ }

		protected override System.Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2)
		{
			return value1 == value2;
		}

		protected override void WriteOneLine(ITextContainer text, IConcept sign, IConcept value1, IConcept value2)
		{
			var formatString = value1 != null && value2 != null
				? new Func<ILanguage, String>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsCommon)
				: language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsCommonNotSet;

			var parameters = new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamSign, sign },
			};
			if (value1 != null)
			{
				parameters[Strings.ParamValue] = value1;
			}

			text.Append(formatString, parameters);
		}

		protected override void WriteNotEmptyResultWithoutData(ITextContainer text)
		{
			text.Append(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsNoCommon);
		}

		protected override void FormatParentsDiff(
			ITextContainer text,
			ICollection<IConcept> parents,
			ICollection<IConcept> parents1,
			ICollection<IConcept> parents2)
		{
			if (parents1.Count == parents.Count && parents2.Count == parents.Count)
			{
				text.Append(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsSameHierarchy);
			}
		}
	}
}
