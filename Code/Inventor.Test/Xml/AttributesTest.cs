using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Base;

namespace Inventor.Test.Xml
{
	[TestFixture]
	public class AttributesTest
	{
		[Test]
		[TestCaseSource(nameof(getAllAttributes))]
		public void OneAttribute(IAttribute attribute)
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(attribute);

			var attributeRepository = new AttributeRepository();

			// act
			var xml = new Core.Xml.Concept(concept, attributeRepository);
			var restored = xml.Load();

			// assert
			Assert.AreSame(attribute, restored.Attributes.Single());
		}

		[Test]
		public void AllAttributes()
		{
			// arrange
			var concept = new Concept();
			foreach (var attribute in getAllAttributes())
			{
				concept.WithAttribute(attribute);
			}

			var attributeRepository = new AttributeRepository();

			// act
			var xml = new Core.Xml.Concept(concept, attributeRepository);
			var restored = xml.Load();

			// assert
			Assert.IsTrue(restored.Attributes.SequenceEqual(concept.Attributes));
		}

		[Test]
		public void UnknownAttribute()
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(new WrongAttribute());

			var attributeRepository = new AttributeRepository();

			// act & assert
			Assert.Throws<NotSupportedException>(() => new Core.Xml.Concept(concept, attributeRepository));
		}

		private IEnumerable<IAttribute> getAllAttributes()
		{
			var repository = new AttributeRepository();
			return repository.AttributeDefinitions.Values.Select(a => a.AttributeValue);
		}

		private class WrongAttribute : IAttribute
		{ }
	}
}
