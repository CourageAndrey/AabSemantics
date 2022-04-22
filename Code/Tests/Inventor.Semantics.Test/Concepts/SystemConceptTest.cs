using System;

using NUnit.Framework;

using Inventor.Semantics.Concepts;

namespace Inventor.Semantics.Test.Concepts
{
	[TestFixture]
	public class SystemConceptTest
	{
		[Test]
		public void CannotChangeIdOfSystemConcept()
		{
			// arrange
			var concept = new SystemConcept("1", null, null);

			// act & assert
			Assert.Throws<NotSupportedException>(() => concept.UpdateIdIfAllowed("2"));
		}

		[Test]
		public void SettingTheSameIdDoesNotChangeSystemConcept()
		{
			// arrange
			var concept = new SystemConcept("1", null, null);

			// act & assert
			Assert.DoesNotThrow(() => concept.UpdateIdIfAllowed("1"));
		}
	}
}
