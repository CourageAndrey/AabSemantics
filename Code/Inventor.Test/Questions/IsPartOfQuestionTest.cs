﻿using System.Linq;

using NUnit.Framework;

using Inventor.Core.Answers;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;
using Inventor.Test.Sample;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class IsPartOfQuestionTest
	{
		[Test]
		public void ReturnFalseIfNotFound()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Base_Vehicle);

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
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Vehicle_Car);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var statement = (HasPartStatement) answer.Explanation.Statements.Single();
			Assert.IsTrue(statement.Whole == semanticNetwork.Vehicle_Car && statement.Part == semanticNetwork.Part_Engine);
		}
	}
}
