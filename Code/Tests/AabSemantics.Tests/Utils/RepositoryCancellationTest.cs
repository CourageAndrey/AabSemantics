using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class RepositoryCancellationTest
	{
		[Test]
		public void GivenCancelledToken_WhenReadInMemoryRepository_ThenThrow()
		{
			// arrange
			var collection = new Repository<IConcept>(new[] { new Concept("1") });
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();
				var token = tokenSource.Token;

				// act & assert: an in-memory repository has nothing to wait for, but it still reports
				// a cancelled token instead of quietly doing the work
				Assert.That(
					async () => await collection.GetCountAsync(token),
					Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					async () => await collection.GetKeysAsync(token),
					Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					async () => await collection.GetItemAsync("1", token),
					Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					async () => await collection.ContainsAsync("1", token),
					Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					async () => await collection.TryGetValueAsync("1", token),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		[Test]
		public void GivenCancelledToken_WhenWriteToInMemoryRepository_ThenThrowAndChangeNothing()
		{
			// arrange
			var stored = new Concept("1");
			var collection = new Repository<IConcept>(new[] { stored });
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();
				var token = tokenSource.Token;

				// act & assert
				Assert.That(
					async () => await collection.AddAsync(new Concept("2"), token),
					Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					async () => await collection.RemoveAsync(stored, token),
					Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					async () => await collection.ClearAsync(token),
					Throws.InstanceOf<OperationCanceledException>());

				// nothing has been added and nothing has been removed
				Assert.That(collection.Count, Is.EqualTo(1));
				Assert.That(collection.Contains("1"), Is.True);
			}
		}

		[Test]
		public async Task GivenActiveToken_WhenCallRepository_ThenSucceed()
		{
			// arrange
			var collection = new Repository<IConcept>();
			using (var tokenSource = new CancellationTokenSource())
			{
				var token = tokenSource.Token;

				// act
				await collection.AddAsync(new Concept("1"), token);
				await collection.AddAsync(new Concept("2"), token);

				// assert
				Assert.That(await collection.GetCountAsync(token), Is.EqualTo(2));
				Assert.That(await collection.ContainsAsync("1", token), Is.True);
				Assert.That((await collection.TryGetValueAsync("2", token)).Key, Is.True);

				Assert.That(await collection.RemoveAsync(await collection.GetItemAsync("1", token), token), Is.True);
				await collection.ClearAsync(token);
				Assert.That(await collection.GetCountAsync(token), Is.EqualTo(0));
			}
		}

		[Test]
		public void GivenCancelledToken_WhenCallInMemoryRepositoryByBlockingCall_ThenThrow()
		{
			// arrange
			IRepository<IConcept> collection = new Repository<IConcept>(new[] { new Concept("1") });
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();
				var token = tokenSource.Token;

				// act & assert: the blocking wrappers report cancellation just as the asynchronous
				// methods do, including on the path which short-circuits to the synchronous API
				Assert.That(() => collection.Add(new Concept("2"), token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.Remove(new Concept("1"), token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.Clear(token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.GetCount(token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.GetItem("1", token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.GetKeys(token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.Contains("1", token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					() => collection.TryGetValue("1", out var _, token),
					Throws.InstanceOf<OperationCanceledException>());

				Assert.That(collection.GetCount(), Is.EqualTo(1));
			}
		}

		[Test]
		public void GivenCancelledToken_WhenCallCustomRepositoryByBlockingCall_ThenThrow()
		{
			// arrange: a repository which is not the in-memory one goes through the asynchronous path
			IRepository<IConcept> collection = new ForwardingRepository(new Concept("1"));
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();
				var token = tokenSource.Token;

				// act & assert
				Assert.That(() => collection.Add(new Concept("2"), token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.Remove(new Concept("1"), token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.Clear(token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.GetCount(token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.GetItem("1", token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.GetKeys(token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(() => collection.Contains("1", token), Throws.InstanceOf<OperationCanceledException>());
				Assert.That(
					() => collection.TryGetValue("1", out var _, token),
					Throws.InstanceOf<OperationCanceledException>());

				Assert.That(collection.GetCount(), Is.EqualTo(1));
			}
		}

		/// <summary>
		/// Repository which is not <see cref="Repository{T}"/>, so that the blocking wrappers cannot
		/// short-circuit to the synchronous API and have to await the asynchronous methods.
		/// </summary>
		private class ForwardingRepository : IRepository<IConcept>
		{
			private readonly IRepository<IConcept> _collection;

			public ForwardingRepository(params IConcept[] items)
			{
				_collection = new Repository<IConcept>(items);
			}

			public IEnumerator<IConcept> GetEnumerator()
			{
				return _collection.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public Task AddAsync(IConcept item, CancellationToken cancellationToken = default)
			{
				return _collection.AddAsync(item, cancellationToken);
			}

			public Task<Boolean> RemoveAsync(IConcept item, CancellationToken cancellationToken = default)
			{
				return _collection.RemoveAsync(item, cancellationToken);
			}

			public Task ClearAsync(CancellationToken cancellationToken = default)
			{
				return _collection.ClearAsync(cancellationToken);
			}

			public Task<Int32> GetCountAsync(CancellationToken cancellationToken = default)
			{
				return _collection.GetCountAsync(cancellationToken);
			}

			public Task<IConcept> GetItemAsync(String key, CancellationToken cancellationToken = default)
			{
				return _collection.GetItemAsync(key, cancellationToken);
			}

			public Task<ICollection<String>> GetKeysAsync(CancellationToken cancellationToken = default)
			{
				return _collection.GetKeysAsync(cancellationToken);
			}

			public Task<Boolean> ContainsAsync(String key, CancellationToken cancellationToken = default)
			{
				return _collection.ContainsAsync(key, cancellationToken);
			}

			public Task<KeyValuePair<Boolean, IConcept>> TryGetValueAsync(String key, CancellationToken cancellationToken = default)
			{
				return _collection.TryGetValueAsync(key, cancellationToken);
			}
		}
	}
}
