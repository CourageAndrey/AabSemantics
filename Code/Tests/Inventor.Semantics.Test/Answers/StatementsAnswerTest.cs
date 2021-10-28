using System.Linq;

using NUnit.Framework;

using Inventor.Semantics;
using Inventor.Semantics.Answers;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Text.Containers;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Test.Answers
{
	[TestFixture]
	public class StatementsAnswerTest
	{
		[Test]
		public void CheckTypedToUntypedAndViceVersaTransformations()
		{
			// arrange

			var concept1 = ConceptCreationHelper.CreateConcept();
			var concept2 = ConceptCreationHelper.CreateConcept();

			var resultStatementsTyped = new IsStatement[]
			{
				new IsStatement(null, concept1, concept2),
				new IsStatement(null, concept2, concept1),
			};

			var resultStatementsUntyped = resultStatementsTyped.OfType<IStatement>().ToArray();

			var text = new UnstructuredContainer();

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
