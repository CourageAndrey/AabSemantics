using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Serialization.Json;
using AabSemantics.Serialization.Xml;
using AabSemantics.Statements;
using AabSemantics.TestCore;
using AabSemantics.Utils;

namespace AabSemantics.Tests.Concurrency
{
	/// <summary>
	/// Thread safety of read-only work against a single shared semantic network — the way a server
	/// hosting one knowledge base uses the engine. Every test here asserts two things at once: that
	/// nothing throws, and that concurrency did not change the result.
	/// </summary>
	[TestFixture]
	public class ConcurrentSemanticNetworkTest
	{
		private const Int32 CallCount = 200;
		private const Int32 ChainLength = 8;

		#region Questions

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskSameQuestionConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out var root, out var leaf);

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, () => (BooleanAnswer) semanticNetwork.Ask().IfIs(leaf, root));

			// assert
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(answer => answer.Result), Is.True);
			Assert.That(answers.All(answer => !answer.IsEmpty), Is.True);
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskDifferentQuestionsConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out var root, out _);
			var concepts = semanticNetwork.Concepts.ToList();

			// act: each call asks about another level of the same hierarchy
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, index =>
			{
				var descendant = concepts[1 + index % (concepts.Count - 1)];
				return (BooleanAnswer) semanticNetwork.Ask().IfIs(descendant, root);
			});

			// assert: every concept below the root is a descendant of it
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(answer => answer.Result), Is.True);
		}

		[Test]
		public void GivenTransitiveQuestion_WhenAskedConcurrently_ThenNestedQuestionsDoNotInterfere()
		{
			// arrange: a chain long enough that answering requires several levels of nested questions
			var semanticNetwork = CreateHierarchy(ChainLength, out var root, out var leaf);
			var expected = ((BooleanAnswer) semanticNetwork.Ask().IfIs(leaf, root)).Explanation.Statements.Count;

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, () => (BooleanAnswer) semanticNetwork.Ask().IfIs(leaf, root));

			// assert: every concurrent answer collected the same proof as the sequential one
			Assert.That(answers.All(answer => answer.Result), Is.True);
			Assert.That(answers.Select(answer => answer.Explanation.Statements.Count).Distinct(), Is.EqualTo(new[] { expected }));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskQuestionsConcurrently_ThenNoQuestionContextLeaks()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out var root, out var leaf);
			Assert.That(semanticNetwork.Context.Children.Count, Is.EqualTo(0), "the network is expected to start without question contexts");

			// act
			ConcurrencyHelper.RunConcurrently(CallCount, () => semanticNetwork.Ask().IfIs(leaf, root));

			// assert: every question context has been disposed and detached from its parent
			Assert.That(semanticNetwork.Context.Children.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskQuestionsConcurrently_ThenNoStatementsAreAdded()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out var root, out var leaf);
			int statementsBefore = semanticNetwork.Statements.GetCount();

			// act
			ConcurrencyHelper.RunConcurrently(CallCount, () => semanticNetwork.Ask().IfIs(leaf, root));

			// assert: asking must not mutate the knowledge base
			Assert.That(semanticNetwork.Statements.GetCount(), Is.EqualTo(statementsBefore));
		}

		#endregion

		#region Whole-network operations

		[Test]
		public void GivenSharedSemanticNetwork_WhenCheckConsistencyConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out _, out _);

			// act
			var results = ConcurrencyHelper.RunConcurrently(50, () => semanticNetwork.CheckConsistency().ToString());

			// assert
			Assert.That(results.Distinct().Count(), Is.EqualTo(1));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenDescribeRulesConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out _, out _);

			// act
			var results = ConcurrencyHelper.RunConcurrently(50, () => semanticNetwork.DescribeRules().ToString());

			// assert
			Assert.That(results.Distinct().Count(), Is.EqualTo(1));
		}

		#endregion

		#region Serialization

		[Test]
		public void GivenSharedSemanticNetwork_WhenSerializeToXmlConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out _, out _);

			// act: the serializer cache is shared, so every call reuses one XmlSerializer instance
			var results = ConcurrencyHelper.RunConcurrently(100, () => new AabSemantics.Serialization.Xml.SemanticNetwork(semanticNetwork).SerializeToXmlString());

			// assert
			Assert.That(results.Distinct().Count(), Is.EqualTo(1));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenSerializeToJsonConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(ChainLength, out _, out _);

			// act
			var results = ConcurrencyHelper.RunConcurrently(100, () => new AabSemantics.Serialization.Json.SemanticNetwork(semanticNetwork).SerializeToJsonString());

			// assert
			Assert.That(results.Distinct().Count(), Is.EqualTo(1));
		}

		#endregion

		#region Metadata and contexts

		[Test]
		public void GivenMetadataRepositories_WhenReadConcurrently_ThenEveryDefinitionIsResolved()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(3, out _, out _);
			var statement = semanticNetwork.Statements.First();

			// act: the metadata repositories are process-wide, so concurrent readers share them
			var names = ConcurrencyHelper.RunConcurrently(CallCount, () =>
			{
				var statementDefinition = Repositories.Statements.Definitions.GetSuitable(statement);
				var answerDefinition = Repositories.Answers.Definitions.GetSuitable(AabSemantics.Answers.Answer.CreateUnknown());
				Assert.That(answerDefinition, Is.Not.Null);
				return statementDefinition.GetName(Language.Default);
			});

			// assert
			Assert.That(names.Distinct().Count(), Is.EqualTo(1));
		}

		[Test]
		public void GivenContext_WhenActiveContextsAreFirstReadConcurrently_ThenEveryCallerSeesWholeHierarchy()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(3, out var root, out var leaf);

			using (var questionContext = semanticNetwork.Context.CreateQuestionContext(new IsQuestion(leaf, root)))
			{
				// act: ActiveContexts is computed lazily, so the very first readers race to build it
				var hierarchies = ConcurrencyHelper.RunConcurrently(CallCount, () => questionContext.ActiveContexts);

				// assert: whoever wins the race, every caller sees the question, network and system contexts
				Assert.That(hierarchies.Select(hierarchy => hierarchy.Count).Distinct(), Is.EqualTo(new[] { 3 }));
				Assert.That(hierarchies.All(hierarchy => hierarchy.Contains(questionContext)), Is.True);
				Assert.That(hierarchies.All(hierarchy => hierarchy.Contains(semanticNetwork.Context)), Is.True);
			}
		}

		#endregion

		#region Helpers

		/// <summary>Builds a network holding a single "is a" chain, so that answering requires transitive inference.</summary>
		private static SemanticNetwork CreateHierarchy(Int32 length, out IConcept root, out IConcept leaf)
		{
			var semanticNetwork = new SemanticNetwork(Language.Default);

			var concepts = System.Linq.Enumerable.Range(0, length)
				.Select(index => $"concept-{index}".CreateConceptByName())
				.ToList();
			foreach (var concept in concepts)
			{
				semanticNetwork.Concepts.Add(concept);
			}
			for (int index = 0; index < length - 1; index++)
			{
				semanticNetwork.DeclareThat(concepts[index]).IsAncestorOf(concepts[index + 1]);
			}

			root = concepts.First();
			leaf = concepts.Last();
			return semanticNetwork;
		}


		#endregion
	}
}
