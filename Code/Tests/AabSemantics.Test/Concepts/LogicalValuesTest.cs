using System;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Concepts;

namespace AabSemantics.Test.Concepts
{
	[TestFixture]
	public class LogicalValuesTest
	{
		[Test]
		public void OnlyLogicalValuesSuit()
		{
			foreach (var concept in SystemConcepts.GetAll())
			{
				if (!LogicalValues.All.Contains(concept))
				{
					Assert.Throws<InvalidOperationException>(() => { concept.ToBoolean(); });
					Assert.Throws<InvalidOperationException>(() => { concept.Invert(); });
				}
			}
		}

		[Test]
		public void DoubleInversionDoNothing()
		{
			foreach (var value in LogicalValues.All)
			{
				Assert.AreSame(value, value.Invert().Invert());
			}
		}

		[Test]
		public void ConversionToBooleanAndBackDoNothing()
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
		public void ImpossibleToInvertIfNotBoolean()
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
		public void ImpossibleToConvertToBooleanIfNotBoolean()
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
