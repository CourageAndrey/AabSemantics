using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Mutations;

namespace AabSemantics.Tests.Mutations
{
	[TestFixture]
	public class ConceptSearchPatternTest
	{
		[Test]
		public void GivenNoConceptFilter_WhenTryToCreateConceptSearchPattern_ThenThrow()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ConceptSearchPattern(null));
		}

		[Test]
		public void GivenAllConceptSearchPattern_WhenFind_ThenReturnAll()
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
