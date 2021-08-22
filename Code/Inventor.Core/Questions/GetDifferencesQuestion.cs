using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Questions
{
	public class GetDifferencesQuestion : CompareConceptPropertiesQuestion
	{
		public GetDifferencesQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(concept1, concept2, preconditions)
		{ }

		protected override Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2)
		{
			return value1 != value2;
		}

		protected override void WriteOneLine(FormattedText text, IConcept sign, IConcept value1, IConcept value2, ILanguage language)
		{
			String formatString = value1 != null && value2 != null
				? language.Answers.CompareConceptsDifference
				: (value1 != null ? language.Answers.CompareConceptsFirstNotSet : language.Answers.CompareConceptsSecondNotSet);

			var parameters = new Dictionary<String, INamed>
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

			text.Add(() => formatString, parameters);
		}

		protected override void WriteNotEmptyResultWithoutData(FormattedText text, ILanguage language)
		{
			text.Add(() => language.Answers.CompareConceptsNoDifference, new Dictionary<string, INamed>());
		}
	}
}
