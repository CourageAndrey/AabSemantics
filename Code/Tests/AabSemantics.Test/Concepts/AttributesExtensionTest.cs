using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Processes.Attributes;

namespace AabSemantics.Test.Concepts
{
	[TestFixture]
	public class AttributesExtensionTest
	{
		[Test]
		public void AttributesFlow()
		{
			var concept = new Concept();

			// 0. no attributes added
			Assert.IsFalse(concept.HasAttribute<IsValueAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsComparisonSignAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsSequenceSignAttribute>());

			// 1. add IsValueAttribute
			concept.WithAttribute(IsValueAttribute.Value);
			Assert.IsTrue(concept.HasAttribute<IsValueAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsComparisonSignAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsSequenceSignAttribute>());

			// 2. add IsComparisonSignAttribute
			concept.WithAttribute(IsComparisonSignAttribute.Value);
			Assert.IsTrue(concept.HasAttribute<IsValueAttribute>());
			Assert.IsTrue(concept.HasAttribute<IsComparisonSignAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsSequenceSignAttribute>());

			// 3. add IsSequenceSignAttribute
			concept.WithAttribute(IsSequenceSignAttribute.Value);
			Assert.IsTrue(concept.HasAttribute<IsValueAttribute>());
			Assert.IsTrue(concept.HasAttribute<IsComparisonSignAttribute>());
			Assert.IsTrue(concept.HasAttribute<IsSequenceSignAttribute>());

			// 4. remove all attributes
			concept.WithoutAttributes();
			Assert.IsFalse(concept.HasAttribute<IsValueAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsComparisonSignAttribute>());
			Assert.IsFalse(concept.HasAttribute<IsSequenceSignAttribute>());
		}
	}
}
