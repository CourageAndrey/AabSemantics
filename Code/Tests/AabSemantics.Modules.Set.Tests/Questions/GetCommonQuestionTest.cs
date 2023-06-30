﻿using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class GetCommonQuestionTest
	{
		[Test]
		public void GivenConceptsCanNotBeCompared_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.AreaType_Air, semanticNetwork.MotorType_Jet);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenNoCommon_WhenBeingAsked_ThenReturnEmptyExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_JetFighter);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}

		[Test]
		public void GivenAllSetOnTheSameLevel_WhenBeingAsked_ThenReturnAllCommon()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.Vehicle_Steamboat, semanticNetwork.Vehicle_SteamLocomotive);

			// assert
			Assert.IsFalse(answer.IsEmpty);

			Assert.AreSame(semanticNetwork.Sign_MotorType, (((ConceptsAnswer) answer).Result).Single());

			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}

		[Test]
		public void GivenAllSetOnTheDifferentLevels_WhenBeingAsked_ThenReturnAllCommonManyLevels()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			CreateCompareConceptsTest(semanticNetwork);

			// act
			var answer = semanticNetwork.Ask().WhatInCommon(
				semanticNetwork.Concepts.First(c => c.HasAttribute<IsProcessAttribute>()),
				semanticNetwork.Concepts.Last(c => c.HasAttribute<IsProcessAttribute>()));

			// assert
			Assert.IsFalse(answer.IsEmpty);

			var signs = (((ConceptsAnswer) answer).Result).Select(c => c.Name.GetValue(language)).ToList();
			Assert.AreEqual(2, signs.Count);
			Assert.IsTrue(signs.Contains(SignSameValues));
			Assert.IsTrue(signs.Contains(SignBothNotSet));

			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}

		internal const string SignSameValues = "Same values";
		internal const string SignBothNotSet = "Both not set";
		internal const string SignDifferentValues = "Different values";
		internal const string SignFirstNotSet = "First not set";
		internal const string SignSecondNotSet = "Second not set";
		internal const string SignPseudoCommon = "Pseudo common";
		internal const string SignPseudoDifference = "Pseudo difference";

		internal static void CreateCompareConceptsTest(ISemanticNetwork semanticNetwork)
		{
			var parent = "Parent 1".CreateConcept();
			var parentOfParent = "Parent 2".CreateConcept();
			var differentParent = "Different Parent".CreateConcept();
			var concept1 = "Concept 1".CreateConcept();
			var concept2 = "Concept 2".CreateConcept();
			concept1.WithAttribute(IsProcessAttribute.Value);
			concept2.WithAttribute(IsProcessAttribute.Value);
			semanticNetwork.Concepts.Add(parent);
			semanticNetwork.Concepts.Add(parentOfParent);
			semanticNetwork.Concepts.Add(differentParent);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);

			semanticNetwork.DeclareThat(parent).IsAncestorOf(concept1);
			semanticNetwork.DeclareThat(parent).IsAncestorOf(concept2);
			semanticNetwork.DeclareThat(parentOfParent).IsAncestorOf(parent);
			semanticNetwork.DeclareThat(differentParent).IsAncestorOf(concept1);
			semanticNetwork.DeclareThat(differentParent).IsAncestorOf(concept2);

			var signSameValues = SignSameValues.CreateConcept();
			var signBothNotSet = SignBothNotSet.CreateConcept();
			var signDifferentValues = SignDifferentValues.CreateConcept();
			var signFirstNotSet = SignFirstNotSet.CreateConcept();
			var signSecondNotSet = SignSecondNotSet.CreateConcept();
			var signPseudoCommon = SignPseudoCommon.CreateConcept();
			var signPseudoDifference = SignPseudoDifference.CreateConcept();
			foreach (var sign in new[]
			{
				signSameValues,
				signBothNotSet,
				signDifferentValues,
				signFirstNotSet,
				signSecondNotSet,
				signPseudoCommon,
				signPseudoDifference,
			})
			{
				sign.WithAttribute(IsSignAttribute.Value);
			}
			semanticNetwork.Concepts.Add(signSameValues);
			semanticNetwork.Concepts.Add(signBothNotSet);
			semanticNetwork.Concepts.Add(signDifferentValues);
			semanticNetwork.Concepts.Add(signFirstNotSet);
			semanticNetwork.Concepts.Add(signSecondNotSet);
			semanticNetwork.Concepts.Add(signPseudoCommon);
			semanticNetwork.Concepts.Add(signPseudoDifference);

			semanticNetwork.DeclareThat(parent).HasSign(signSameValues);
			semanticNetwork.DeclareThat(parent).HasSign(signBothNotSet);
			semanticNetwork.DeclareThat(parentOfParent).HasSign(signDifferentValues);
			semanticNetwork.DeclareThat(differentParent).HasSign(signFirstNotSet);
			semanticNetwork.DeclareThat(differentParent).HasSign(signSecondNotSet);

			semanticNetwork.DeclareThat(parent).HasSignValue(signSameValues, LogicalValues.True);
			semanticNetwork.DeclareThat(concept1).HasSignValue(signDifferentValues, LogicalValues.True);
			semanticNetwork.DeclareThat(concept2).HasSignValue(signDifferentValues, LogicalValues.False);
			semanticNetwork.DeclareThat(concept2).HasSignValue(signFirstNotSet, LogicalValues.True);
			semanticNetwork.DeclareThat(concept1).HasSignValue(signSecondNotSet, LogicalValues.True);
		}
	}
}