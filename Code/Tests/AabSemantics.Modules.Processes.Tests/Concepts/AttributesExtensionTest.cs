using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Processes.Attributes;

namespace AabSemantics.Modules.Processes.Tests.Concepts
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
			Assert.That(concept.HasAttribute<IsSequenceSignAttribute>(), Is.False);

			// 1. add IsValueAttribute
			concept.WithAttribute(IsValueAttribute.Value);
			Assert.That(concept.Attributes.Count, Is.EqualTo(1));
			Assert.That(concept.HasAttribute<IsValueAttribute>(), Is.True);
			Assert.That(concept.HasAttribute<IsSequenceSignAttribute>(), Is.False);

			// 2. add IsSequenceSignAttribute
			concept.WithAttribute(IsSequenceSignAttribute.Value);
			Assert.That(concept.Attributes.Count, Is.EqualTo(2));
			Assert.That(concept.HasAttribute<IsValueAttribute>(), Is.True);
			Assert.That(concept.HasAttribute<IsSequenceSignAttribute>(), Is.True);

			// 3. remove all attributes
			concept.WithoutAttributes();
			Assert.That(concept.Attributes.Count, Is.EqualTo(0));
			Assert.That(concept.HasAttribute<IsValueAttribute>(), Is.False);
			Assert.That(concept.HasAttribute<IsSequenceSignAttribute>(), Is.False);
		}
	}
}
