using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Modules.Set.Questions;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Questions
{
	[TestFixture]
	public class IsSubjectAreaQuestionTest
	{
		[Test]
		public void ReturnFalseIfNotFound()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Base_Vehicle, semanticNetwork.SubjectArea_Numbers);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnTrueIfFound()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Base_Vehicle, semanticNetwork.SubjectArea_Transport);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var statement = (GroupStatement) answer.Explanation.Statements.Single();
			Assert.IsTrue(statement.Concept == semanticNetwork.Base_Vehicle && statement.Area == semanticNetwork.SubjectArea_Transport);
		}
	}
}
