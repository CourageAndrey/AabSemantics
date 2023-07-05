using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Mathematics.Attributes;

namespace AabSemantics.Modules.Mathematics.Tests.Serialization.Xml
{
	[TestFixture]
	public class AttributesTest
	{
		[OneTimeSetUp]
		public void InitializeModules()
		{
			new Boolean.BooleanModule().RegisterMetadata();
			new Classification.ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
		}

		[Test]
		[TestCaseSource(nameof(GetAllAttributes))]
		public void GivenOneAttribute_WhenSerializeDeserialize_ThenSucceed(IAttribute attribute)
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(attribute);

			// act
			var xml = new AabSemantics.Serialization.Xml.Concept(concept);
			var restored = xml.Load();

			// assert
			Assert.AreSame(attribute, restored.Attributes.Single());
		}

		[Test]
		public void GivenAllAttributes_WhenSerializeDeserialize_ThenSucceed()
		{
			// arrange
			var concept = new Concept();
			foreach (var attribute in GetAllAttributes())
			{
				concept.WithAttribute(attribute);
			}

			// act
			var xml = new AabSemantics.Serialization.Xml.Concept(concept);
			var restored = xml.Load();

			// assert
			Assert.IsTrue(restored.Attributes.SequenceEqual(concept.Attributes));
		}

		private static IEnumerable<IAttribute> GetAllAttributes()
		{
			yield return IsComparisonSignAttribute.Value;
		}
	}
}
