using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Mutations;

namespace AabSemantics.Tests.Mutations
{
	[TestFixture]
	public class ProductionsTest
	{
		[Test]
		public void GivenNoLookupPattern_WhenTryToCreate_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new Production(null, knowledgeStructure => { }));
		}

		[Test]
		public void GivenNoSearchMethod_WhenTryToCreate_ThenFail()
		{
			// arrange
			var conceptSearchPattern = new ConceptSearchPattern(concept => true);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new Production(conceptSearchPattern, null));
		}

		[Test]
		public void GivenNoSemanticNetwork_WhenMutate_ThenThrowArgumentNullException()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => MutationHelper.Mutate(null, Array.Empty<IMutation>()));
		}

		[Test]
		public void GivenNoMutations_WhenMutate_ThenThrowArgumentNullException()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => semanticNetwork.Mutate(null));
		}

		[Test]
		public void GivenConstantMutationsList_WhenMutate_ThenWorkWhenPossible()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			semanticNetwork.Concepts.Add(1.CreateConcept());
			semanticNetwork.Concepts.Add(2.CreateConcept());
			semanticNetwork.Concepts.Add(3.CreateConcept());

			var conceptSearchPattern = new ConceptSearchPattern(concept => true);
			var production = new Production(
				conceptSearchPattern,
				knowledgeStructure => { semanticNetwork.Concepts.Remove((IConcept) knowledgeStructure.Knowledge[conceptSearchPattern]); });

			// act
			var appliedMutations = semanticNetwork.Mutate(new IMutation[] { production });

			// assert
			Assert.AreEqual(3, appliedMutations.Count);
			Assert.IsTrue(appliedMutations.All(mutation => mutation == production));
		}
	}
}
