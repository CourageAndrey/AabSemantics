using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Classification.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class CustomStatementExtensionsTest
	{
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
	}
}
