using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Statements;

namespace AabSemantics.Modules.Mathematics.Tests.Statements
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
				Assert.That(contradictions.Count, Is.EqualTo(0));
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
					Assert.That(contradictions.Count, Is.EqualTo(0));
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
				Assert.That(contradictions.Count, Is.EqualTo(0));
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
				Assert.That(contradiction.Value1 == concept1 || contradiction.Value1 == concept2, Is.True);
				Assert.That(contradiction.Value2 == concept1 || contradiction.Value2 == concept2, Is.True);
				Assert.That(contradiction.Signs.Count, Is.GreaterThanOrEqualTo(2));
				Assert.That(contradiction.Signs.Contains(contradictedPair.Item1), Is.True);
				Assert.That(contradiction.Signs.Contains(contradictedPair.Item2), Is.True);
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
					Assert.That(contradictions.Count, Is.Not.EqualTo(0));
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
				Assert.That(contradictions.Count, Is.Not.EqualTo(0));
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
				Assert.That(contradiction.Value1, Is.SameAs(value));
				Assert.That(contradiction.Value2, Is.SameAs(value));
				Assert.That(contradiction.Signs.Contains(ComparisonSigns.IsEqualTo), Is.True);
				Assert.That(contradiction.Signs.Count, Is.GreaterThan(1));
			}
		}

		private static IConcept CreateValueConcept(int number)
		{
			var concept = number.CreateConceptByObject();
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
