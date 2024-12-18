using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Statements
{
	[TestFixture]
	public class SignValueTest
	{
		private const string ConceptId_TopLevel = "CONCEPT_TOP";
		private const string ConceptId_MiddleLevel = "CONCEPT_MIDDLE";
		private const string ConceptId_DownLevel = "CONCEPT_DOWN";
		private const string ConceptId_Sign1 = "SIGN_1";
		private const string ConceptId_Sign2 = "SIGN_2";
		private const string ConceptId_Sign3 = "SIGN_3";
		private const string ConceptId_ValueA = "VALUE_A";
		private const string ConceptId_ValueB = "VALUE_B";
		private const string ConceptId_ValueC = "VALUE_C";
		private const string ConceptId_ValueD = "VALUE_D";
		private const string ConceptId_ValueE = "VALUE_E";

		[Test]
		public void GivenNoSignValues_WhenGetSignValues_ThenReturnNothing()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = CreateTest(language);

			// act
			var withoutRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, semanticNetwork.Concepts.First(), false);
			var withRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, semanticNetwork.Concepts.First(), true);

			// assert
			Assert.That(withoutRecursion.Count, Is.EqualTo(0));
			Assert.That(withRecursion.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenOnlyOwnSignValues_WhenGetSignValues_ThenReturnThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = CreateTest(language);

			var concept = semanticNetwork.Concepts[ConceptId_DownLevel];

			var statements = semanticNetwork.DeclareThat(concept).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ semanticNetwork.Concepts[ConceptId_Sign1], semanticNetwork.Concepts[ConceptId_ValueA] },
				{ semanticNetwork.Concepts[ConceptId_Sign2], semanticNetwork.Concepts[ConceptId_ValueB] },
				{ semanticNetwork.Concepts[ConceptId_Sign3], semanticNetwork.Concepts[ConceptId_ValueC] },
			}).OrderBy(s => s.ID).ToList();

			// act
			var withoutRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, false).OrderBy(s => s.ID).ToList();
			var withRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, true).OrderBy(s => s.ID).ToList();

			// assert
			Assert.That(withoutRecursion.SequenceEqual(statements), Is.True);
			Assert.That(withRecursion.SequenceEqual(statements), Is.True);
		}

		[Test]
		public void GivenOnlyInheritedSignValues_WhenGetSignValues_ThenReturnThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = CreateTest(language);

			var conceptTop = semanticNetwork.Concepts[ConceptId_TopLevel];
			var conceptMiddle = semanticNetwork.Concepts[ConceptId_MiddleLevel];
			var concept = semanticNetwork.Concepts[ConceptId_DownLevel];

			var statements = new List<SignValueStatement>
			{
				semanticNetwork.DeclareThat(conceptTop).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign1], semanticNetwork.Concepts[ConceptId_ValueA]),
				semanticNetwork.DeclareThat(conceptMiddle).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign2], semanticNetwork.Concepts[ConceptId_ValueB]),
			}.OrderBy(s => s.ID).ToList();

			// act
			var withoutRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, false).OrderBy(s => s.ID).ToList();
			var withRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, true).OrderBy(s => s.ID).ToList();

			// assert
			Assert.That(withoutRecursion.Count, Is.EqualTo(0));
			Assert.That(withRecursion.SequenceEqual(statements), Is.True);
		}

		[Test]
		public void GivenAllSignValues_WhenGetSignValues_ThenReturnThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = CreateTest(language);

			var conceptTop = semanticNetwork.Concepts[ConceptId_TopLevel];
			var conceptMiddle = semanticNetwork.Concepts[ConceptId_MiddleLevel];
			var concept = semanticNetwork.Concepts[ConceptId_DownLevel];

			var statements = new List<SignValueStatement>
			{
				semanticNetwork.DeclareThat(conceptTop).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign1], semanticNetwork.Concepts[ConceptId_ValueA]),
				semanticNetwork.DeclareThat(conceptMiddle).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign2], semanticNetwork.Concepts[ConceptId_ValueB]),
				semanticNetwork.DeclareThat(concept).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign3], semanticNetwork.Concepts[ConceptId_ValueC]),
			}.OrderBy(s => s.ID).ToList();

			// act
			var withoutRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, false).OrderBy(s => s.ID).ToList();
			var withRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, true).OrderBy(s => s.ID).ToList();

			// assert
			Assert.That(withoutRecursion.Single().Value, Is.SameAs(semanticNetwork.Concepts[ConceptId_ValueC]));
			Assert.That(withRecursion.SequenceEqual(statements), Is.True);
		}

		[Test]
		public void GivenOverridenSignValues_WhenGetSignValues_ThenReturnThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = CreateTest(language);

			var conceptTop = semanticNetwork.Concepts[ConceptId_TopLevel];
			var conceptMiddle = semanticNetwork.Concepts[ConceptId_MiddleLevel];
			var concept = semanticNetwork.Concepts[ConceptId_DownLevel];

			semanticNetwork.DeclareThat(conceptTop).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign1], semanticNetwork.Concepts[ConceptId_ValueA]);
			semanticNetwork.DeclareThat(conceptMiddle).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign2], semanticNetwork.Concepts[ConceptId_ValueB]);
			var statements = new List<SignValueStatement>
			{
				semanticNetwork.DeclareThat(concept).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign1], semanticNetwork.Concepts[ConceptId_ValueC]),
				semanticNetwork.DeclareThat(concept).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign2], semanticNetwork.Concepts[ConceptId_ValueD]),
				semanticNetwork.DeclareThat(concept).HasSignValue(semanticNetwork.Concepts[ConceptId_Sign3], semanticNetwork.Concepts[ConceptId_ValueE]),
			}.OrderBy(s => s.ID).ToList();

			// act
			var withoutRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, false).OrderBy(s => s.ID).ToList();
			var withRecursion = SignValueStatement.GetSignValues(semanticNetwork.Statements, concept, true).OrderBy(s => s.ID).ToList();

			// assert
			Assert.That(withoutRecursion.SequenceEqual(statements), Is.True);
			Assert.That(withRecursion.SequenceEqual(statements), Is.True);
		}

		private static ISemanticNetwork CreateTest(ILanguage language)
		{
			var semanticNetwork = new SemanticNetwork(language);

			var topLevelConcept = ConceptId_TopLevel.CreateConceptByName();
			var middleLevelConcept = ConceptId_MiddleLevel.CreateConceptByName();
			var downLevelConcept = ConceptId_DownLevel.CreateConceptByName();
			semanticNetwork.Concepts.Add(topLevelConcept);
			semanticNetwork.Concepts.Add(middleLevelConcept);
			semanticNetwork.Concepts.Add(downLevelConcept);

			var sign1 = ConceptId_Sign1.CreateConceptByName().WithAttribute(IsSignAttribute.Value);
			var sign2 = ConceptId_Sign2.CreateConceptByName().WithAttribute(IsSignAttribute.Value);
			var sign3 = ConceptId_Sign3.CreateConceptByName().WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(sign1);
			semanticNetwork.Concepts.Add(sign2);
			semanticNetwork.Concepts.Add(sign3);

			var valueA = ConceptId_ValueA.CreateConceptByName().WithAttribute(IsValueAttribute.Value);
			var valueB = ConceptId_ValueB.CreateConceptByName().WithAttribute(IsValueAttribute.Value);
			var valueC = ConceptId_ValueC.CreateConceptByName().WithAttribute(IsValueAttribute.Value);
			var valueD = ConceptId_ValueD.CreateConceptByName().WithAttribute(IsValueAttribute.Value);
			var valueE = ConceptId_ValueE.CreateConceptByName().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(valueA);
			semanticNetwork.Concepts.Add(valueB);
			semanticNetwork.Concepts.Add(valueC);
			semanticNetwork.Concepts.Add(valueD);
			semanticNetwork.Concepts.Add(valueE);

			semanticNetwork.DeclareThat(topLevelConcept).IsAncestorOf(middleLevelConcept);
			semanticNetwork.DeclareThat(middleLevelConcept).IsAncestorOf(downLevelConcept);

			return semanticNetwork;
		}
	}
}
