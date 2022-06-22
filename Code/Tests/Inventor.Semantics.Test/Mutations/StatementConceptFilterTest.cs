using System;

using NUnit.Framework;

using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Mutations;

namespace Inventor.Semantics.Test.Mutations
{
	[TestFixture]
	public class StatementConceptFilterTest
	{
		[Test]
		public void ImpossibleToCreateWithoutConceptSelector()
		{
			// arrange
			var conceptFilter = new ConceptSearchPattern(concept => true);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new StatementConceptFilter<IsStatement>(null, conceptFilter));
		}

		[Test]
		public void ImpossibleToCreateWithoutConceptFilter()
		{
			// arrange
			var conceptSelector = new StatementConceptSelector<IsStatement>(statement => null);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new StatementConceptFilter<IsStatement>(conceptSelector, null));
		}

		[Test]
		public void GivenAllCorrectParametersWhenCreateThenCreate()
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
