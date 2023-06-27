using System;

using NUnit.Framework;

using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Mutations;

namespace AabSemantics.Tests.Mutations
{
	[TestFixture]
	public class StatementConceptFilterTest
	{
		[Test]
		public void GivenNoConceptSelector_WhenTryToCreate_ThenFail()
		{
			// arrange
			var conceptFilter = new ConceptSearchPattern(concept => true);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new StatementConceptFilter<IsStatement>(null, conceptFilter));
		}

		[Test]
		public void GivenNoConceptFilter_WhenTryToCreate_ThenFail()
		{
			// arrange
			var conceptSelector = new StatementConceptSelector<IsStatement>(statement => null);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new StatementConceptFilter<IsStatement>(conceptSelector, null));
		}

		[Test]
		public void GivenAllCorrectParameters_WhenCreate_ThenSucceed()
		{
			// arrange
			var conceptFilter = new ConceptSearchPattern(concept => true);
			var conceptSelector = new StatementConceptSelector<IsStatement>(statement => null);

			// act
			var filter = new StatementConceptFilter<IsStatement>(conceptSelector, conceptFilter);

			// assert
			Assert.AreSame(conceptFilter, filter.ConceptFilter);
			Assert.AreSame(conceptSelector, filter.ConceptSelector);
		}
	}
}
