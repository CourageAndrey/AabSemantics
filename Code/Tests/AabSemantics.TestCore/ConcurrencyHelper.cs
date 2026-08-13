using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

namespace AabSemantics.TestCore
{
	/// <summary>
	/// Runs an operation from many threads at once and reports every failure. Thread safety tests
	/// need this because an exception thrown inside a parallel body would otherwise surface as an
	/// <see cref="AggregateException"/> that hides how many calls actually failed.
	/// </summary>
	public static class ConcurrencyHelper
	{
		/// <summary>Runs an operation concurrently and returns every result.</summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="count">How many concurrent calls to make.</param>
		/// <param name="operation">Operation to run.</param>
		/// <returns>The results, in no particular order.</returns>
		public static List<T> RunConcurrently<T>(Int32 count, Func<T> operation)
		{
			return RunConcurrently(count, _ => operation());
		}

		/// <summary>Runs an operation concurrently, passing each call its own index, and returns every result.</summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="count">How many concurrent calls to make.</param>
		/// <param name="operation">Operation to run; receives the zero-based call index.</param>
		/// <returns>The results, in no particular order.</returns>
		public static List<T> RunConcurrently<T>(Int32 count, Func<Int32, T> operation)
		{
			var results = new ConcurrentBag<T>();
			var errors = new ConcurrentBag<Exception>();

			Parallel.For(0, count, index =>
			{
				try
				{
					results.Add(operation(index));
				}
				catch (Exception error)
				{
					errors.Add(error);
				}
			});

			if (errors.Count > 0)
			{
				Assert.Fail($"{errors.Count} of {count} concurrent calls failed, the first one with {errors.First()}");
			}
			return results.ToList();
		}

		/// <summary>Runs an operation concurrently, ignoring its result.</summary>
		/// <param name="count">How many concurrent calls to make.</param>
		/// <param name="operation">Operation to run.</param>
		public static void RunConcurrently(Int32 count, Action operation)
		{
			RunConcurrently(count, () => { operation(); return true; });
		}
	}
}
