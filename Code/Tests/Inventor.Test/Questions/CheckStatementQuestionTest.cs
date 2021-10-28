using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics;
using Inventor.Semantics.Answers;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class CheckStatementQuestionTest
	{
		[Test]
		public void ReturnEmptyAnswerIfNoMathingStatementsFound()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var statementToCheck = new TestStatement(1);
			var statementWrong = new TestStatement(2);

			// act
			var answerNoStatements = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			semanticNetwork.Statements.Add(statementWrong);
			var answerWrongStatement = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			// assert
			var answer = (BooleanAnswer) answerNoStatements;
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(answer.Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);

			answer = (BooleanAnswer) answerWrongStatement;
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(answer.Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnFoundStatementIfExists()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var statementToCheck = new TestStatement(1);
			var statementRight = new TestStatement(1);
			var statementWrong = new TestStatement(2);
			semanticNetwork.Statements.Add(statementRight);
			semanticNetwork.Statements.Add(statementWrong);

			// act
			var answerToCheck = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			var answerRight = semanticNetwork.Ask().IsTrueThat(statementRight);

			// assert
			Assert.IsFalse(answerToCheck.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerToCheck).Result);
			Assert.AreSame(statementRight, answerToCheck.Explanation.Statements.Single());

			Assert.IsFalse(answerRight.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerRight).Result);
			Assert.AreSame(statementRight, answerRight.Explanation.Statements.Single());
		}

		[Test]
		public void SupportTransitiveStatements()
		{
			// arrange
			var language = Language.Default;

			var semanticNetwork = new SemanticNetwork(language);
			var conceptParent = "parent".CreateConcept();
			var conceptIntermediate = "intermediate".CreateConcept();
			var conceptChild = "child".CreateConcept();
			semanticNetwork.Concepts.Add(conceptParent);
			semanticNetwork.Concepts.Add(conceptIntermediate);
			semanticNetwork.Concepts.Add(conceptChild);

			var statementPI = new TransitiveTestStatement(conceptParent, conceptIntermediate);
			var statementIC = new TransitiveTestStatement(conceptIntermediate, conceptChild);
			semanticNetwork.Statements.Add(statementPI);
			semanticNetwork.Statements.Add(statementIC);

			var statementToCheck = new TransitiveTestStatement(conceptParent, conceptChild);

			// act
			var answer = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.IsTrue(answer.Explanation.Statements.Contains(statementPI));
			Assert.IsTrue(answer.Explanation.Statements.Contains(statementIC));
		}

		private class TestStatement : Statement<TestStatement>
		{
			public int Number
			{ get; }

			public TestStatement(int number)
				: base(number.ToString(), new LocalizedStringConstant(l => number.ToString()), new LocalizedStringConstant(l => number.ToString()))
			{
				Number = number;
			}

			public override IEnumerable<IConcept> GetChildConcepts()
			{
				yield break;
			}

			protected override string GetDescriptionTrueText(ILanguage language)
			{
				return string.Empty;
			}

			protected override string GetDescriptionFalseText(ILanguage language)
			{
				return string.Empty;
			}

			protected override string GetDescriptionQuestionText(ILanguage language)
			{
				return string.Empty;
			}

			protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
			{
				return new Dictionary<string, IKnowledge>();
			}

			public override bool Equals(TestStatement other)
			{
				return Number == other.Number;
			}
		}

		private class TransitiveTestStatement : Statement<TransitiveTestStatement>, IParentChild<IConcept>
		{

			public IConcept Parent
			{ get; }

			public IConcept Child
			{ get; }

			public TransitiveTestStatement(IConcept parent, IConcept child)
				: base(string.Empty.EnsureIdIsSet(), new LocalizedStringConstant(l => string.Empty), new LocalizedStringConstant(l => string.Empty))
			{
				Parent = parent;
				Child = child;
			}

			public override bool Equals(TransitiveTestStatement other)
			{
				return Parent == other.Parent && Child == other.Child;
			}

			public override IEnumerable<IConcept> GetChildConcepts()
			{
				yield return Parent;
				yield return Child;
			}

			protected override string GetDescriptionTrueText(ILanguage language)
			{
				return string.Empty;
			}

			protected override string GetDescriptionFalseText(ILanguage language)
			{
				return string.Empty;
			}

			protected override string GetDescriptionQuestionText(ILanguage language)
			{
				return string.Empty;
			}

			protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
			{
				return new Dictionary<string, IKnowledge>();
			}
		}
	}
}
