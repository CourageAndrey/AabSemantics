using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class SynchronizedCollectionTest
	{
		#region ICollection semantics

		[Test]
		public void GivenNoWrappedCollection_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new SynchronizedCollection<int>(null));
		}

		[Test]
		public void GivenNewCollection_WhenCheckState_ThenItIsEmptyAndWritable()
		{
			// arrange
			var collection = new SynchronizedCollection<String>();

			// assert
			Assert.That(collection.Count, Is.EqualTo(0));
			Assert.That(collection.IsReadOnly, Is.False);
		}

		[Test]
		public void GivenItems_WhenAddAndRemove_ThenStateIsCorrect()
		{
			// arrange
			var collection = new SynchronizedCollection<String>();

			// act
			collection.Add("A");
			collection.Add("B");

			// assert
			Assert.That(collection.Count, Is.EqualTo(2));
			Assert.That(collection.Contains("A"), Is.True);
			Assert.That(collection.Contains("Z"), Is.False);

			// act & assert
			Assert.That(collection.Remove("A"), Is.True);
			Assert.That(collection.Remove("A"), Is.False);
			Assert.That(collection.Count, Is.EqualTo(1));
			Assert.That(collection.Contains("A"), Is.False);
		}

		[Test]
		public void GivenItems_WhenClear_ThenBecomeEmpty()
		{
			// arrange
			var collection = new SynchronizedCollection<String> { "A", "B", "C" };

			// act
			collection.Clear();

			// assert
			Assert.That(collection.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenItems_WhenCopyTo_ThenAllItemsAreCopied()
		{
			// arrange
			var collection = new SynchronizedCollection<String> { "A", "B" };
			var array = new String[3];

			// act
			collection.CopyTo(array, 1);

			// assert
			Assert.That(array, Is.EqualTo(new[] { null, "A", "B" }));
		}

		[Test]
		public void GivenItems_WhenEnumerate_ThenAllItemsAreReturned()
		{
			// arrange
			var collection = new SynchronizedCollection<String> { "A", "B", "C" };

			// act
			var typed = collection.ToList();
			var untyped = ((IEnumerable) collection).Cast<String>().ToList();

			// assert
			Assert.That(typed, Is.EqualTo(new[] { "A", "B", "C" }));
			Assert.That(untyped, Is.EqualTo(new[] { "A", "B", "C" }));
		}

		[Test]
		public void GivenWrappedCollection_WhenModifyThroughWrapper_ThenWrappedOneIsModified()
		{
			// arrange
			var wrapped = new List<String> { "A" };
			var collection = new SynchronizedCollection<String>(wrapped);

			// act
			collection.Add("B");
			collection.Remove("A");

			// assert
			Assert.That(wrapped, Is.EqualTo(new[] { "B" }));
		}

		#endregion

		#region Snapshot enumeration

		[Test]
		public void GivenEnumerationInProgress_WhenModifyCollection_ThenEnumerationSurvives()
		{
			// arrange
			var collection = new SynchronizedCollection<Int32> { 1, 2, 3 };

			// act: a plain List would throw InvalidOperationException here
			var seen = new List<Int32>();
			foreach (var item in collection)
			{
				seen.Add(item);
				collection.Add(item + 100);
			}

			// assert: the enumeration went over the snapshot taken when it started
			Assert.That(seen, Is.EqualTo(new[] { 1, 2, 3 }));
			Assert.That(collection.Count, Is.EqualTo(6));
		}

		[Test]
		public void GivenCollection_WhenCreateSnapshot_ThenItIsIndependentCopy()
		{
			// arrange
			var collection = new SynchronizedCollection<Int32> { 1, 2 };

			// act
			var snapshot = collection.CreateCopy();
			collection.Add(3);

			// assert
			Assert.That(snapshot, Is.EqualTo(new[] { 1, 2 }));
			Assert.That(collection.Count, Is.EqualTo(3));
		}

		#endregion

		#region Thread safety

		private const Int32 ThreadCount = 8;
		private const Int32 ItemsPerThread = 500;

		[Test]
		public void GivenManyThreads_WhenAddConcurrently_ThenNoItemIsLost()
		{
			// arrange
			var collection = new SynchronizedCollection<Int32>();

			// act
			Parallel.For(0, ThreadCount, thread =>
			{
				for (int i = 0; i < ItemsPerThread; i++)
				{
					collection.Add(thread * ItemsPerThread + i);
				}
			});

			// assert: an unsynchronized List would lose items or corrupt its size here
			Assert.That(collection.Count, Is.EqualTo(ThreadCount * ItemsPerThread));
			Assert.That(collection.Distinct().Count(), Is.EqualTo(ThreadCount * ItemsPerThread));
		}

		[Test]
		public void GivenManyThreads_WhenAddAndRemoveConcurrently_ThenCollectionBecomesEmpty()
		{
			// arrange
			var collection = new SynchronizedCollection<String>();

			// act: every thread adds its own items and then removes them back
			Parallel.For(0, ThreadCount, thread =>
			{
				var own = System.Linq.Enumerable.Range(0, ItemsPerThread).Select(i => $"{thread}-{i}").ToList();
				foreach (var item in own)
				{
					collection.Add(item);
				}
				foreach (var item in own)
				{
					Assert.That(collection.Remove(item), Is.True);
				}
			});

			// assert
			Assert.That(collection.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenConcurrentWriter_WhenEnumerate_ThenNeverThrows()
		{
			// arrange
			var collection = new SynchronizedCollection<Int32>(System.Linq.Enumerable.Range(0, 100).ToList());
			var stop = new CancellationTokenSource();

			var writer = Task.Run(() =>
			{
				int i = 1000;
				while (!stop.IsCancellationRequested)
				{
					collection.Add(i++);
					collection.Remove(i - 1);
				}
			});

			// act & assert: enumerating a plain List under such a writer throws InvalidOperationException
			try
			{
				for (int attempt = 0; attempt < 2000; attempt++)
				{
					Assert.DoesNotThrow(() => { foreach (var item in collection) { } });
				}
			}
			finally
			{
				stop.Cancel();
				writer.Wait(TimeSpan.FromSeconds(5));
			}
		}

		#endregion
	}
}
