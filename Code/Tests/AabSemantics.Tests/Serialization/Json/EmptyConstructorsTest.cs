using NUnit.Framework;

using AabSemantics.Modules.Boolean.Json;
using AabSemantics.Modules.Classification.Json;
using AabSemantics.Serialization.Json;
using AabSemantics.Serialization.Json.Answers;
using AabSemantics.TestCore;

namespace AabSemantics.Tests.Serialization.Json
{
	[TestFixture]
	public class EmptyConstructorsTest
	{
		[Test]
		public void GivenSemanticNetwork_WhenCreateWithoutParameters_ThenSucceed()
		{
			// arrange
			var semanticNetwork = new AabSemantics.Serialization.Json.SemanticNetwork();

			// assert
			Assert.That(semanticNetwork.Name.Values.Count, Is.EqualTo(0));
			Assert.That(semanticNetwork.Concepts.Count, Is.EqualTo(0));
			Assert.That(semanticNetwork.Statements.Count, Is.EqualTo(0));
			Assert.That(semanticNetwork.Modules.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenLocalizedString_WhenCreateWithoutParameters_ThenSucceed()
		{
			// arrange
			var localizedString = new LocalizedString();

			// assert
			Assert.That(localizedString.Values.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenConcept_WhenCreateWithoutParameters_ThenSucceed()
		{
			// arrange
			var concept = new Concept();

			// assert
			Assert.That(concept.ID, Is.Null);
			Assert.That(concept.Name, Is.Null);
			Assert.That(concept.Hint, Is.Null);
			Assert.That(concept.Attributes.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenStatements_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Statement[]
			{
				new IsStatement(),
				new CustomStatement(),
			}.TestParameterlessConstructors();
		}

		[Test]
		public void GivenQuestions_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Question[]
			{
				new CheckStatementQuestion(),
				new EnumerateAncestorsQuestion(),
				new EnumerateDescendantsQuestion(),
				new IsQuestion(),
				new CustomStatementQuestion(),
			}.TestParameterlessConstructors();
		}

		[Test]
		public void GivenAnswers_WhenCreateWithoutParameters_ThenSucceed()
		{
			// arrange
			var emptyAnswer = new Answer();
			var emptyBooleanAnswer = new BooleanAnswer();
			var emptyConceptAnswer = new ConceptAnswer();
			var emptyConceptsAnswer = new ConceptsAnswer();
			var emptyStatementAnswer = new  StatementAnswer();
			var emptyStatementsAnswer = new StatementsAnswer();
			var emptyAnswers = new[]
			{
				emptyAnswer,
				emptyBooleanAnswer,
				emptyConceptAnswer,
				emptyConceptsAnswer,
				emptyStatementAnswer,
				emptyStatementsAnswer,
			};

			// assert
			Assert.That(emptyBooleanAnswer.Result, Is.False);
			Assert.That(emptyConceptAnswer.Concept, Is.Null);
			Assert.That(emptyConceptsAnswer.Concepts.Count, Is.EqualTo(0));
			Assert.That(emptyStatementAnswer.Statement, Is.Null);
			Assert.That(emptyStatementsAnswer.Statements.Count, Is.EqualTo(0));
			foreach (var answer in emptyAnswers)
			{
				Assert.That(answer.IsEmpty, Is.True);
				Assert.That(string.IsNullOrEmpty(answer.Description), Is.True);
				Assert.That(answer.Explanation.Count, Is.EqualTo(0));
			}
		}
	}
}
