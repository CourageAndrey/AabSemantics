using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class ProcessesStatementContradictionsCheckerTest
	{
		private const int _testChainLength = 5;

		[Test]
		public void GivenSingleStatement_WhenCheckContradictions_ThenNoContradictions()
		{
			// arrange
			var processA = CreateProcessConcept("A");
			var processB = CreateProcessConcept("B");

			// act
			foreach (var sign in SequenceSigns.All)
			{
				var statements = new[] { new ProcessesStatement(null, processA, processB, sign) };

				var contradictions = statements.CheckForContradictions();

				// assert
				Assert.AreEqual(0, contradictions.Count);
			}
		}

		[Test]
		public void GivenValidSequenceCombination_WhenCheckContradictions_ThenNoContradictions()
		{
			// arrange
			var processA = CreateProcessConcept("A");
			var processI = CreateProcessConcept("I");
			var processB = CreateProcessConcept("B");

			// act
			foreach (var signPair in SequenceSigns.ValidSequenceCombinations)
			{
				var signAI = signPair.Key;
				foreach (var signIB in signPair.Value.Keys)
				{
					var statements = new[]
					{
						new ProcessesStatement(null, processA, processI, signAI),
						new ProcessesStatement(null, processI, processB, signIB),
					};

					var contradictions = statements.CheckForContradictions();

					// assert
					Assert.AreEqual(0, contradictions.Count);
				}
			}
		}

		[Test]
		public void GivenSingleSignChain_WhenCheckContradictions_ThenNoContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			char letter = 'A';
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(CreateProcessConcept(letter.ToString()));
				letter++;
			}

			// act
			foreach (var sign in SequenceSigns.TransitiveSigns)
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
			var concept1 = CreateProcessConcept("A");
			var concept2 = CreateProcessConcept("B");

			// act
			foreach (var sign in SequenceSigns.All)
			{
				var statements = new List<ProcessesStatement>
				{
					new ProcessesStatement(null, concept1, concept2, sign),
					new ProcessesStatement(null, concept1, concept2, sign),
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
			var conceptA = CreateProcessConcept("A");
			var conceptB = CreateProcessConcept("B");

			// act
			foreach (var sign in SequenceSigns.All)
			{
				foreach (var contradictedSign in SequenceSigns.All)
				{
					if (new[] { sign, contradictedSign }.Contradicts())
					{
						var statements = new List<ProcessesStatement>
						{
							new ProcessesStatement(null, conceptA, conceptB, sign),
							new ProcessesStatement(null, conceptA, conceptB, contradictedSign),
						};

						var contradictions = statements.CheckForContradictions();

						// assert
						var contradiction = contradictions.First(c => c.Value1 != c.Value2);
						Assert.IsTrue(contradiction.Value1 == conceptA || contradiction.Value1 == conceptB);
						Assert.IsTrue(contradiction.Value2 == conceptA || contradiction.Value2 == conceptB);
						Assert.LessOrEqual(2, contradiction.Signs.Count);
						Assert.IsTrue(contradiction.Signs.Contains(sign));
						Assert.IsTrue(contradiction.Signs.Contains(contradictedSign));
					}
				}
			}
		}

		[Test]
		public void GivenSingleSignSequenceLoop_WhenCheckContradictions_ThenFindContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			char letter = 'A';
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(CreateProcessConcept(letter.ToString()));
				letter++;
			}

			// act
			for (int chainLength = 3; chainLength <= Math.Max(3, _testChainLength); chainLength++)
			{
				foreach (var sign in SequenceSigns.TransitiveSigns.Except(SequenceSigns.WhenSigns))
				{
					var statements = CreateSimpleLoop(concepts.Take(chainLength).ToList(), sign);

					var contradictions = statements.CheckForContradictions();

					// assert
					Assert.AreNotEqual(0, contradictions.Count);
				}
			}
		}

		[Test]
		public void GivenValueSequencedItself_WhenCheckContradictions_ThenCheckSelfContradiction()
		{
			// arrange
			var process = CreateProcessConcept("process");

			// act
			foreach (var sign in SequenceSigns.All)
			{
				var statements = new[] { new ProcessesStatement(null, process, process, sign) };

				var contradictions = statements.CheckForContradictions();

				// assert
				if (SequenceSigns.SelfInvalidSigns.Contains(sign))
				{
					Assert.IsTrue(contradictions.Single().Signs.Contains(sign));
				}
				else
				{
					Assert.AreEqual(0, contradictions.Count);
				}
			}
		}

		private static IConcept CreateProcessConcept(string name)
		{
			var concept = name.CreateConcept();
			concept.WithAttribute(IsProcessAttribute.Value);
			return concept;
		}

		private static List<ProcessesStatement> CreateSimpleChain(List<IConcept> concepts, IConcept sign)
		{
			var statements = new List<ProcessesStatement>();
			for (int i = 0; i < concepts.Count - 1; i++)
			{
				statements.Add(new ProcessesStatement(null, concepts[i],
					concepts[i + 1],
					sign));
			}
			return statements;
		}

		private static List<ProcessesStatement> CreateSimpleLoop(List<IConcept> concepts, IConcept sign)
		{
			var statements = CreateSimpleChain(concepts, sign);
			statements.Add(new ProcessesStatement(null, statements.Last().ProcessB,
				statements.First().ProcessA,
				sign));
			return statements;
		}
	}
}
