﻿using System;

using NUnit.Framework;

using Inventor.Core.Concepts;

namespace Inventor.Test.Concepts
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
	}
}