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

		protected override void WriteOneLine(ITextContainer text, IConcept sign, IConcept value1, IConcept value2)
		{
			var formatString = value1 != null && value2 != null
				? new Func<ILanguage, String>(language => language.Answers.CompareConceptsDifference)
				: (value1 != null
					? new Func<ILanguage, String>(language => language.Answers.CompareConceptsFirstNotSet)
					: language => language.Answers.CompareConceptsSecondNotSet);

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
			text.Append(language => language.Answers.CompareConceptsNoDifference);
		}
	}
}
