using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.TestCore;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Tests.Concurrency
{
	/// <summary>
	/// Thread safety of the set module against a single shared semantic network. Its recursive
	/// questions walk the classification hierarchy and ask a nested question per ancestor, so a
	/// single call already runs several question contexts concurrently.
	/// </summary>
	[TestFixture]
	public class ConcurrentSetTest
	{
		private const Int32 CallCount = 200;

		#region Recursive questions

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskRecursiveHasSignQuestionConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange: the sign is declared on the base concept, so answering requires recursion
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateSetTestData();
			var expected = (BooleanAnswer) new HasSignQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType, true)
				.Ask(semanticNetwork.SemanticNetwork.Context);
			Assert.That(expected.Result, Is.True, "the test data is expected to let the sign be inherited");

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, () => (BooleanAnswer) new HasSignQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType, true)
				.Ask(semanticNetwork.SemanticNetwork.Context));

			// assert
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(answer => answer.Result), Is.True);
			Assert.That(answers.Select(answer => answer.Explanation.Statements.Count).Distinct(), Is.EqualTo(new[] { expected.Explanation.Statements.Count }));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskSignValueQuestionConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateSetTestData();
			var expected = (ConceptAnswer) new SignValueQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType)
				.Ask(semanticNetwork.SemanticNetwork.Context);

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, () => (ConceptAnswer) new SignValueQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType)
				.Ask(semanticNetwork.SemanticNetwork.Context));

			// assert: the resolved value must be the very same concept every time
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.Select(answer => answer.Result).Distinct(), Is.EqualTo(new[] { expected.Result }));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskDifferentQuestionsConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateSetTestData();
			var questions = new Func<IQuestion>[]
			{
				() => new HasSignQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType, true),
				() => new HasSignsQuestion(semanticNetwork.Vehicle_Car, true),
				() => new SignValueQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType),
				() => new EnumerateSignsQuestion(semanticNetwork.Base_Vehicle, true),
				() => new IsPartOfQuestion(semanticNetwork.Part_Engine, semanticNetwork.Base_Vehicle),
				() => new EnumeratePartsQuestion(semanticNetwork.Base_Vehicle),
				() => new IsSubjectAreaQuestion(semanticNetwork.Base_Vehicle, semanticNetwork.SubjectArea_Transport),
				() => new DescribeSubjectAreaQuestion(semanticNetwork.SubjectArea_Transport),
			};
			var expected = questions.Select(create => create().Ask(semanticNetwork.SemanticNetwork.Context).IsEmpty).ToList();

			// act: mixing question kinds makes the calls contend for the same statements
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, index =>
			{
				int kind = index % questions.Length;
				return new { Kind = kind, IsEmpty = questions[kind]().Ask(semanticNetwork.SemanticNetwork.Context).IsEmpty };
			});

			// assert: each question kind keeps answering the way it does sequentially
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(answer => answer.IsEmpty == expected[answer.Kind]), Is.True);
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskComparisonQuestionsConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange: comparing two concepts reads both hierarchies and all their sign values
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateSetTestData();
			var expectedCommon = new GetCommonQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Motorcycle)
				.Ask(semanticNetwork.SemanticNetwork.Context).Description.ToString();
			var expectedDifference = new GetDifferencesQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Motorcycle)
				.Ask(semanticNetwork.SemanticNetwork.Context).Description.ToString();

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, index =>
			{
				bool common = index % 2 == 0;
				var question = common
					? (IQuestion) new GetCommonQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Motorcycle)
					: new GetDifferencesQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Motorcycle);
				return new { Expected = common ? expectedCommon : expectedDifference, Actual = question.Ask(semanticNetwork.SemanticNetwork.Context).Description.ToString() };
			});

			// assert
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(pair => pair.Actual == pair.Expected), Is.True);
		}

		#endregion

		#region Contexts and consistency

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskRecursiveQuestionsConcurrently_ThenNoQuestionContextLeaks()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateSetTestData();

			// act
			ConcurrencyHelper.RunConcurrently(CallCount, () => new HasSignQuestion(semanticNetwork.Vehicle_JetFighter, semanticNetwork.Sign_MotorType, true)
				.Ask(semanticNetwork.SemanticNetwork.Context));

			// assert: nested question contexts are disposed along with their parents
			Assert.That(semanticNetwork.SemanticNetwork.Context.Children.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenCheckConsistencyConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange: the set module contributes four statement types, each with its own checks
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateSetTestData();

			// act
			var results = ConcurrencyHelper.RunConcurrently(50, () => semanticNetwork.SemanticNetwork.CheckConsistency().ToString());

			// assert
			Assert.That(results.Distinct().Count(), Is.EqualTo(1));
		}

		[Test]
		public void GivenSignStatements_WhenReadSignsAndValuesConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateSetTestData();
			var statements = semanticNetwork.SemanticNetwork.Statements.ToList();
			int expectedSigns = HasSignStatement.GetSignsAsync(statements, semanticNetwork.Vehicle_Car, true).Await().Count;
			int expectedValues = SignValueStatement.GetSignValues(statements, semanticNetwork.Vehicle_Car, true).Count;

			// act
			var results = ConcurrencyHelper.RunConcurrently(CallCount, () => new
			{
				Signs = HasSignStatement.GetSignsAsync(statements, semanticNetwork.Vehicle_Car, true).Await().Count,
				Values = SignValueStatement.GetSignValues(statements, semanticNetwork.Vehicle_Car, true).Count,
			});

			// assert
			Assert.That(results.All(result => result.Signs == expectedSigns), Is.True);
			Assert.That(results.All(result => result.Values == expectedValues), Is.True);
		}

		#endregion
	}
}
