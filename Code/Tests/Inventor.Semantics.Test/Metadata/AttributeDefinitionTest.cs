using System;

using NUnit.Framework;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Modules.Processes.Attributes;

namespace Inventor.Semantics.Test.Metadata
{
	[TestFixture]
	public class AttributeDefinitionTest
	{
		[Test]
		public void ImpossibleToCreateDefinitionWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(
				null,
				IsValueAttribute.Value,
				language => language.Culture));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithInvalidType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new AttributeDefinition(
				typeof(CheckStatementQuestion),
				IsValueAttribute.Value,
				language => language.Culture));
			Assert.Throws<ArgumentException>(() => new AttributeDefinition(
				typeof(TestAbstractAttribute),
				IsValueAttribute.Value,
				language => language.Culture));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutValue()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(
				typeof(IsValueAttribute),
				null,
				language => language.Culture));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithWrongValue()
		{
			// act & assert
			Assert.Throws<InvalidCastException>(() => new AttributeDefinition(
				typeof(IsValueAttribute),
				IsProcessAttribute.Value,
				language => language.Culture));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutNameGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(
				typeof(IsValueAttribute),
				IsValueAttribute.Value,
				null));
		}

		[Test]
		public void CheckName()
		{
			// arrange
			var definition = new AttributeDefinition(
				typeof(IsValueAttribute),
				IsValueAttribute.Value,
				language => language.Culture);

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.AreEqual(Language.Default.Culture, name);
		}

		[Test]
		public void ValidateNone()
		{
			// arrange
			var language = Language.Default;

			// act & assert
			Assert.AreEqual(language.Attributes.None, AttributeDefinition.None.GetName(language));
		}

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
			var wrongValue = Modules.Boolean.Attributes.IsBooleanAttribute.Value;
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
			Assert.AreSame(type, attribute.Type);
			Assert.AreSame(value, attribute.Value);
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
			Assert.AreSame(type, attribute.Type);
			Assert.AreSame(value, attribute.Value);
			Assert.AreEqual(_attributeName, attribute.GetName(null));
		}

		private const string _attributeName = "TeStAtTrIbUtE";

		private class TestAttributeChecked : IAttribute
		{ }

		private class TestAttributeDerived : TestAttributeChecked
		{ }

		private class TestAttributeXml : Semantics.Serialization.Xml.Attribute
		{
			public override IAttribute Load()
			{
				throw new NotImplementedException();
			}
		}

		private abstract class TestAbstractAttribute : Attribute
		{ }
	}
}
