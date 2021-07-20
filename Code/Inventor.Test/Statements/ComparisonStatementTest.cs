﻿using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Test.Statements
{
	public class ComparisonStatementTest
	{
		private const int _testChainLength = 5;

		[Test]
		public void SingleSignChainsHaveNoContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(createValueConcept(i));
			}

			// act
			foreach (var sign in SystemConcepts.ComparisonSigns)
			{
				for (int revertCount = 0; revertCount < _testChainLength; revertCount++)
				{
					var statements = createSimpleChain(concepts, sign);
					for (int r = 0; r < revertCount; r++)
					{
						statements[r] = statements[r].SwapOperands();
					}

					var contradictions = statements.CheckForContradictions();

					// assert
					Assert.AreEqual(0, contradictions.Count);
				}
			}
		}

		[Test]
		public void DuplicatedStatementsHaveNoContradictions()
		{
			// arrange
			var concept1 = createValueConcept(1);
			var concept2 = createValueConcept(2);

			// act
			foreach (var sign in SystemConcepts.ComparisonSigns)
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(concept1, concept2, sign),
					new ComparisonStatement(concept1, concept2, sign),
				};

				var contradictions = statements.CheckForContradictions();

				// assert
				Assert.AreEqual(0, contradictions.Count);
			}
		}

		[Test]
		public void TwoCondradictedStatementsHaveContradictions()
		{
			// arrange
			var concept1 = createValueConcept(1);
			var concept2 = createValueConcept(2);

			// act
			foreach (var contradictedPair in ComparisonStatement.Contradictions)
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(concept1, concept2, contradictedPair.Item1),
					new ComparisonStatement(concept1, concept2, contradictedPair.Item2),
				};

				var contradictions = statements.CheckForContradictions();

				// assert
				var contradiction = contradictions.Single();
				Assert.AreNotSame(contradiction.Value1, contradiction.Value2);
				Assert.IsTrue(contradiction.Value1 == concept1 || contradiction.Value1 == concept2);
				Assert.IsTrue(contradiction.Value2 == concept1 || contradiction.Value2 == concept2);
				Assert.AreEqual(2, contradiction.Signs.Count);
				Assert.IsTrue(contradiction.Signs.Contains(contradictedPair.Item1));
				Assert.IsTrue(contradiction.Signs.Contains(contradictedPair.Item2));
			}
		}

		[Test]
		public void SingleSignLoopsHaveContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(createValueConcept(i));
			}

			// act
			for (int chainLength = 3; chainLength <= Math.Max(3, _testChainLength); chainLength++)
			{
				foreach (var sign in new[]
				{
					SystemConcepts.IsGreaterThan,
					SystemConcepts.IsLessThan,
				})
				{
					var statements = createSimpleChain(concepts.Take(chainLength).ToList(), sign);
					statements.Add(new ComparisonStatement(
						statements.Last().RightValue,
						statements.First().LeftValue,
						sign));

					var contradictions = statements.CheckForContradictions();

					// assert
					Assert.AreNotEqual(0, contradictions.Count);
				}
			}
		}

		[Test]
		public void TwoValuesEqualToThirdButDontEqualEachOtherHaveContradictions()
		{
			// arrange
			var a = createValueConcept(1);
			var b = createValueConcept(1);
			var c = createValueConcept(1);

			// act
			foreach (var sign in new[]
			{
				SystemConcepts.IsGreaterThan,
				SystemConcepts.IsLessThan,
				SystemConcepts.IsNotEqualTo,
			})
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(a, b, SystemConcepts.IsEqualTo),
					new ComparisonStatement(a, c, SystemConcepts.IsEqualTo),
					new ComparisonStatement(b, c, sign),
				};

				var contradictions = statements.CheckForContradictions();

				// assert
				Assert.AreNotEqual(0, contradictions.Count);
			}
		}

		[Test]
		public void ValueHasToBeEqualToItself()
		{
			// arrange
			var value = createValueConcept(0);

			// act
			foreach (var sign in new[]
			{
				SystemConcepts.IsGreaterThan,
				SystemConcepts.IsLessThan,
				SystemConcepts.IsNotEqualTo,
			})
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(value, value, sign),
				};

				var contradictions = statements.CheckForContradictions();

				// assert
				var contradiction = contradictions.Single();
				Assert.AreSame(value, contradiction.Value1);
				Assert.AreSame(value, contradiction.Value2);
				Assert.IsTrue(contradiction.Signs.Contains(SystemConcepts.IsEqualTo));
				Assert.Less(1, contradiction.Signs.Count);
			}
		}

		private IConcept createValueConcept(int number)
		{
			var concept = new Concept(
				new LocalizedStringVariable(new[] { new KeyValuePair<string, string>(Language.Default.Culture, number.ToString()) }),
				new LocalizedStringVariable(new[] { new KeyValuePair<string, string>(Language.Default.Culture, number.ToString()) }));
			concept.Attributes.Add(IsValueAttribute.Value);
			return concept;
		}

		private List<ComparisonStatement> createSimpleChain(List<IConcept> concepts, IConcept sign)
		{
			var statements = new List<ComparisonStatement>();
			for (int i = 0; i < concepts.Count - 1; i++)
			{
				statements.Add(new ComparisonStatement(
					concepts[i],
					concepts[i + 1],
					sign));
			}
			return statements;
		}
	}
}