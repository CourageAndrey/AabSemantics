using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;

namespace AabSemantics.Modules.Set.Tests.Statements
{
	[TestFixture]
	public class CustomStatementExtensionsTest
	{
		[SetUp]
		public void SetUp()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
		}

		[Test]
		public void GivenGroupStatement_WhenConvertToCustomAndBack_ThenResultLooksEqual()
		{
			// arrange
			var area = ConceptCreationHelper.CreateEmptyConcept();
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			var statement = new GroupStatement(null, area, concept);

			// act
			var custom = statement.ToCustomStatement();
			var restored = custom.ToGroupStatement();

			// assert
			Assert.That(statement, Is.EqualTo(restored));
		}

		[Test]
		public void GivenHasPartStatement_WhenConvertToCustomAndBack_ThenResultLooksEqual()
		{
			// arrange
			var whole = ConceptCreationHelper.CreateEmptyConcept();
			var part = ConceptCreationHelper.CreateEmptyConcept();

			var statement = new HasPartStatement(null, whole, part);

			// act
			var custom = statement.ToCustomStatement();
			var restored = custom.ToHasPartStatement();

			// assert
			Assert.That(statement, Is.EqualTo(restored));
		}

		[Test]
		public void GivenHasSignStatement_WhenConvertToCustomAndBack_ThenResultLooksEqual()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			var statement = new HasSignStatement(null, concept, sign);

			// act
			var custom = statement.ToCustomStatement();
			var restored = custom.ToHasSignStatement();

			// assert
			Assert.That(statement, Is.EqualTo(restored));
		}

		[Test]
		public void GivenSignValueStatement_WhenConvertToCustomAndBack_ThenResultLooksEqual()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept();
			value.WithAttribute(IsValueAttribute.Value);

			var statement = new SignValueStatement(null, concept, sign, value);

			// act
			var custom = statement.ToCustomStatement();
			var restored = custom.ToSignValueStatement();

			// assert
			Assert.That(statement, Is.EqualTo(restored));
		}

		[Test]
		public void TestMutualConversions()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept();
			value.WithAttribute(IsValueAttribute.Value);

			var hasSignStatement = new HasSignStatement(null, concept, sign);
			var signValueStatement = new SignValueStatement(null, concept, sign, value);

			// act
			var customHasSignStatement = hasSignStatement.ToCustomStatement();
			var customSignValueStatement = signValueStatement.ToCustomStatement();

			// assert
			Assert.That(customSignValueStatement.ToHasSignStatement(), Is.EqualTo(hasSignStatement));
			Assert.Throws<KeyNotFoundException>(() => customHasSignStatement.ToSignValueStatement());
		}
	}
}
