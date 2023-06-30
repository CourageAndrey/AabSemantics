using System;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Concepts;

namespace AabSemantics.Tests.Concepts
{
	[TestFixture]
	public class LogicalValuesTest
	{
		[Test]
		public void GivenNonLogicalValues_WhenTryToCallLogicalValueExtensions_ThenFail()
		{
			// arrange
			var concept = "test".CreateConcept();

			// act && assert
			Assert.Throws<InvalidOperationException>(() => { concept.ToBoolean(); });
			Assert.Throws<InvalidOperationException>(() => { concept.Invert(); });
		}

		[Test]
		public void GivenLogicalValues_WhenRevertTwice_ThenGetTheSame()
		{
			foreach (var value in LogicalValues.All)
			{
				Assert.AreSame(value, value.Invert().Invert());
			}
		}

		[Test]
		public void GivenLogicalValues_WhenConvertToBooleanAndBack_ThenGetTheSame()
		{
			foreach (var value in LogicalValues.All)
			{
				Assert.AreSame(value, value.ToBoolean().ToLogicalValue());
			}

			foreach (var value in new[] { true, false })
			{
				Assert.AreEqual(value, value.ToLogicalValue().ToBoolean());
			}
		}

		[Test]
		public void GivenFakeLogicalValue_WhenTryInvert_ThenReturnTheSame()
		{
			// arrange
			var testConcept = "test".CreateConcept();
			LogicalValues.All.Add(testConcept);

			// act && assert
			try
			{
				Assert.AreSame(testConcept, testConcept.Invert());
			}
			finally
			{
				LogicalValues.All.Remove(testConcept);
			}
		}

		[Test]
		public void GivenFakeLogicalValue_WhenTryConvertToBoolean_ThenFail()
		{
			// arrange
			var testConcept = "test".CreateConcept();
			LogicalValues.All.Add(testConcept);

			// act && assert
			try
			{
				Assert.Throws<NotSupportedException>(() => testConcept.ToBoolean());
			}
			finally
			{
				LogicalValues.All.Remove(testConcept);
			}
		}
	}
}
