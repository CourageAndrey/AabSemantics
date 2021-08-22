using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Questions
{
	public class GetCommonQuestion : CompareConceptPropertiesQuestion
	{
		public GetCommonQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(concept1, concept2, preconditions)
		{ }

		protected override Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2)
		{
			return value1 == value2;
		}

		protected override void WriteOneLine(FormattedText text, IConcept sign, IConcept value1, IConcept value2, ILanguage language)
		{
			String formatString = value1 != null && value2 != null
				? language.Answers.CompareConceptsCommon
				: language.Answers.CompareConceptsCommonNotSet;

			var parameters = new Dictionary<String, INamed>
			{
				{ Strings.ParamSign, sign },
			};
			if (value1 != null)
			{
				parameters[Strings.ParamConcept1] = value1;
			}

			text.Add(() => formatString, parameters);
		}

		protected override void WriteNotEmptyResultWithoutData(FormattedText text, ILanguage language)
		{
			text.Add(() => language.Answers.CompareConceptsNoCommon, new Dictionary<string, INamed>());
		}
	}
}
