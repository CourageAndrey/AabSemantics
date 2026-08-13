using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Text.Containers;
using AabSemantics.Text.Primitives;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>
	/// Base of the two concept-comparison questions. It gathers both concepts' sign values and their
	/// hierarchies, then lets a subclass decide which pairs to report and how to word them.
	/// </summary>
	public abstract class CompareConceptPropertiesQuestion : Question
	{
		#region Properties

		/// <summary>The first compared concept.</summary>
		public IConcept Concept1
		{ get; }

		/// <summary>The second compared concept.</summary>
		public IConcept Concept2
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="concept1">First compared concept.</param>
		/// <param name="concept2">Second compared concept.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">Either concept is <c>null</c>.</exception>
		protected CompareConceptPropertiesQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept1 == concept2) throw new ArgumentException("Attempt to compare concept with itself has no sense.");

			Concept1 = concept1.EnsureNotNull(nameof(concept1));
			Concept2 = concept2.EnsureNotNull(nameof(concept2));
		}

		/// <summary>Derives the answer from the network's statements.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>The answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			var allStatements = context.SemanticNetwork.Statements.Enumerate(context.ActiveContexts).ToList();

			// get hierarchies
			var isStatements1 = new List<IsStatement>();
			var isStatements2 = new List<IsStatement>();
			var parents1 = allStatements.GetParentsAllLevels(Concept1, isStatements1);
			var parents2 = allStatements.GetParentsAllLevels(Concept2, isStatements2);

			// intersect parents
			var isStatements = new List<IsStatement>();
			var parents = intersect(parents1, parents2, isStatements1, isStatements2, isStatements);
			if (parents.Count == 0)
			{
				return new Answers.Answer(
					new FormattedText(
						language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CanNotCompareConcepts,
						new Dictionary<String, IKnowledge>
						{
							{ Strings.ParamConcept1, Concept1 },
							{ Strings.ParamConcept2, Concept2 },
						}),
					new Explanation(Array.Empty<IStatement>()),
					true);
			}

			// get signs
			var allSignStatements = allStatements.OfType<HasSignStatement>().ToList();
			var signStatements1 = getAllSigns(allSignStatements, parents1);
			var signStatements2 = getAllSigns(allSignStatements, parents2);

			var signs = new HashSet<IConcept>(signStatements1.Select(s => s.Sign).Intersect(signStatements2.Select(s => s.Sign)));

			var signStatements = new List<HasSignStatement>();
			signStatements.AddRange(signStatements1.Where(s => signs.Contains(s.Sign)));
			signStatements.AddRange(signStatements2.Where(s => signs.Contains(s.Sign) && !signStatements.Contains(s)));

			// compare sign values
			var resultSignValues = new Dictionary<IConcept, Tuple<IConcept, IConcept>>();
			var signValueStatements = new List<SignValueStatement>();
			foreach (var sign in signs)
			{
				var valueStatement1 = await SignValueStatement.GetSignValueAsync(allStatements, Concept1, sign);
				var valueStatement2 = await SignValueStatement.GetSignValueAsync(allStatements, Concept2, sign);
				var value1 = valueStatement1?.Value;
				var value2 = valueStatement2?.Value;

				if (NeedToTakeIntoAccount(value1, value2))
				{
					resultSignValues[sign] = new Tuple<IConcept, IConcept>(value1, value2);
					signValueStatements.Add(valueStatement1);
					signValueStatements.Add(valueStatement2);
				}
			}

			// format final result
			var explanation = new List<IStatement>();
			explanation.AddRange(isStatements);
			explanation.AddRange(signStatements);
			explanation.AddRange(signValueStatements);

			return new Answers.ConceptsAnswer(
				resultSignValues.Keys,
				formatAnswer(parents, parents1, parents2, resultSignValues),
				new Explanation(explanation));
		}

		/// <summary>Decides whether a pair of sign values is worth reporting for this kind of question.</summary>
		/// <param name="value1">Value defined for the first concept; may be <c>null</c>.</param>
		/// <param name="value2">Value defined for the second concept; may be <c>null</c>.</param>
		/// <returns><c>true</c> when the pair should appear in the answer.</returns>
		protected abstract System.Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2);

		/// <summary>Appends the caption shown when a result exists but has no data rows.</summary>
		/// <param name="text">Text container to append to.</param>
		protected abstract void WriteNotEmptyResultWithoutData(ITextContainer text);

		/// <summary>Appends one comparison line to the answer text.</summary>
		/// <param name="text">Text container to append to.</param>
		/// <param name="sign">Sign being compared.</param>
		/// <param name="value1">Value of the first concept; may be <c>null</c>.</param>
		/// <param name="value2">Value of the second concept; may be <c>null</c>.</param>
		protected abstract void WriteOneLine(ITextContainer text, IConcept sign, IConcept value1, IConcept value2);

		/// <summary>Appends the part of the answer describing how the two concepts sit in the hierarchy.</summary>
		protected abstract void FormatParentsDiff(
			ITextContainer text,
			ICollection<IConcept> parents,
			ICollection<IConcept> parents1,
			ICollection<IConcept> parents2);

		private IText formatAnswer(
			ICollection<IConcept> parents,
			ICollection<IConcept> parents1,
			ICollection<IConcept> parents2,
			IDictionary<IConcept, Tuple<IConcept, IConcept>> signValueStatements)
		{
			var result = new UnstructuredContainer(new FormattedText(
				language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.CompareConceptsResult,
				new Dictionary<String, IKnowledge>
				{
					{ Strings.ParamConcept1, Concept1 },
					{ Strings.ParamConcept2, Concept2 },
				})).AppendBulletsList(parents.Enumerate());

			FormatParentsDiff(result, parents, parents1, parents2);

			if (signValueStatements.Count > 0)
			{
				foreach (var sign in signValueStatements)
				{
					WriteOneLine(result, sign.Key, sign.Value.Item1, sign.Value.Item2);
				}
			}
			else
			{
				WriteNotEmptyResultWithoutData(result);
			}

			return result;
		}

		private static ICollection<IConcept> intersect(
			IEnumerable<IConcept> parents1,
			IEnumerable<IConcept> parents2,
			IEnumerable<IsStatement> isStatements1,
			IEnumerable<IsStatement> isStatements2,
			List<IsStatement> isStatements)
		{
			var parents = new HashSet<IConcept>(parents1.Intersect(parents2));

			isStatements.AddRange(isStatements1.Where(i => parents.Contains(i.Ancestor)));
			isStatements.AddRange(isStatements2.Where(i => parents.Contains(i.Ancestor) && !isStatements.Contains(i)));

			return parents;
		}

		private static ICollection<HasSignStatement> getAllSigns(IEnumerable<HasSignStatement> hasSignStatements, ICollection<IConcept> concepts)
		{
			return hasSignStatements.Where(s => concepts.Contains(s.Concept)).ToList();
		}
	}
}
