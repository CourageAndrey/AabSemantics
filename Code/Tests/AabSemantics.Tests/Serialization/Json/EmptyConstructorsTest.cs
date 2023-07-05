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
			Assert.AreEqual(0, semanticNetwork.Name.Values.Count);
			Assert.AreEqual(0, semanticNetwork.Concepts.Count);
			Assert.AreEqual(0, semanticNetwork.Statements.Count);
			Assert.AreEqual(0, semanticNetwork.Modules.Count);
		}

		[Test]
		public void GivenLocalizedString_WhenCreateWithoutParameters_ThenSucceed()
		{
			// arrange
			var localizedString = new LocalizedString();

			// assert
			Assert.AreEqual(0, localizedString.Values.Count);
		}

		[Test]
		public void GivenConcept_WhenCreateWithoutParameters_ThenSucceed()
		{
			// arrange
			var concept = new Concept();

			// assert
			Assert.IsNull(concept.ID);
			Assert.IsNull(concept.Name);
			Assert.IsNull(concept.Hint);
			Assert.AreEqual(0, concept.Attributes.Count);
		}

		[Test]
		public void GivenStatements_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Statement[]
			{
				new IsStatement(),
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
			Assert.IsFalse(emptyBooleanAnswer.Result);
			Assert.IsNull(emptyConceptAnswer.Concept);
			Assert.AreEqual(0, emptyConceptsAnswer.Concepts.Count);
			Assert.IsNull(emptyStatementAnswer.Statement);
			Assert.AreEqual(0, emptyStatementsAnswer.Statements.Count);
			foreach (var answer in emptyAnswers)
			{
				Assert.AreEqual(true, answer.IsEmpty);
				Assert.IsTrue(string.IsNullOrEmpty(answer.Description));
				Assert.AreEqual(0, answer.Explanation.Count);
			}
		}
	}
}
