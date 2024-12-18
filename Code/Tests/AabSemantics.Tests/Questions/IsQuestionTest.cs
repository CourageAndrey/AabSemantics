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
			IConcept concept = "test".CreateConceptByName();

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
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var questionRegular = new IsQuestion(car, vehicle);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.Ask().IfIs(car, vehicle);

			// assert
			Assert.That(answerBuilder.Result, Is.EqualTo(answerRegular.Result));
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
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
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var answer = semanticNetwork.Ask().IfIs(vehicle, car);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
			Assert.That(answer.Description.ToString().StartsWith("No, "), Is.True);
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
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var answer = semanticNetwork.Ask().IfIs(car, vehicle);

			//assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.True);

			var classification = (IsStatement) answer.Explanation.Statements.Single();
			Assert.That(classification.Ancestor, Is.SameAs(vehicle));
			Assert.That(classification.Descendant, Is.SameAs(car));
			Assert.That(answer.Description.ToString().StartsWith("Yes, "), Is.True);
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
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			IConcept sportcar;
			semanticNetwork.Concepts.Add(sportcar = "sportcar".CreateConceptByName());
			var classification = semanticNetwork.DeclareThat(sportcar).IsDescendantOf(car);

			// act
			var answer = semanticNetwork.Ask().IfIs(sportcar, vehicle);

			//assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.True);

			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answer.Explanation.Statements.OfType<IsStatement>().Count(), Is.EqualTo(2));
			Assert.That(answer.Explanation.Statements.Contains(classification), Is.True);
		}
	}
}
