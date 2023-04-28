using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Text.Containers;
using Inventor.Semantics.Text.Primitives;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;
using Inventor.Semantics.Utils;
using Inventor.Semantics.Contexts;

namespace Inventor.Semantics.Set.Questions
{
	public abstract class CompareConceptPropertiesQuestion : Question
	{
		#region Properties

		public IList<IConcept> Concepts
		{ get; }

		#endregion

		protected CompareConceptPropertiesQuestion(IList<IConcept> concepts, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concepts = concepts.EnsureNotNull(nameof(concepts)).Distinct().ToList();
			if (Concepts.Count < 2) throw new ArgumentException("It's necessary at least two different concepts to compare.");
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			var allStatements = context.SemanticNetwork.Statements.Enumerate(context.ActiveContexts).ToList();

			// get hierarchies
			var allIsStatements = new List<List<IsStatement>>();
			var allParents = new List<List<IConcept>>();
			for (int c = 0; c < Concepts.Count; c++)
			{
				var isStatements = new List<IsStatement>();
				allParents.Add(allStatements.GetParentsAllLevels(Concepts[c], isStatements));
				allIsStatements.Add(isStatements);
			}

			// intersect parents
			var commonIsStatements = new List<IsStatement>();
			var parents = intersect(allParents, allIsStatements, commonIsStatements);
			if (parents.Count == 0)
			{
				var format = new UnstructuredContainer(new FormattedText(
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.CanNotCompareConcepts, // !!!!!!!!!!!!!!!!!!
					new Dictionary<String, IKnowledge>())).AppendBulletsList(Concepts.Enumerate());

				return new Answers.Answer(
					format,
					new Explanation(Array.Empty<IStatement>()),
					true);
			}

			// get signs
			var allSignStatements = allStatements.OfType<HasSignStatement>().ToList();
			var signStatementsToCheck = new List<ICollection<HasSignStatement>>();
			for (int p = 0; p < allParents.Count; p++)
			{
				signStatementsToCheck.Add(getAllSigns(allSignStatements, allParents[p]));
			}

			var signsEnumeration = signStatementsToCheck[0].Select(s => s.Sign);
			for (int ss = 1; ss < signStatementsToCheck.Count; ss++)
			{
				signsEnumeration = signsEnumeration.Intersect(signStatementsToCheck[ss].Select(s => s.Sign));
			}
			var signs = new HashSet<IConcept>(signsEnumeration);

			var signStatements = new List<HasSignStatement>();

			foreach (var statements in signStatementsToCheck)
			{
				signStatements.AddRange(statements.Where(s => signs.Contains(s.Sign) && !signStatements.Contains(s)));
			}

			// compare sign values
			var resultSignValues = new Dictionary<IConcept, IList<IConcept>>();
			var signValueStatements = new List<SignValueStatement>();
			foreach (var sign in signs)
			{
				var valueStatements = new List<SignValueStatement>();
				var values = new List<IConcept>();
				foreach (var concept in Concepts)
				{
					var valueStatement = SignValueStatement.GetSignValue(allStatements, concept, sign);
					valueStatements.Add(valueStatement);

					var value = valueStatement?.Value;
					values.Add(value);
				}

				if (NeedToTakeIntoAccount(values))
				{
					resultSignValues[sign] = values;
					signValueStatements.AddRange(valueStatements);
				}
			}

			// format final result
			var explanation = new List<IStatement>();
			explanation.AddRange(commonIsStatements);
			explanation.AddRange(signStatements);
			explanation.AddRange(signValueStatements);

			return new Answers.ConceptsAnswer(
				resultSignValues.Keys,
				formatAnswer(parents, resultSignValues),
				new Explanation(explanation));
		}

		protected abstract Boolean NeedToTakeIntoAccount(IList<IConcept> values);

		protected abstract void WriteNotEmptyResultWithoutData(ITextContainer text);

		protected abstract void WriteOneLine(ITextContainer text, IConcept sign, IConcept value1, IConcept value2);

		private IText formatAnswer(
			ICollection<IConcept> parents,
			IDictionary<IConcept, IList<IConcept>> signValueStatements)
		{
			var result = new UnstructuredContainer(new FormattedText(
				language => language.GetExtension<ILanguageSetModule>().Questions.Answers.CompareConceptsResult,
				new Dictionary<String, IKnowledge>
				{
					{ Strings.ParamConcept1, Concepts[0] },// !!!!!!!!!!!!!!!!!!
					{ Strings.ParamConcept2, Concepts[1] },// !!!!!!!!!!!!!!!!!!
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
			List<List<IConcept>> allParents,
			List<List<IsStatement>> allIsStatements,
			List<IsStatement> commonIsStatements)
		{
			IEnumerable<IConcept> parentsEnumeration = allParents[0];
			for (int p = 1; p < allParents.Count; p++)
			{
				parentsEnumeration = parentsEnumeration.Intersect(allParents[p]);
			}
			var parents = new HashSet<IConcept>(parentsEnumeration);

			foreach (var isStatements in allIsStatements)
			{
				commonIsStatements.AddRange(isStatements.Where(i => parents.Contains(i.Ancestor) && !commonIsStatements.Contains(i)));
			}

			return parents;
		}

		private static ICollection<HasSignStatement> getAllSigns(IEnumerable<HasSignStatement> hasSignStatements, ICollection<IConcept> concepts)
		{
			return hasSignStatements.Where(s => concepts.Contains(s.Concept)).ToList();
		}
	}
}
