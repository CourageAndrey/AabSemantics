﻿using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Attributes;

namespace AabSemantics.Tests.Concepts
{
	[TestFixture]
	public class AttributesExtensionTest
	{
		[Test]
		public void GivenConcept_WhenAddOrRemoveAttributes_ThenConceptAttributesChange()
		{
			var concept = new Concept();

			// 0. no attributes added
			Assert.That(concept.Attributes.Count, Is.EqualTo(0));
			Assert.That(concept.HasAttribute<IsValueAttribute>(), Is.False);

			// 1. add IsValueAttribute
			concept.WithAttribute(IsValueAttribute.Value);
			Assert.That(concept.Attributes.Count, Is.EqualTo(1));
			Assert.That(concept.HasAttribute<IsValueAttribute>(), Is.True);

			// 2. remove all attributes
			concept.WithoutAttributes();
			Assert.That(concept.Attributes.Count, Is.EqualTo(0));
			Assert.That(concept.HasAttribute<IsValueAttribute>(), Is.False);
		}
	}
}
