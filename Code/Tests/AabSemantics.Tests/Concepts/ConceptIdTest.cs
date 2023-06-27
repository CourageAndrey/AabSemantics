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
			Assert.AreEqual("Modified_ID", concept.ID);
		}

		[Test]
		public void GivenConcept_WhenTryToSetEmptyId_ThenIdIsUpdatedWithNonEmptyValue()
		{
			// arrange
			var concept = new Concept("Initial_ID");

			// act
			concept.UpdateIdIfAllowed(string.Empty);

			// assert
			Assert.IsFalse(string.IsNullOrEmpty(concept.ID));
			Assert.AreNotEqual("Initial_ID", concept.ID);
		}

		[Test]
		public void GivenConcept_WhenTryToSetNullId_ThenIdIsUpdatedWithNonEmptyValue()
		{
			// arrange
			var concept = new Concept("Initial_ID");

			// act
			concept.UpdateIdIfAllowed(null);

			// assert
			Assert.IsFalse(string.IsNullOrEmpty(concept.ID));
			Assert.AreNotEqual("Initial_ID", concept.ID);
		}
	}
}
