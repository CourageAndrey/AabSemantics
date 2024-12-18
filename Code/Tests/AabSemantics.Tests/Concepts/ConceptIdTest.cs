using NUnit.Framework;

using AabSemantics.Concepts;

namespace AabSemantics.Tests.Concepts
{
	[TestFixture]
	public class ConceptIdTest
	{
		[Test]
		public void GivenConcept_WhenSetNewId_ThenIdIsChanged()
		{
			// arrange
			var concept = new Concept("Initial_ID");

			// act
			concept.UpdateIdIfAllowed("Modified_ID");

			// assert
			Assert.That(concept.ID, Is.EqualTo("Modified_ID"));
		}

		[Test]
		public void GivenConcept_WhenTryToSetEmptyId_ThenIdIsUpdatedWithNonEmptyValue()
		{
			// arrange
			var concept = new Concept("Initial_ID");

			// act
			concept.UpdateIdIfAllowed(string.Empty);

			// assert
			Assert.That(string.IsNullOrEmpty(concept.ID), Is.False);
			Assert.That(concept.ID, Is.Not.EqualTo("Initial_ID"));
		}

		[Test]
		public void GivenConcept_WhenTryToSetNullId_ThenIdIsUpdatedWithNonEmptyValue()
		{
			// arrange
			var concept = new Concept("Initial_ID");

			// act
			concept.UpdateIdIfAllowed(null);

			// assert
			Assert.That(string.IsNullOrEmpty(concept.ID), Is.False);
			Assert.That(concept.ID, Is.Not.EqualTo("Initial_ID"));
		}
	}
}
