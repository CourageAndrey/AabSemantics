using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class CustomStatementQuestionTest
	{
		[Test]
		public void GivenNoStatements_WhenBeingAsked_ThenEmptyResult()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			// act
			var question = new CustomStatementQuestion(null, null);
			var answer = question.Ask(semanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Count, Is.EqualTo(0));
#warning Assert.That(answer.Description.ToString().StartsWith("No, "), Is.True);
		}

		[Test]
		public void GivenNoFilters_WhenBeingAsked_ThenReturnAllCustom()
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

			var custom = DeclareCustom(
				semanticNetwork,
				"other one",
				new Dictionary<string, IConcept> { { "car", car }, { "vehicle", vehicle } });

			// act
			var question = new CustomStatementQuestion(null, null);
			var answer = question.Ask(semanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Single(), Is.SameAs(custom));
#warning Assert.That(answer.Description.ToString().StartsWith("No, "), Is.True);
		}

		[Test]
		public void GivenByType_WhenBeingAsked_ThenFindCorrespondingStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());
			CustomStatement custom;
			semanticNetwork.Statements.Add(custom = new IsStatement(null, vehicle, car).ToCustomStatement());

			DeclareCustom(
				semanticNetwork,
				"other one",
				new Dictionary<string, IConcept> { { "car", car }, { "vehicle", vehicle } });

			// act
			var question = new CustomStatementQuestion(nameof(IsStatement), null);
			var answer = question.Ask(semanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Single(), Is.SameAs(custom));
#warning Assert.That(answer.Description.ToString().StartsWith("No, "), Is.True);
		}

		[Test]
		public void GivenByAllParameters_WhenBeingAsked_ThenFindCorrespondingStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());

			semanticNetwork.Statements.Add(new IsStatement(null, vehicle, car).ToCustomStatement());

			CustomStatement custom1;
			semanticNetwork.Statements.Add(custom1 = new IsStatement(null, car, vehicle).ToCustomStatement());

			var parameters = new Dictionary<string, IConcept> { { "car", car }, { "vehicle", vehicle } };
			var custom2 = DeclareCustom(
				semanticNetwork,
				custom1.Type,
				parameters);

			var custom3 = DeclareCustom(
				semanticNetwork,
				"other one",
				parameters);

			// act
			var question = new CustomStatementQuestion(null, parameters);
			var answer = question.Ask(semanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Count, Is.EqualTo(2));
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Contains(custom3), Is.True);
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result, Contains.Item(custom2));
#warning Assert.That(answer.Description.ToString().StartsWith("No, "), Is.True);
		}

		[Test]
		public void GivenBySomeParameters_WhenBeingAsked_ThenFindCorrespondingStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());

			semanticNetwork.Statements.Add(new IsStatement(null, vehicle, car).ToCustomStatement());

			CustomStatement statement;
			semanticNetwork.Statements.Add(statement = new IsStatement(null, car, vehicle).ToCustomStatement());

			var parameters = new Dictionary<string, IConcept> { { "car", car }, { "vehicle", vehicle } };
			var custom1 = DeclareCustom(
				semanticNetwork,
				statement.Type,
				parameters);

			var custom2 = DeclareCustom(
				semanticNetwork,
				"other one",
				parameters);

			// act
			var question = new CustomStatementQuestion(null, new Dictionary<string, IConcept> { { "car", car } });
			var answer = question.Ask(semanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Count, Is.EqualTo(2));
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result, Contains.Item(custom1));
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result, Contains.Item(custom2));
#warning Assert.That(answer.Description.ToString().StartsWith("No, "), Is.True);
		}

		[Test]
		public void GivenByFullInfo_WhenBeingAsked_ThenFindCorrespondingStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());

			semanticNetwork.Statements.Add(new IsStatement(null, vehicle, car).ToCustomStatement());

			CustomStatement statement;
			semanticNetwork.Statements.Add(statement = new IsStatement(null, car, vehicle).ToCustomStatement());

			var parameters = new Dictionary<string, IConcept> { { "car", car }, { "vehicle", vehicle } };
			DeclareCustom(
				semanticNetwork,
				statement.Type,
				parameters);

			var custom = DeclareCustom(
				semanticNetwork,
				"other one",
				parameters);

			// act
			var question = new CustomStatementQuestion("other one", parameters);
			var answer = question.Ask(semanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Count, Is.EqualTo(1));
			Assert.That(((StatementsAnswer<CustomStatement>) answer).Result.Single(), Is.SameAs(custom));
#warning Assert.That(answer.Description.ToString().StartsWith("No, "), Is.True);
		}

		private static CustomStatement DeclareCustom(ISemanticNetwork semanticNetwork, string type, IDictionary<string, IConcept> parameters)
		{
			CustomStatement result;
			semanticNetwork.Statements.Add(result = new CustomStatement(
				null,
				type,
				l => null,
				l => null,
				l => null,
				LocalizedString.Empty,
				LocalizedString.Empty,
				parameters));
			return result;
		}
	}
}
