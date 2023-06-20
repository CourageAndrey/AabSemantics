using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Mutations;

namespace AabSemantics.Test.Mutations
{
	[TestFixture]
	public class ConceptSearchPatternTest
	{
		[Test]
		public void GivenNoConceptFilterWhenTryToCreateConceptSearchPatternThenThrow()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ConceptSearchPattern(null));
		}

		[Test]
		public void GivenAllConceptSearchPatternWhenFindThenReturnAll()
		{
			// arrange
			var concepts = new List<IConcept>();
			for (int i = 0; i < 10; i++)
			{
				concepts.Add(i.CreateConcept());
			}

			// act
			var filteredConcepts = ConceptSearchPattern.All.FindConcepts(concepts).ToList();

			// assert
			Assert.IsTrue(filteredConcepts.SequenceEqual(concepts));
		}
	}
}
