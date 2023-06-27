using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class EnumerableTest
	{
		[Test]
		public void GivenMissingElementWhenIndexOfThenReturnNegative()
		{
			// arrange
			var list = generateTestSequence();
			IEnumerable<TestItem> sequence = list;

			// act
			int index = sequence.IndexOf(new TestItem(int.MinValue));

			// assert
			Assert.Less(index, 0);
		}

		[Test]
		public void GivenElementWithinListWhenIndexOfThenReturnIndex()
		{
			// arrange
			var list = generateTestSequence();
			IEnumerable<TestItem> sequence = list;

			for (int i = _testSequenceMinimum; i <= _testSequenceMaximum; i++)
			{
				// act
				int index = sequence.IndexOf(list[i]);

				// assert
				Assert.AreEqual(index, i);
			}
		}

		private const int _testSequenceSize = 10;
		private const int _testSequenceMinimum = 0;
		private const int _testSequenceMaximum = _testSequenceSize - _testSequenceMinimum - 1;

		private class TestItem
		{
			public int Number
			{ get; }

			public TestItem(int number)
			{
				Number = number;
			}
		}

		private static List<TestItem> generateTestSequence()
		{
			var sequence = new List<TestItem>();

			for (int i = _testSequenceMinimum; i < _testSequenceSize - _testSequenceMinimum; i++)
			{
				sequence.Add(new TestItem(i));
			}

			return sequence;
		}
	}
}
