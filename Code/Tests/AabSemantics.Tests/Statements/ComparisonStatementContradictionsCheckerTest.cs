using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class ComparisonStatementContradictionsCheckerTest
	{
		private const int _testChainLength = 5;

		[Test]
		public void GivenSingleStatement_WhenCheckContradictions_ThenNoContradictions()
		{
			// arrange
			var value1 = CreateValueConcept(1);
			var value2 = CreateValueConcept(2);

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
		public void GivenSingleSignChain_WhenCheckContradictions_ThenNoContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(CreateValueConcept(i));
			}

			// act
			foreach (var sign in ComparisonSigns.All)
			{
				for (int revertCount = 0; revertCount < _testChainLength; revertCount++)
				{
					var statements = CreateSimpleChain(concepts, sign);
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
		public void GivenDuplicatingStatements_WhenCheckContradictions_ThenNoContradictions()
		{
			// arrange
			var concept1 = CreateValueConcept(1);
			var concept2 = CreateValueConcept(2);

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
		public void GivenTwoContradictedStatements_WhenCheckContradictions_ThenFindContradictions()
		{
			// arrange
			var concept1 = CreateValueConcept(1);
			var concept2 = CreateValueConcept(2);

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
		public void GivenSingleSignComparisonLoop_WhenCheckContradictions_ThenFindContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(CreateValueConcept(i));
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
					var statements = CreateSimpleLoop(concepts.Take(chainLength).ToList(), sign);

					var contradictions = statements.CheckForContradictions();

					// assert
					Assert.AreNotEqual(0, contradictions.Count);
				}
			}
		}

		[Test]
		public void GivenTwoValuesEqualToThirdButDontEqualEachOther_WhenCheckContradictions_ThenFindContradictions()
		{
			// arrange
			var a = CreateValueConcept(1);
			var b = CreateValueConcept(1);
			var c = CreateValueConcept(1);

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
		public void GivenValueComparedToItself_WhenCheckContradictions_ThenFindContradictions()
		{
			// arrange
			var value = CreateValueConcept(0);

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

		private static IConcept CreateValueConcept(int number)
		{
			var concept = number.CreateConcept();
			concept.WithAttribute(IsValueAttribute.Value);
			return concept;
		}

		private static List<ComparisonStatement> CreateSimpleChain(List<IConcept> concepts, IConcept sign)
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

		private static List<ComparisonStatement> CreateSimpleLoop(List<IConcept> concepts, IConcept sign)
		{
			var statements = CreateSimpleChain(concepts, sign);
			statements.Add(new ComparisonStatement(null, statements.Last().RightValue,
				statements.First().LeftValue,
				sign));
			return statements;
		}
	}
}
