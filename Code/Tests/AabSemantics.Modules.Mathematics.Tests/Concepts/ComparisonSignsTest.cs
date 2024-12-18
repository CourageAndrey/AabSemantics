using System;

using NUnit.Framework;

using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Mathematics.Concepts;

namespace AabSemantics.Modules.Mathematics.Tests.Concepts
{
	[TestFixture]
	public class ComparisonSignsTest
	{
		[Test]
		public void GivenNonComparisonSigns_WhenTryToCallComparisonSignExtensions_ThenFail()
		{
			foreach (var concept in LogicalValues.All)
			{
				Assert.Throws<InvalidOperationException>(() => { concept.Contradicts(ComparisonSigns.IsEqualTo); });
				Assert.Throws<InvalidOperationException>(() => { ComparisonSigns.IsEqualTo.Contradicts(concept); });

				Assert.Throws<InvalidOperationException>(() => { concept.Revert(); });

				Assert.Throws<InvalidOperationException>(() => { concept.CanBeReverted(); });

				Assert.Throws<InvalidOperationException>(() => { ComparisonSigns.CompareThreeValues(concept, ComparisonSigns.IsEqualTo); });
				Assert.Throws<InvalidOperationException>(() => { ComparisonSigns.CompareThreeValues(ComparisonSigns.IsEqualTo, concept); });
			}
		}

		[Test]
		public void GivenComparisonSigns_WhenRevertTwice_ThenGetTheSame()
		{
			foreach (var sign in ComparisonSigns.All)
			{
				Assert.That(sign.Revert().Revert(), Is.SameAs(sign));
			}
		}
	}
}
