using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;

namespace AabSemantics.Tests.Concepts
{
	[TestFixture]
	public class ConceptConstructorTest
	{
		private const string TestConceptId = "123";

		[Test]
		public void GivenAllPropertiesSet_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.AreEqual(TestConceptId, concept.ID);
		}

		[Test]
		public void GivenNoId_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(null, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.IsFalse(string.IsNullOrEmpty(concept.ID));
		}

		[Test]
		public void GivenNoName_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, null, new LocalizedStringVariable());

			// assert
			Assert.IsNotNull(concept.Name);
		}

		[Test]
		public void GivenNoHint_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, new LocalizedStringVariable(), null);

			// assert
			Assert.IsNotNull(concept.Hint);
		}

		[Test]
		public void GivenDifferentIdAndName_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = TestConceptId.CreateConcept(new string(TestConceptId.Reverse().ToArray()));

			// assert
			Assert.AreNotEqual(concept.ID, concept.Hint);
		}
	}
}
