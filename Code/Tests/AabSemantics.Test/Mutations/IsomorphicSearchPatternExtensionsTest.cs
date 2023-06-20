using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Mutations;

namespace AabSemantics.Test.Mutations
{
	[TestFixture]
	public class IsomorphicSearchPatternExtensionsTest
	{
		[Test]
		public void GivenNoMatchesWhenDoesMatchThenReturnFalse()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var matches = new List<KnowledgeStructure>();
			var searchPattern = new TestIsomorphicSearchPattern(matches);

			// act
			bool result = semanticNetwork.DoesMatch(searchPattern);

			// assert
			Assert.IsFalse(result);
		}

		[Test]
		public void GivenMatchesWhenDoesMatchThenReturnTrue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var matches = new List<KnowledgeStructure>();
			var searchPattern = new TestIsomorphicSearchPattern(matches);
			matches.Add(new KnowledgeStructure(semanticNetwork, searchPattern, new Dictionary<IsomorphicSearchPattern, IKnowledge>()));
			matches.Add(new KnowledgeStructure(semanticNetwork, searchPattern, new Dictionary<IsomorphicSearchPattern, IKnowledge>()));

			// act
			bool result = semanticNetwork.DoesMatch(searchPattern);

			// assert
			Assert.IsTrue(result);
		}

		[Test]
		public void GivenNoMatchesWhenFindFirstMatchThenReturnNull()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var matches = new List<KnowledgeStructure>();
			var searchPattern = new TestIsomorphicSearchPattern(matches);

			// act
			var result = semanticNetwork.FindFirstMatch(searchPattern);

			// assert
			Assert.IsNull(result);
		}

		[Test]
		public void GivenMatchesWhenFindFirstMatchThenReturnFirstMatch()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var matches = new List<KnowledgeStructure>();
			var searchPattern = new TestIsomorphicSearchPattern(matches);
			matches.Add(new KnowledgeStructure(semanticNetwork, searchPattern, new Dictionary<IsomorphicSearchPattern, IKnowledge>()));
			matches.Add(new KnowledgeStructure(semanticNetwork, searchPattern, new Dictionary<IsomorphicSearchPattern, IKnowledge>()));

			// act
			var result = semanticNetwork.FindFirstMatch(searchPattern);

			// assert
			Assert.AreSame(matches[0], result);
		}

		private class TestIsomorphicSearchPattern : IsomorphicSearchPattern
		{
			private readonly IEnumerable<KnowledgeStructure> _matches;

			public TestIsomorphicSearchPattern(IEnumerable<KnowledgeStructure> matches)
			{
				_matches = matches;
			}

			public override IEnumerable<KnowledgeStructure> FindMatches(ISemanticNetwork semanticNetwork)
			{
				return _matches;
			}
		}
	}
}
