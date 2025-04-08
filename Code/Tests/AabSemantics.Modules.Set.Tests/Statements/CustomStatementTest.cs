using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Set.Tests.Statements
{
	[TestFixture]
	public class CustomStatementTest
	{
		[SetUp]
		public void SetUp()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
		}

		[Test]
		public void GivenCustomHasPartStatement_WhenDescribe_ThenReturnTheSameAsOriginal()
		{
			// arrange
			var whole = "w".CreateConceptByName();
			var part = "p".CreateConceptByName();

			// act
			var statement = new HasPartStatement(null, whole, part);
			var custom = statement.ToCustomStatement();

			// assert
			statement.CheckAllDescriptionsAgainst(custom, "Composition");
		}

		[Test]
		public void GivenCustomGroupStatement_WhenDescribe_ThenReturnTheSameAsOriginal()
		{
			// arrange
			var area = "a".CreateConceptByName();
			var concept = "c".CreateConceptByName();

			// act
			var statement = new GroupStatement(null, area, concept);
			var custom = statement.ToCustomStatement();

			// assert
			statement.CheckAllDescriptionsAgainst(custom, "Subject Area");
		}

		[Test]
		public void GivenCustomHasSignStatement_WhenDescribe_ThenReturnTheSameAsOriginal()
		{
			// arrange
			var concept = "c".CreateConceptByName();
			var sign = "s".CreateConceptByName().WithAttribute(IsSignAttribute.Value);

			// act
			var statement = new HasSignStatement(null, concept, sign);
			var custom = statement.ToCustomStatement();

			// assert
			statement.CheckAllDescriptionsAgainst(custom, "Has Sign");
		}

		[Test]
		public void GivenCustomSignValueStatement_WhenDescribe_ThenReturnTheSameAsOriginal()
		{
			// arrange
			var concept = "c".CreateConceptByName();
			var sign = "s".CreateConceptByName().WithAttribute(IsSignAttribute.Value);
			var value = "v".CreateConceptByName().WithAttribute(IsValueAttribute.Value);

			// act
			var statement = new SignValueStatement(null, concept, sign, value);
			var custom = statement.ToCustomStatement();

			// assert
			statement.CheckAllDescriptionsAgainst(custom, "Sign Value");
		}
	}
}
