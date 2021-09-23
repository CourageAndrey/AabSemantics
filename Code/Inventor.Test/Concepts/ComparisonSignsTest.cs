using System;

using NUnit.Framework;

using Inventor.Core.Concepts;
using Inventor.Mathematics.Concepts;

namespace Inventor.Test.Concepts
{
	[TestFixture]
	public class ComparisonSignsTest
	{
		[Test]
		public void OnlyComparisonSignsSuit()
		{
			foreach (var concept in SystemConcepts.GetAll())
			{
				if (!ComparisonSigns.All.Contains(concept))
				{
					Assert.Throws<InvalidOperationException>(() => { concept.Contradicts(ComparisonSigns.IsEqualTo); });
					Assert.Throws<InvalidOperationException>(() => { ComparisonSigns.IsEqualTo.Contradicts(concept); });

					Assert.Throws<InvalidOperationException>(() => { ComparisonSigns.Revert(concept); });

					Assert.Throws<InvalidOperationException>(() => { concept.CanBeReverted(); });

					Assert.Throws<InvalidOperationException>(() => { ComparisonSigns.CompareThreeValues(concept, ComparisonSigns.IsEqualTo); });
					Assert.Throws<InvalidOperationException>(() => { ComparisonSigns.CompareThreeValues(ComparisonSigns.IsEqualTo, concept); });
				}
			}
		}

		[Test]
		public void DoubleReversionDoNothing()
		{
			foreach (var sign in ComparisonSigns.All)
			{
				Assert.AreSame(sign, ComparisonSigns.Revert(ComparisonSigns.Revert(sign)));
			}
		}
	}
}
