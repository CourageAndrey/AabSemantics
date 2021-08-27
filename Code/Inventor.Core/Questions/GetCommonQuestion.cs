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

		protected override void WriteOneLine(Text.TextContainer text, IConcept sign, IConcept value1, IConcept value2)
		{
			var formatString = value1 != null && value2 != null
				? new Func<ILanguage, String>(language => language.Answers.CompareConceptsCommon)
				: language => language.Answers.CompareConceptsCommonNotSet;

			var parameters = new Dictionary<String, INamed>
			{
				{ Strings.ParamSign, sign },
			};
			if (value1 != null)
			{
				parameters[Strings.ParamConcept1] = value1;
			}

			text.Add(formatString, parameters);
		}

		protected override void WriteNotEmptyResultWithoutData(Text.TextContainer text)
		{
			text.Add(language => language.Answers.CompareConceptsNoCommon, new Dictionary<string, INamed>());
		}
	}
}
