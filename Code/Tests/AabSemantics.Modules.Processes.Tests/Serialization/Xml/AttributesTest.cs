using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Processes.Attributes;

namespace AabSemantics.Modules.Processes.Tests.Serialization.Xml
{
	[TestFixture]
	public class AttributesTest
	{
		[OneTimeSetUp]
		public void InitializeModules()
		{
			new Boolean.BooleanModule().RegisterMetadata();
			new Classification.ClassificationModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
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
			Assert.That(restored.Attributes.Single(), Is.SameAs(attribute));
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
			Assert.That(restored.Attributes.SequenceEqual(concept.Attributes), Is.True);
		}

		private static IEnumerable<IAttribute> GetAllAttributes()
		{
			yield return IsProcessAttribute.Value;
			yield return IsSequenceSignAttribute.Value;
		}
	}
}
