using System;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Boolean.Questions;

namespace AabSemantics.Tests.Metadata
{
	[TestFixture]
	public class AttributeDefinitionTest
	{
		[Test]
		public void GivenNoType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(
				null,
				IsValueAttribute.Value,
				language => language.Culture));
		}

		[Test]
		public void GivenInvalidType_WhenTryToCreate_ThenFail()
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
		public void GivenNoValue_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(
				typeof(IsValueAttribute),
				null,
				language => language.Culture));
		}

		[Test]
		public void GivenNoNameGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeDefinition(
				typeof(IsValueAttribute),
				IsValueAttribute.Value,
				null));
		}

		[Test]
		public void GivenCorrectDefinition_WhenGetName_ThenReturnIt()
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
		public void GivenNoneAttribute_WhenGetName_ThenReturnNoneName()
		{
			// arrange
			var language = Language.Default;

			// act & assert
			Assert.AreEqual(language.Attributes.None, AttributeDefinition.None.GetName(language));
		}

		[Test]
		public void GivenWrongTypeValue_WhenTryToCreate_ThenFail()
		{
			// arrange
			var type = typeof(TestAttributeChecked);
			var wrongValue = IsBooleanAttribute.Value;
			Func<ILanguage, string> nameGetter = language => _attributeName;

			// act && assert
			Assert.Throws<InvalidCastException>(() => new AttributeDefinition(type, wrongValue, nameGetter));
		}

		[Test]
		public void GivenCertainTypeValue_WhenCreate_ThenSucceed()
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
		public void GivenDerivedTypeValue_WhenCreate_ThenSucceed()
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

		private abstract class TestAbstractAttribute : Attribute
		{ }
	}
}
