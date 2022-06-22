using System;
using System.Collections.Generic;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Mutations;

namespace Inventor.Semantics.Test.Mutations
{
	[TestFixture]
	public class KnowledgeStructureTest
	{
		[Test]
		public void ImpossibleToCreateWithoutSemanticNetwork()
		{
			// arrange
			var searchPattern = new ConceptSearchPattern(concept => true);
			var knowledge = new Dictionary<IsomorphicSearchPattern, IKnowledge>();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new KnowledgeStructure(null, searchPattern, knowledge));
		}

		[Test]
		public void ImpossibleToCreateWithoutSearchPattern()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var knowledge = new Dictionary<IsomorphicSearchPattern, IKnowledge>();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new KnowledgeStructure(semanticNetwork, null, knowledge));
		}

		[Test]
		public void ImpossibleToCreateWithoutKnowledge()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var searchPattern = new ConceptSearchPattern(concept => true);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new KnowledgeStructure(semanticNetwork, searchPattern, null));
		}

		[Test]
		public void GivenAllCorrectParametersWhenCreateThenCreate()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var searchPattern = new ConceptSearchPattern(concept => true);
			var knowledge = new Dictionary<IsomorphicSearchPattern, IKnowledge>();

			// act && assert
			KnowledgeStructure knowledgeStructure = null;
			Assert.DoesNotThrow(() => knowledgeStructure = new KnowledgeStructure(semanticNetwork, searchPattern, knowledge));
			Assert.AreSame(semanticNetwork, knowledgeStructure.SemanticNetwork);
			Assert.AreSame(searchPattern, knowledgeStructure.SearchPattern);
			Assert.AreSame(knowledge, knowledgeStructure.Knowledge);
		}
	}
}
