using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Modules.Classification.Statements;
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
			var concept1 = 1.CreateConceptByObject();
			var concept2 = 2.CreateConceptByObject();

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
			Assert.That(typedAnswer.Result.SequenceEqual(explicitAnswer.Result), Is.True);
			Assert.That(typedAnswer.Explanation.Statements.SequenceEqual(explicitAnswer.Explanation.Statements), Is.True);

			Assert.That(untypedAnswer.Result.SequenceEqual(genericAnswer.Result), Is.True);
			Assert.That(untypedAnswer.Explanation.Statements.SequenceEqual(genericAnswer.Explanation.Statements), Is.True);
		}

		private class TestStatement : TestCore.TestStatement
		{
			private readonly IConcept _concept1, _concept2;

			public TestStatement(IConcept concept1, IConcept concept2)
				: base()
			{
				_concept1 = concept1;
				_concept2 = concept2;
			}

			public override IEnumerable<IConcept> GetChildConcepts()
			{
				yield return _concept1;
				yield return _concept2;
			}

			public override bool Equals(TestCore.TestStatement other)
			{
				var typed = (TestStatement) other;
				return _concept1 == typed._concept1 && _concept2 == typed._concept2;
			}
		}
	}
}
