using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;
using AabSemantics.TestCore;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class CustomStatementTest
	{
		[SetUp]
		public void SetUp()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();

			Repositories.RegisterCustomStatement(
				"test",
				new[] { "concept1", "concept2" },
				l => string.Empty,
				l => string.Empty,
				l => string.Empty);
		}

		[Test]
		public void GivenIsStatement_WhenConvertToCustomAndBack_ThenResultLooksEqual()
		{
			// arrange
			var ancestor = ConceptCreationHelper.CreateEmptyConcept();
			var descendant = ConceptCreationHelper.CreateEmptyConcept();

			var statement = new IsStatement(null, ancestor, descendant);

			// act
			var custom = statement.ToCustomStatement();
			var restored = custom.ToIsStatement();

			// assert
			Assert.That(statement, Is.EqualTo(restored));
		}

		[Test]
		public void GivenNullOther_WhenEquals_ThenReturnFalse()
		{
			IConcept concept1 = 1.CreateConceptByObject(),
					 concept2 = 2.CreateConceptByObject();
			var statement = new CustomStatement(null, "test", new Dictionary<string, IConcept>
			{
				{ "concept1", concept1 },
				{ "concept2", concept2 },
			});

			// act && assert
			Assert.That(statement.Equals(null), Is.False);
		}

		[Test]
		public void GivenCustomIsStatement_WhenDescribe_ThenReturnTheSameAsOriginal()
		{
			// arrange
			var ancestor = "a".CreateConceptByName();
			var descendant = "d".CreateConceptByName();

			// act
			var statement = new IsStatement(null, ancestor, descendant);
			var custom = statement.ToCustomStatement();

			// assert
			statement.CheckAllDescriptionsAgainst(custom, "Classification");
		}
	}
}
