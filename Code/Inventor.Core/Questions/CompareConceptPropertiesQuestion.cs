using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Localization.Modules;
using Inventor.Core.Statements;
using Inventor.Core.Text.Containers;
using Inventor.Core.Text.Primitives;

namespace Inventor.Core.Questions
{
	public abstract class CompareConceptPropertiesQuestion : Question
	{
		#region Properties

		public IConcept Concept1
		{ get; }

		public IConcept Concept2
		{ get; }

		#endregion

		protected CompareConceptPropertiesQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept1 == null) throw new ArgumentNullException(nameof(concept1));
			if (concept2 == null) throw new ArgumentNullException(nameof(concept2));
			if (concept1 == concept2) throw new ArgumentException("Attempt to compare concept with itself has no sense.");

			Concept1 = concept1;
			Concept2 = concept2;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
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
						language => language.GetExtension<ILanguageSetModule>().Questions.Answers.CanNotCompareConcepts,
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
				var valueStatement1 = SignValueStatement.GetSignValue(allStatements, Concept1, sign);
				var valueStatement2 = SignValueStatement.GetSignValue(allStatements, Concept2, sign);
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
				formatAnswer(parents, resultSignValues),
				new Explanation(explanation));
		}

		protected abstract Boolean NeedToTakeIntoAccount(IConcept value1, IConcept value2);

		protected abstract void WriteNotEmptyResultWithoutData(ITextContainer text);

		protected abstract void WriteOneLine(ITextContainer text, IConcept sign, IConcept value1, IConcept value2);

		private IText formatAnswer(
			ICollection<IConcept> parents,
			IDictionary<IConcept, Tuple<IConcept, IConcept>> signValueStatements)
		{
			var result = new UnstructuredContainer(new FormattedText(
				language => language.GetExtension<ILanguageSetModule>().Questions.Answers.CompareConceptsResult,
				new Dictionary<String, IKnowledge>
				{
					{ Strings.ParamConcept1, Concept1 },
					{ Strings.ParamConcept2, Concept2 },
				})).AppendBulletsList(parents.Enumerate());

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
