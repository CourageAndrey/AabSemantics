using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Modules.Set.Localization;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks which properties two concepts differ in.</summary>
	public class GetDifferencesQuestion : CompareConceptPropertiesQuestion
	{
		/// <summary>Creates the question.</summary>
		/// <param name="concept1">First compared concept.</param>
		/// <param name="concept2">Second compared concept.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public GetDifferencesQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(concept1, concept2, preconditions)
		{ }

		/// <inheritdoc/>
		/// <remarks>Keeps only pairs where the two concepts define different values.</remarks>
		protected override System.Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2)
		{
			return value1 != value2;
		}

		/// <inheritdoc/>
		/// <remarks>Appends a line naming a property and the two differing values.</remarks>
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

		/// <inheritdoc/>
		/// <remarks>Appends the "no differences" caption.</remarks>
		protected override void WriteNotEmptyResultWithoutData(ITextContainer text)
		{
			text.Append(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsNoDifference);
		}

		/// <inheritdoc/>
		/// <remarks>Appends how the two hierarchies diverge.</remarks>
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
