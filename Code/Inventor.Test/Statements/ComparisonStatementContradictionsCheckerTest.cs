using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Statements;

namespace Inventor.Test.Statements
{
	[TestFixture]
	public class ComparisonStatementContradictionsCheckerTest
	{
		private const int _testChainLength = 5;

		[Test]
		public void SingleStatementCanNotContradict()
		{
			// arrange
			var value1 = createValueConcept(1);
			var value2 = createValueConcept(2);

			// act
			foreach (var sign in ComparisonSigns.All)
			{
				var statements = new[] { new ComparisonStatement(null, value1, value2, sign) };

				var contradictions = statements.CheckForContradictions();

				// assert
				Assert.AreEqual(0, contradictions.Count);
			}
		}

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
			foreach (var sign in ComparisonSigns.All)
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
			foreach (var sign in ComparisonSigns.All)
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(null, concept1, concept2, sign),
					new ComparisonStatement(null, concept1, concept2, sign),
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
			foreach (var contradictedPair in ComparisonSigns.Contradictions)
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(null, concept1, concept2, contradictedPair.Item1),
					new ComparisonStatement(null, concept1, concept2, contradictedPair.Item2),
				};

				var contradictions = statements.CheckForContradictions();

				// assert
				var contradiction = contradictions.First(c => c.Value1 != c.Value2);
				Assert.IsTrue(contradiction.Value1 == concept1 || contradiction.Value1 == concept2);
				Assert.IsTrue(contradiction.Value2 == concept1 || contradiction.Value2 == concept2);
				Assert.LessOrEqual(2, contradiction.Signs.Count);
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
					ComparisonSigns.IsGreaterThan,
					ComparisonSigns.IsLessThan,
				})
				{
					var statements = createSimpleLoop(concepts.Take(chainLength).ToList(), sign);

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
				ComparisonSigns.IsGreaterThan,
				ComparisonSigns.IsLessThan,
				ComparisonSigns.IsNotEqualTo,
			})
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(null, a, b, ComparisonSigns.IsEqualTo),
					new ComparisonStatement(null, a, c, ComparisonSigns.IsEqualTo),
					new ComparisonStatement(null, b, c, sign),
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
				ComparisonSigns.IsGreaterThan,
				ComparisonSigns.IsLessThan,
				ComparisonSigns.IsNotEqualTo,
			})
			{
				var statements = new List<ComparisonStatement>
				{
					new ComparisonStatement(null, value, value, sign),
				};

				var contradictions = statements.CheckForContradictions();

				// assert
				var contradiction = contradictions.Single();
				Assert.AreSame(value, contradiction.Value1);
				Assert.AreSame(value, contradiction.Value2);
				Assert.IsTrue(contradiction.Signs.Contains(ComparisonSigns.IsEqualTo));
				Assert.Less(1, contradiction.Signs.Count);
			}
		}

		private static IConcept createValueConcept(int number)
		{
			var concept = number.CreateConcept();
			concept.WithAttribute(IsValueAttribute.Value);
			return concept;
		}

		private static List<ComparisonStatement> createSimpleChain(List<IConcept> concepts, IConcept sign)
		{
			var statements = new List<ComparisonStatement>();
			for (int i = 0; i < concepts.Count - 1; i++)
			{
				statements.Add(new ComparisonStatement(null, concepts[i],
					concepts[i + 1],
					sign));
			}
			return statements;
		}

		private static List<ComparisonStatement> createSimpleLoop(List<IConcept> concepts, IConcept sign)
		{
			var statements = createSimpleChain(concepts, sign);
			statements.Add(new ComparisonStatement(null, statements.Last().RightValue,
				statements.First().LeftValue,
				sign));
			return statements;
		}
	}
}
