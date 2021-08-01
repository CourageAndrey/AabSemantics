using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

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
			var question = new CheckStatementQuestion(statementToCheck);

			// act
			var answerNoStatements = question.Ask(semanticNetwork.Context);

			semanticNetwork.Statements.Add(statementWrong);
			var answerWrongStatement = question.Ask(semanticNetwork.Context);

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
			var answerToCheck = new CheckStatementQuestion(statementToCheck).Ask(semanticNetwork.Context);

			var answerRight = new CheckStatementQuestion(statementRight).Ask(semanticNetwork.Context);

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
			var conceptParent = new Concept(new LocalizedStringConstant(l => "parent"));
			var conceptIntermediate = new Concept(new LocalizedStringConstant(l => "intermediate"));
			var conceptChild = new Concept(new LocalizedStringConstant(l => "child"));
			semanticNetwork.Concepts.Add(conceptParent);
			semanticNetwork.Concepts.Add(conceptIntermediate);
			semanticNetwork.Concepts.Add(conceptChild);

			var statementPI = new TransitiveTestStatement(conceptParent, conceptIntermediate);
			var statementIC = new TransitiveTestStatement(conceptIntermediate, conceptChild);
			semanticNetwork.Statements.Add(statementPI);
			semanticNetwork.Statements.Add(statementIC);

			var statementToCheck = new TransitiveTestStatement(conceptParent, conceptChild);
			var question = new CheckStatementQuestion(statementToCheck);

			// act
			var answer = question.Ask(semanticNetwork.Context);

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
				: base(new LocalizedStringConstant(l => number.ToString()), new LocalizedStringConstant(l => number.ToString()))
			{
				Number = number;
			}

			public override IEnumerable<IConcept> GetChildConcepts()
			{
				yield break;
			}

			protected override Func<string> GetDescriptionText(ILanguageStatements language)
			{
				return () => string.Empty;
			}

			protected override IDictionary<string, INamed> GetDescriptionParameters()
			{
				return new Dictionary<string, INamed>();
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
				: base(new LocalizedStringConstant(l => string.Empty), new LocalizedStringConstant(l => string.Empty))
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

			protected override Func<string> GetDescriptionText(ILanguageStatements language)
			{
				return () => string.Empty;
			}

			protected override IDictionary<string, INamed> GetDescriptionParameters()
			{
				return new Dictionary<string, INamed>();
			}
		}
	}
}
