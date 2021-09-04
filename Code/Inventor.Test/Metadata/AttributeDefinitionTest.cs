using System;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Metadata;

namespace Inventor.Test.Metadata
{
	[TestFixture]
	public class AttributeDefinitionTest
	{
		[Test]
		public void GivenNullTypeWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var value = new TestAttributeChecked();
			Func<ILanguage, string> nameGetter = language => _attributeName;
			var xml = new Core.Xml.IsValueAttribute();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(null, value, nameGetter, xml));
		}

		[Test]
		public void GivenNullValueWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			Func<ILanguage, string> nameGetter = language => _attributeName;
			var xml = new TestAttributeXml();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(type, null, nameGetter, xml));
		}

		[Test]
		public void GivenNullNameGetterWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var value = new TestAttributeChecked();
			var xml = new TestAttributeXml();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(type, value, null, xml));
		}

		[Test]
		public void GivenNullXmlWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var value = new TestAttributeChecked();
			Func<ILanguage, string> nameGetter = language => _attributeName;

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(type, value, nameGetter, null));
		}

		[Test]
		public void GivenWrongTypeValueWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var wrongValue = Core.Attributes.IsBooleanAttribute.Value;
			Func<ILanguage, string> nameGetter = language => _attributeName;
			var xml = new TestAttributeXml();

			// act && assert
			Assert.Throws<InvalidCastException>(() => new AttributeDefinition(type, wrongValue, nameGetter, xml));
		}

		[Test]
		public void GivenCertainTypeValueWhenCreateAttributeDefinitionThenSucceed()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var value = new TestAttributeChecked();
			Func<ILanguage, string> nameGetter = language => _attributeName;
			var xml = new TestAttributeXml();

			// act
			var attribute = new AttributeDefinition(type, value, nameGetter, xml);

			// assert
			Assert.AreSame(type, attribute.Type);
			Assert.AreSame(value, attribute.AttributeValue);
			Assert.AreEqual(_attributeName, attribute.GetName(null));
		}

		[Test]
		public void GivenDerivedTypeValueWhenCreateAttributeDefinitionThenSucceed()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var value = new TestAttributeDerived();
			Func<ILanguage, string> nameGetter = language => _attributeName;
			var xml = new TestAttributeXml();

			// act
			var attribute = new AttributeDefinition(type, value, nameGetter, xml);

			// assert
			Assert.AreSame(type, attribute.Type);
			Assert.AreSame(value, attribute.AttributeValue);
			Assert.AreEqual(_attributeName, attribute.GetName(null));
		}

		private const string _attributeName = "TeStAtTrIbUtE";

		private class TestAttributeChecked : IAttribute
		{ }

		private class TestAttributeDerived : TestAttributeChecked
		{ }

		private class TestAttributeXml : Core.Xml.Attribute
		{
			public override IAttribute Load()
			{
				throw new NotImplementedException();
			}
		}
	}
}
