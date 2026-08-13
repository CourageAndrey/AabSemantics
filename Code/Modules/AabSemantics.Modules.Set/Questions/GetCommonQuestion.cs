using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks which properties two concepts share.</summary>
	public class GetCommonQuestion : CompareConceptPropertiesQuestion
	{
		/// <summary>Creates the question.</summary>
		/// <param name="concept1">First compared concept.</param>
		/// <param name="concept2">Second compared concept.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public GetCommonQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(concept1, concept2, preconditions)
		{ }

		/// <inheritdoc/>
		/// <remarks>Keeps only pairs where both concepts define the same value.</remarks>
		protected override System.Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2)
		{
			return value1 == value2;
		}

		/// <inheritdoc/>
		/// <remarks>Appends a line naming a shared property and its value.</remarks>
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

		/// <inheritdoc/>
		/// <remarks>Appends the "nothing in common" caption.</remarks>
		protected override void WriteNotEmptyResultWithoutData(ITextContainer text)
		{
			text.Append(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsNoCommon);
		}

		/// <inheritdoc/>
		/// <remarks>Appends the shared part of the two hierarchies.</remarks>
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
