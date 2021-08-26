using System;

using NUnit.Framework;

using Inventor.Core;

namespace Inventor.Test
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

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(null, value, nameGetter));
		}

		[Test]
		public void GivenNullValueWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			Func<ILanguage, string> nameGetter = language => _attributeName;

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(type, null, nameGetter));
		}

		[Test]
		public void GivenNullNameGetterWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var value = new TestAttributeChecked();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(type, value, null));
		}

		[Test]
		public void GivenWrongTypeValueWhenCreateAttributeDefinitionThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var wrongValue = Core.Attributes.IsBooleanAttribute.Value;
			Func<ILanguage, string> nameGetter = language => _attributeName;

			// act && assert
			Assert.Throws<InvalidCastException>(() => new AttributeDefinition(type, wrongValue, nameGetter));
		}

		[Test]
		public void GivenCertainTypeValueWhenCreateAttributeDefinitionThenSucceed()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var value = new TestAttributeChecked();
			Func<ILanguage, string> nameGetter = language => _attributeName;

			// act
			var attribute = new AttributeDefinition(type, value, nameGetter);

			// assert
			Assert.AreSame(type, attribute.AttributeType);
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

			// act
			var attribute = new AttributeDefinition(type, value, nameGetter);

			// assert
			Assert.AreSame(type, attribute.AttributeType);
			Assert.AreSame(value, attribute.AttributeValue);
			Assert.AreEqual(_attributeName, attribute.GetName(null));
		}

		private const string _attributeName = "TeStAtTrIbUtE";

		private class TestAttributeChecked : IAttribute
		{ }

		private class TestAttributeDerived : TestAttributeChecked
		{ }
	}
}
