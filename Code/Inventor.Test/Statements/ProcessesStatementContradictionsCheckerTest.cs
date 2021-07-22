using System;
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
	public class ProcessesStatementContradictionsCheckerTest
	{
		private const int _testChainLength = 5;

		[Test]
		public void SingleStatementCanNotContradict()
		{
			// arrange
			var processA = createProcessConcept("A");
			var processB = createProcessConcept("B");

			// act
			foreach (var sign in SequenceSigns.All)
			{
				var statements = new[] { new ProcessesStatement(processA, processB, sign) };

				var contradictions = statements.CheckForContradictions();

				// assert
				Assert.AreEqual(0, contradictions.Count);
			}
		}

		[Test]
		public void ValidSequenceCombinationsCanNotContradict()
		{
			// arrange
			var processA = createProcessConcept("A");
			var processI = createProcessConcept("I");
			var processB = createProcessConcept("B");

			// act
			foreach (var signPair in SequenceSigns.ValidSequenceCombinations)
			{
				var signAI = signPair.Key;
				foreach (var signIB in signPair.Value.Keys)
				{
					var statements = new[]
					{
						new ProcessesStatement(processA, processI, signAI),
						new ProcessesStatement(processI, processB, signIB),
					};

					var contradictions = statements.CheckForContradictions();

					// assert
					Assert.AreEqual(0, contradictions.Count);
				}
			}
		}

		[Test]
		public void SingleTransitiveSignChainsHaveNoContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			char letter = 'A';
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(createProcessConcept(letter.ToString()));
				letter++;
			}

			// act
			foreach (var sign in SequenceSigns.TransitiveSigns)
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
			var concept1 = createProcessConcept("A");
			var concept2 = createProcessConcept("B");

			// act
			foreach (var sign in SequenceSigns.All)
			{
				var statements = new List<ProcessesStatement>
				{
					new ProcessesStatement(concept1, concept2, sign),
					new ProcessesStatement(concept1, concept2, sign),
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
			var conceptA = createProcessConcept("A");
			var conceptB = createProcessConcept("B");

			// act
			foreach (var sign in SequenceSigns.All)
			{
				var revertedSign = SequenceSigns.Revert(sign);
				if (sign != revertedSign)
				{
					var statements = new List<ProcessesStatement>
					{
						new ProcessesStatement(conceptA, conceptB, sign),
						new ProcessesStatement(conceptA, conceptB, revertedSign),
					};

					var contradictions = statements.CheckForContradictions();

					// assert
					var contradiction = contradictions.First(c => c.Value1 != c.Value2);
					Assert.IsTrue(contradiction.Value1 == conceptA || contradiction.Value1 == conceptB);
					Assert.IsTrue(contradiction.Value2 == conceptA || contradiction.Value2 == conceptB);
					Assert.LessOrEqual(2, contradiction.Signs.Count);
					Assert.IsTrue(contradiction.Signs.Contains(sign));
					Assert.IsTrue(contradiction.Signs.Contains(revertedSign));
				}
			}
		}

		[Test]
		public void SingleSignLoopsHaveContradictions()
		{
			// arrange
			var concepts = new List<IConcept>();
			char letter = 'A';
			for (int i = 0; i < _testChainLength; i++)
			{
				concepts.Add(createProcessConcept(letter.ToString()));
				letter++;
			}

			// act
			for (int chainLength = 3; chainLength <= Math.Max(3, _testChainLength); chainLength++)
			{
				foreach (var sign in SequenceSigns.TransitiveSigns.Except(SequenceSigns.WhenSigns))
				{
					var statements = createSimpleLoop(concepts.Take(chainLength).ToList(), sign);

					var contradictions = statements.CheckForContradictions();

					// assert
					Assert.AreNotEqual(0, contradictions.Count);
				}
			}
		}

		private static IConcept createProcessConcept(string name)
		{
			var concept = new Concept(
				new LocalizedStringVariable(new[] { new KeyValuePair<string, string>(Language.Default.Culture, name) }),
				new LocalizedStringVariable(new[] { new KeyValuePair<string, string>(Language.Default.Culture, name) }));
			concept.Attributes.Add(IsProcessAttribute.Value);
			return concept;
		}

		private static List<ProcessesStatement> createSimpleChain(List<IConcept> concepts, IConcept sign)
		{
			var statements = new List<ProcessesStatement>();
			for (int i = 0; i < concepts.Count - 1; i++)
			{
				statements.Add(new ProcessesStatement(
					concepts[i],
					concepts[i + 1],
					sign));
			}
			return statements;
		}

		private static List<ProcessesStatement> createSimpleLoop(List<IConcept> concepts, IConcept sign)
		{
			var statements = createSimpleChain(concepts, sign);
			statements.Add(new ProcessesStatement(
				statements.Last().ProcessB,
				statements.First().ProcessA,
				sign));
			return statements;
		}
	}
}
