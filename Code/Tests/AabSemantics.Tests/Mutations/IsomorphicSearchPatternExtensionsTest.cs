﻿using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Mutations;

namespace AabSemantics.Tests.Mutations
{
	[TestFixture]
	public class IsomorphicSearchPatternExtensionsTest
	{
		[Test]
		public void GivenNoMatches_WhenDoesMatch_ThenReturnFalse()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var matches = new List<KnowledgeStructure>();
			var searchPattern = new TestIsomorphicSearchPattern(matches);

			// act
			bool result = semanticNetwork.DoesMatch(searchPattern);

			// assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void GivenMatches_WhenDoesMatch_ThenReturnTrue()
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
			Assert.That(result, Is.True);
		}

		[Test]
		public void GivenNoMatches_WhenFindFirstMatch_ThenReturnNull()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var matches = new List<KnowledgeStructure>();
			var searchPattern = new TestIsomorphicSearchPattern(matches);

			// act
			var result = semanticNetwork.FindFirstMatch(searchPattern);

			// assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GivenMatches_WhenFindFirstMatch_ThenReturnFirstMatch()
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
			Assert.That(matches[0], Is.SameAs(result));
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
