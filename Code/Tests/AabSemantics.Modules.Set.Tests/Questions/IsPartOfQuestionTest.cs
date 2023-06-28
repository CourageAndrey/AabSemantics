using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Test.Sample;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class IsPartOfQuestionTest
	{
		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnFalse()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Base_Vehicle);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnTrue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Vehicle_Car);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var statement = (HasPartStatement) answer.Explanation.Statements.Single();
			Assert.IsTrue(statement.Whole == semanticNetwork.Vehicle_Car && statement.Part == semanticNetwork.Part_Engine);
		}
	}
}
