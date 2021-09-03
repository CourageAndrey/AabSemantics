using System.Linq;

using NUnit.Framework;

using Inventor.Core.Answers;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;
using Inventor.Test.Sample;

namespace Inventor.Test.Questions
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
