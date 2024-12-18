using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Mutations;

namespace AabSemantics.Tests.Mutations
{
	[TestFixture]
	public class KnowledgeStructureTest
	{
		[Test]
		public void GivenSemanticNetwork_WhenTryToCreate_ThenFail()
		{
			// arrange
			var searchPattern = new ConceptSearchPattern(concept => true);
			var knowledge = new Dictionary<IsomorphicSearchPattern, IKnowledge>();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new KnowledgeStructure(null, searchPattern, knowledge));
		}

		[Test]
		public void GivenSearchPattern_WhenTryToCreate_ThenFail()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var knowledge = new Dictionary<IsomorphicSearchPattern, IKnowledge>();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new KnowledgeStructure(semanticNetwork, null, knowledge));
		}

		[Test]
		public void GivenKnowledge_WhenTryToCreate_ThenFail()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var searchPattern = new ConceptSearchPattern(concept => true);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new KnowledgeStructure(semanticNetwork, searchPattern, null));
		}

		[Test]
		public void GivenAllCorrectParameters_WhenCreate_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var searchPattern = new ConceptSearchPattern(concept => true);
			var knowledge = new Dictionary<IsomorphicSearchPattern, IKnowledge>();

			// act && assert
			KnowledgeStructure knowledgeStructure = null;
			Assert.DoesNotThrow(() => knowledgeStructure = new KnowledgeStructure(semanticNetwork, searchPattern, knowledge));
			Assert.That(knowledgeStructure.SemanticNetwork, Is.SameAs(semanticNetwork));
			Assert.That(knowledgeStructure.SearchPattern, Is.SameAs(searchPattern));
			Assert.That(knowledgeStructure.Knowledge, Is.SameAs(knowledge));
		}
	}
}
