using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Test.Concepts
{
	[TestFixture]
	public class ConceptConstructorTest
	{
		private const string TestConceptId = "123";

		[Test]
		public void CreateConceptWithAllPropertiesSet()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.AreEqual(TestConceptId, concept.ID);
		}

		[Test]
		public void CreateConceptWithoutId()
		{
			// arrange && act
			var concept = new Concept(null, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.IsFalse(string.IsNullOrEmpty(concept.ID));
		}

		[Test]
		public void CreateConceptWithoutName()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, null, new LocalizedStringVariable());

			// assert
			Assert.IsNotNull(concept.Name);
		}

		[Test]
		public void CreateConceptWithoutHint()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, new LocalizedStringVariable(), null);

			// assert
			Assert.IsNotNull(concept.Hint);
		}
	}
}
