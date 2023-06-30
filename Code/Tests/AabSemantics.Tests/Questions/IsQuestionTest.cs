﻿using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Tests;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class IsQuestionTest
	{
		[Test]
		public void GivenNoStatements_WhenBeingAsked_ThenEmptyResult()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIs(semanticNetwork.Sign_AreaType, semanticNetwork.Base_Vehicle);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenCertainStatement_WhenBeingAsked_ThenFindIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIs(semanticNetwork.Vehicle_Airbus, semanticNetwork.Base_Vehicle);

			//assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var classification = (IsStatement) answer.Explanation.Statements.Single();
			Assert.AreSame(semanticNetwork.Base_Vehicle, classification.Ancestor);
			Assert.AreSame(semanticNetwork.Vehicle_Airbus, classification.Descendant);
		}

		[Test]
		public void GivenTransition_WhenBeingAsked_ThenFindThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var hugeAirbus = ConceptCreationHelper.CreateConcept();
			semanticNetwork.SemanticNetwork.Concepts.Add(hugeAirbus);

			var classification = semanticNetwork.SemanticNetwork.DeclareThat(hugeAirbus).IsDescendantOf(semanticNetwork.Vehicle_Airbus);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIs(hugeAirbus, semanticNetwork.Base_Vehicle);

			//assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.AreEqual(2, answer.Explanation.Statements.OfType<IsStatement>().Count());
			Assert.IsTrue(answer.Explanation.Statements.Contains(classification));
		}
	}
}