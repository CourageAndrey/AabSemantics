using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;

namespace AabSemantics.Tests.Serialization.Xml
{
	[TestFixture]
	public class AttributesTest
	{
		[OneTimeSetUp]
		public void InitializeModules()
		{
			new Modules.Boolean.BooleanModule().RegisterMetadata();
			new Modules.Classification.ClassificationModule().RegisterMetadata();
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

		[Test]
		public void GivenUnknownAttribute_WhenTryToSerialize_ThenFail()
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(new WrongAttribute());

			// act & assert
			Assert.Throws<NotSupportedException>(() => new AabSemantics.Serialization.Xml.Concept(concept));
		}

		private static IEnumerable<IAttribute> GetAllAttributes()
		{
			yield return Modules.Boolean.Attributes.IsBooleanAttribute.Value;
			yield return Modules.Boolean.Attributes.IsValueAttribute.Value;
		}

		private class WrongAttribute : IAttribute
		{ }
	}
}
