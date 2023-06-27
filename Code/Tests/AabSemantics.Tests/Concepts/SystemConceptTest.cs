using System;

using NUnit.Framework;

using AabSemantics.Concepts;

namespace AabSemantics.Tests.Concepts
{
	[TestFixture]
	public class SystemConceptTest
	{
		[Test]
		public void GivenSystemConcept_WhenTryToChangeId_ThenFail()
		{
			// arrange
			var concept = new SystemConcept("1", null, null);

			// act & assert
			Assert.Throws<NotSupportedException>(() => concept.UpdateIdIfAllowed("2"));
		}

		[Test]
		public void GivenSystemConcept_WhenTryChangeIdWithTheSameValue_ThenNothingHappens()
		{
			// arrange
			var concept = new SystemConcept("1", null, null);

			// act & assert
			Assert.DoesNotThrow(() => concept.UpdateIdIfAllowed("1"));
		}
	}
}
