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
			Assert.AreEqual(0, concept.Attributes.Count);
			Assert.IsFalse(concept.HasAttribute<IsValueAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsSequenceSignAttribute>());

			// 1. add IsValueAttribute
			concept.WithAttribute(IsValueAttribute.Value);
			Assert.AreEqual(1, concept.Attributes.Count);
			Assert.IsTrue(concept.HasAttribute<IsValueAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsSequenceSignAttribute>());

			// 2. add IsSequenceSignAttribute
			concept.WithAttribute(IsSequenceSignAttribute.Value);
			Assert.AreEqual(2, concept.Attributes.Count);
			Assert.IsTrue(concept.HasAttribute<IsValueAttribute>());
			Assert.IsTrue(concept.HasAttribute<IsSequenceSignAttribute>());

			// 3. remove all attributes
			concept.WithoutAttributes();
			Assert.AreEqual(0, concept.Attributes.Count);
			Assert.IsFalse(concept.HasAttribute<IsValueAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsSequenceSignAttribute>());
		}
	}
}
