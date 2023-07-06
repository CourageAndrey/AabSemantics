using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class IsQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new IsQuestion(concept, null));
		}

		[Test]
		public void GivenIfIs_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConcept());
			semanticNetwork.Concepts.Add(car = "car".CreateConcept());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var questionRegular = new IsQuestion(car, vehicle);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.Ask().IfIs(car, vehicle);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoStatements_WhenBeingAsked_ThenEmptyResult()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConcept());
			semanticNetwork.Concepts.Add(car = "car".CreateConcept());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var answer = semanticNetwork.Ask().IfIs(vehicle, car);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
			Assert.IsTrue(answer.Description.ToString().StartsWith("No, "));
		}

		[Test]
		public void GivenCertainStatement_WhenBeingAsked_ThenFindIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConcept());
			semanticNetwork.Concepts.Add(car = "car".CreateConcept());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var answer = semanticNetwork.Ask().IfIs(car, vehicle);

			//assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var classification = (IsStatement) answer.Explanation.Statements.Single();
			Assert.AreSame(vehicle, classification.Ancestor);
			Assert.AreSame(car, classification.Descendant);
			Assert.IsTrue(answer.Description.ToString().StartsWith("Yes, "));
		}

		[Test]
		public void GivenTransition_WhenBeingAsked_ThenFindThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConcept());
			semanticNetwork.Concepts.Add(car = "car".CreateConcept());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			IConcept sportcar;
			semanticNetwork.Concepts.Add(sportcar = "sportcar".CreateConcept());
			var classification = semanticNetwork.DeclareThat(sportcar).IsDescendantOf(car);

			// act
			var answer = semanticNetwork.Ask().IfIs(sportcar, vehicle);

			//assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.AreEqual(2, answer.Explanation.Statements.OfType<IsStatement>().Count());
			Assert.IsTrue(answer.Explanation.Statements.Contains(classification));
		}
	}
}
