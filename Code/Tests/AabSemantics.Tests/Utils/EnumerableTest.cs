using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class EnumerableTest
	{
		[Test]
		public void GivenMissingElement_WhenIndexOf_ThenReturnNegative()
		{
			// arrange
			var list = GenerateTestSequence();
			IEnumerable<TestItem> sequence = list;

			// act
			int index = sequence.IndexOf(new TestItem(int.MinValue));

			// assert
			Assert.That(index, Is.LessThan(0));
		}

		[Test]
		public void GivenElementWithinList_WhenIndexOf_ThenReturnIndex()
		{
			// arrange
			var list = GenerateTestSequence();
			IEnumerable<TestItem> sequence = list;

			for (int i = _testSequenceMinimum; i <= _testSequenceMaximum; i++)
			{
				// act
				int index = sequence.IndexOf(list[i]);

				// assert
				Assert.That(index, Is.EqualTo(i));
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

		private static List<TestItem> GenerateTestSequence()
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
