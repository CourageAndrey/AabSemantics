using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Statements;

namespace Inventor.Test.Answers
{
	[TestFixture]
	public class StatementsAnswerTest
	{
		[Test]
		public void CheckTypedToUntypedAndViceVersaTransformations()
		{
			// arrange

			var concept1 = TestHelper.CreateConcept();
			var concept2 = TestHelper.CreateConcept();

			var resultStatementsTyped = new IsStatement[]
			{
				new IsStatement(null, concept1, concept2),
				new IsStatement(null, concept2, concept1),
			};

			var resultStatementsUntyped = resultStatementsTyped.OfType<IStatement>().ToArray();

			var text = new Core.Text.FormattedText();

			var explanationStatements = new IStatement[]
			{
				new HasPartStatement(null, concept1, concept2),
				new HasPartStatement(null, concept2, concept1),
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
	}
}
