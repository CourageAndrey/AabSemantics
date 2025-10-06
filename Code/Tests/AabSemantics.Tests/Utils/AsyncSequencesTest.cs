using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class AsyncSequencesTest
	{
		private async Task CheckMethodWorks<T>(int count, Func<LongEnumerable, Task<T>> method, Action<T> assert)
		{
			// arrange
			var enumerable = new LongEnumerable(count);

			// act & assert
			var task = method(enumerable);

			Thread.Sleep(100);
			Assert.That(task.IsCompleted, Is.False);

			enumerable.Return();

			var result = await task;
			Assert.That(task.IsCompleted, Is.True);
			assert(result);
		}

		private void CheckMethodFails<T, ErrorT>(int count, Func<LongEnumerable, Task<T>> method)
			where ErrorT : Exception
		{
			// arrange
			var enumerable = new LongEnumerable(count);

			// act & assert
			var task = method(enumerable);

			Thread.Sleep(100);
			Assert.That(task.IsCompleted, Is.False);

			enumerable.Return();

			Assert.ThrowsAsync<ErrorT>(async () => await task);
		}

		[Test]
		public async Task GivenLongSequence_CheckFirstAsync()
		{
			CheckMethodFails<int, InvalidOperationException>(0, enumerable => enumerable.FirstAsync());
			await CheckMethodWorks<int>(3, enumerable => enumerable.FirstAsync(), result => Assert.That(result, Is.EqualTo(1)));

			await CheckMethodWorks<int>(3, enumerable => enumerable.FirstAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.FirstAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			CheckMethodFails<int, InvalidOperationException>(0, enumerable => enumerable.FirstAsync(i => i == 1));
			CheckMethodFails<int, InvalidOperationException>(3, enumerable => enumerable.FirstAsync(i => i == 0));
		}

		[Test]
		public async Task GivenLongSequence_CheckFirstOrDefaultAsync()
		{
			await CheckMethodWorks<int>(0, enumerable => enumerable.FirstOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.FirstOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(1)));

			await CheckMethodWorks<int>(3, enumerable => enumerable.FirstOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.FirstOrDefaultAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks<int>(0, enumerable => enumerable.FirstOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.FirstOrDefaultAsync(i => i == 0), result => Assert.That(result, Is.EqualTo(0)));
		}

		[Test]
		public async Task GivenLongSequence_CheckLastAsync()
		{
			CheckMethodFails<int, InvalidOperationException>(0, enumerable => enumerable.LastAsync());
			await CheckMethodWorks<int>(3, enumerable => enumerable.LastAsync(), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks<int>(3, enumerable => enumerable.LastAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.LastAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			CheckMethodFails<int, InvalidOperationException>(0, enumerable => enumerable.LastAsync(i => i == 1));
			CheckMethodFails<int, InvalidOperationException>(3, enumerable => enumerable.LastAsync(i => i == 0));
		}

		[Test]
		public async Task GivenLongSequence_CheckLastOrDefaultAsync()
		{
			await CheckMethodWorks<int>(0, enumerable => enumerable.LastOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.LastOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks<int>(3, enumerable => enumerable.LastOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.LastOrDefaultAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks<int>(0, enumerable => enumerable.LastOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks<int>(3, enumerable => enumerable.LastOrDefaultAsync(i => i == 0), result => Assert.That(result, Is.EqualTo(0)));
		}

		[Test]
		public async Task GivenLongSequence_CheckAnyAsync()
		{
			await CheckMethodWorks<bool>(0, enumerable => enumerable.AnyAsync(), result => Assert.That(result, Is.False));
			await CheckMethodWorks<bool>(3, enumerable => enumerable.AnyAsync(), result => Assert.That(result, Is.True));

			await CheckMethodWorks<bool>(3, enumerable => enumerable.AnyAsync(i => i == 0), result => Assert.That(result, Is.False));
			await CheckMethodWorks<bool>(3, enumerable => enumerable.AnyAsync(i => i == 2), result => Assert.That(result, Is.True));
		}

		[Test]
		public async Task GivenLongSequence_CheckAllAsync()
		{
			await CheckMethodWorks<bool>(0, enumerable => enumerable.AllAsync(i => i < 3), result => Assert.That(result, Is.True));
			await CheckMethodWorks<bool>(3, enumerable => enumerable.AllAsync(i => i == 3), result => Assert.That(result, Is.False));
			await CheckMethodWorks<bool>(3, enumerable => enumerable.AllAsync(i => i < 4), result => Assert.That(result, Is.True));
		}

		[Test]
		public async Task GivenLongSequence_CheckToArrayAsync()
		{
			await CheckMethodWorks<int[]>(0, enumerable => enumerable.ToArrayAsync(), result =>
			{
				Assert.That(result.Length, Is.EqualTo(0));
			});
			await CheckMethodWorks<int[]>(3, enumerable => enumerable.ToArrayAsync(), result =>
			{
				Assert.That(result.Length, Is.EqualTo(3));
				Assert.That(result[0], Is.EqualTo(1));
				Assert.That(result[1], Is.EqualTo(2));
				Assert.That(result[2], Is.EqualTo(3));
			});
		}

		[Test]
		public async Task GivenLongSequence_CheckToListAsync()
		{
			await CheckMethodWorks<List<int>>(0, enumerable => enumerable.ToListAsync(), result =>
			{
				Assert.That(result.Count, Is.EqualTo(0));
			});
			await CheckMethodWorks<List<int>>(3, enumerable => enumerable.ToListAsync(), result =>
			{
				Assert.That(result.Count, Is.EqualTo(3));
				Assert.That(result[0], Is.EqualTo(1));
				Assert.That(result[1], Is.EqualTo(2));
				Assert.That(result[2], Is.EqualTo(3));
			});
		}

		private class LongEnumerable : IEnumerable<int>
		{
			private bool sleep = true;
			private readonly int _count;

			public LongEnumerable(int count)
			{
				_count = count;
			}

			public IEnumerator<int> GetEnumerator()
			{
				while (sleep)
				{
					Thread.Sleep(50);
				}

				for (int i = 1; i <= _count; i++)
				{
					yield return i;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Return()
			{
				sleep = false;
			}
		}
	}
}
