using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;
using AabSemantics.Text.Containers;

namespace AabSemantics.Tests.Answers
{
	[TestFixture]
	public class StatementsAnswerTest
	{
		[Test]
		public void GivenStatementsAnswer_WhenUpcastAndDowncast_ThenAnswerLooksTheSame()
		{
			// arrange
			var concept1 = 1.CreateConcept();
			var concept2 = 2.CreateConcept();

			var resultStatementsTyped = new IsStatement[]
			{
				new IsStatement(null, concept1, concept2),
				new IsStatement(null, concept2, concept1),
			};

			var resultStatementsUntyped = resultStatementsTyped.OfType<IStatement>().ToArray();

			var text = new UnstructuredContainer();

			var explanationStatements = new IStatement[]
			{
				new TestStatement(concept1, concept2),
				new TestStatement(concept2, concept1),
			};

			var typedAnswer = new StatementsAnswer<IsStatement>(resultStatementsTyped, text, new Explanation(explanationStatements));
			var untypedAnswer = new StatementsAnswer(resultStatementsUntyped, text, new Explanation(explanationStatements));

			// act
			var genericAnswer = typedAnswer.MakeGeneric();
			var explicitAnswer = untypedAnswer.MakeExplicit<IsStatement>();

			// assert
			Assert.IsTrue(typedAnswer.Result.SequenceEqual(explicitAnswer.Result));
			Assert.IsTrue(typedAnswer.Explanation.Statements.SequenceEqual(explicitAnswer.Explanation.Statements));

			Assert.IsTrue(untypedAnswer.Result.SequenceEqual(genericAnswer.Result));
			Assert.IsTrue(untypedAnswer.Explanation.Statements.SequenceEqual(genericAnswer.Explanation.Statements));
		}

		private class TestStatement : Statement<TestStatement>
		{
			private readonly IConcept _concept1, _concept2;

			public TestStatement(IConcept concept1, IConcept concept2)
				: base(null, new LocalizedStringConstant(l => null), new LocalizedStringConstant(l => null))
			{
				_concept1 = concept1;
				_concept2 = concept2;
			}

			public override IEnumerable<IConcept> GetChildConcepts()
			{
				throw new System.NotImplementedException();
			}

			protected override string GetDescriptionTrueText(ILanguage language)
			{
				throw new System.NotImplementedException();
			}

			protected override string GetDescriptionFalseText(ILanguage language)
			{
				throw new System.NotImplementedException();
			}

			protected override string GetDescriptionQuestionText(ILanguage language)
			{
				throw new System.NotImplementedException();
			}

			protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
			{
				throw new System.NotImplementedException();
			}

			public override bool Equals(TestStatement other)
			{
				return _concept1 == other._concept1 && _concept2 == other._concept2;
			}
		}
	}
}
