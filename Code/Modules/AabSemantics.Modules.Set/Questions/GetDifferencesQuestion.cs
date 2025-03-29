﻿using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Modules.Set.Localization;

namespace AabSemantics.Modules.Set.Questions
{
	public class GetDifferencesQuestion : CompareConceptPropertiesQuestion
	{
		public GetDifferencesQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(concept1, concept2, preconditions)
		{ }

		protected override System.Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2)
		{
			return value1 != value2;
		}

		protected override void WriteOneLine(ITextContainer text, IConcept sign, IConcept value1, IConcept value2)
		{
			var formatString = value1 != null && value2 != null
				? new Func<ILanguage, String>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsDifference)
				: (value1 != null
					? new Func<ILanguage, String>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsFirstNotSet)
					: language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsSecondNotSet);

			var parameters = new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamSign, sign },
			};
			if (value1 != null)
			{
				parameters[Strings.ParamConcept1] = value1;
			}
			if (value2 != null)
			{
				parameters[Strings.ParamConcept2] = value2;
			}

			text.Append(formatString, parameters);
		}

		protected override void WriteNotEmptyResultWithoutData(ITextContainer text)
		{
			text.Append(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsNoDifference);
		}

		protected override void FormatParentsDiff(
			ITextContainer text,
			ICollection<IConcept> parents,
			ICollection<IConcept> parents1,
			ICollection<IConcept> parents2)
		{
			var uniqueParents1 = parents1.Except(parents).ToList();
			var uniqueParents2 = parents2.Except(parents).ToList();

			if (uniqueParents1.Count > 0)
			{
				text.Append(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsDifferentHierarchyFirst)
					.AppendBulletsList(uniqueParents1.Enumerate());
			}

			if (uniqueParents2.Count > 0)
			{
				text.Append(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsDifferentHierarchySecond)
					.AppendBulletsList(uniqueParents2.Enumerate());
			}
		}
	}
}
