using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
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
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GetCommonQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new GetCommonQuestion(concept, null));
		}

		[Test]
		public void GivenSameArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentException>(() => new GetCommonQuestion(concept, concept));
		}

		[Test]
		public void GivenGetCommon_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new GetCommonQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Airbus);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Airbus);

			// assert
			Assert.That(questionRegular.Concept1, Is.SameAs(semanticNetwork.Vehicle_Car));
			Assert.That(questionRegular.Concept2, Is.SameAs(semanticNetwork.Vehicle_Airbus));
			Assert.That(answerRegular.Result.SequenceEqual(answerBuilder.Result), Is.True);
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
		}

		[Test]
		public void GivenConceptsCanNotBeCompared_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.AreaType_Air, semanticNetwork.MotorType_Jet);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(text.Contains("have no common ancestors and can not be compared."), Is.True);
		}

		[Test]
		public void GivenNoCommon_WhenBeingAsked_ThenReturnEmptyExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_JetFighter);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.GreaterThan(0));

			Assert.That(text.Contains("No common found according to existing information."), Is.True);
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
			Assert.That(answer.IsEmpty, Is.False);

			Assert.That((((ConceptsAnswer) answer).Result).Single(), Is.SameAs(semanticNetwork.Sign_MotorType));

			Assert.That(answer.Explanation.Statements.Count, Is.GreaterThan(0));
		}

		[Test]
		public void GivenAllSetOnTheDifferentLevels_WhenBeingAsked_ThenReturnAllCommonManyLevels()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			CreateCompareConceptsTest(semanticNetwork);

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.Ask().WhatInCommon(
				semanticNetwork.Concepts["Concept 1"],
				semanticNetwork.Concepts["Concept 2"]);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);

			var signs = (((ConceptsAnswer) answer).Result).Select(c => c.Name.GetValue(language)).ToList();
			Assert.That(signs.Count, Is.EqualTo(2));
			Assert.That(signs.Contains(SignSameValues), Is.True);
			Assert.That(signs.Contains(SignBothNotSet), Is.True);

			Assert.That(answer.Explanation.Statements.Count, Is.GreaterThan(0));

			Assert.That(text.Contains("Both have ") && text.Contains(" sign value equal to "), Is.True);
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
			semanticNetwork.Context.Language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			semanticNetwork.Context.Language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			semanticNetwork.Context.Language.Extensions.Add(LanguageSetModule.CreateDefault());

			var parent = "Parent 1".CreateConceptByName();
			var parentOfParent = "Parent 2".CreateConceptByName();
			var differentParent = "Different Parent".CreateConceptByName();
			var concept1 = "Concept 1".CreateConceptByName();
			var concept2 = "Concept 2".CreateConceptByName();
			concept1.WithAttribute(IsValueAttribute.Value);
			concept2.WithAttribute(IsValueAttribute.Value);
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

			var signSameValues = SignSameValues.CreateConceptByName();
			var signBothNotSet = SignBothNotSet.CreateConceptByName();
			var signDifferentValues = SignDifferentValues.CreateConceptByName();
			var signFirstNotSet = SignFirstNotSet.CreateConceptByName();
			var signSecondNotSet = SignSecondNotSet.CreateConceptByName();
			var signPseudoCommon = SignPseudoCommon.CreateConceptByName();
			var signPseudoDifference = SignPseudoDifference.CreateConceptByName();
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

		[Test]
		public void GivenDifferentHierarchyLevels_WhenBeingAsked_ThenReturnAdditionalHierarchies()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			GetCommonQuestionTest.CreateCompareConceptsTest(semanticNetwork);

			const string sideBranchId = "SIDE_BRANCH";
			var sideBranch = sideBranchId.CreateConceptByName();
			semanticNetwork.Concepts.Add(sideBranch);
			semanticNetwork.DeclareThat(sideBranch).IsDescendantOf(semanticNetwork.Concepts["Parent 2"]);

			var render = TextRenders.PlainString;

			// act
			var answerFirst = semanticNetwork.Ask().WhatIsTheDifference(
				semanticNetwork.Concepts["Concept 1"],
				semanticNetwork.Concepts[sideBranchId]);
			var textFirst = render.RenderText(answerFirst.Description, language).ToString();

			var answerSecond = semanticNetwork.Ask().WhatIsTheDifference(
				semanticNetwork.Concepts[sideBranchId],
				semanticNetwork.Concepts["Concept 1"]);
			var textSecond = render.RenderText(answerSecond.Description, language).ToString();

			// assert
			Assert.That(answerFirst.IsEmpty, Is.False);
			Assert.That(answerSecond.IsEmpty, Is.False);

			Assert.That(answerFirst.Explanation.Statements.Count, Is.GreaterThan(0));
			Assert.That(answerSecond.Explanation.Statements.Count, Is.GreaterThan(0));

			Assert.That(textFirst.Contains("First is also:"), Is.True);
			Assert.That(textSecond.Contains("Second is also:"), Is.True);
		}
	}
}
