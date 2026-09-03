using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class RepositoryTests
	{
		private class SimpleIdentifiable : IIdentifiable
		{
			public string ID
			{ get; }

			public SimpleIdentifiable(string id = null)
			{
				ID = id;
			}

			public static implicit operator SimpleIdentifiable(string id)
			{
				return new SimpleIdentifiable(id);
			}

			public override string ToString()
			{
				return ID;
			}
		}

		[Test]
		public async Task GivenNoHandlers_WhenAdd_ThenSucceed()
		{
			// arrange
			var syncCollection = new Repository<SimpleIdentifiable>();
			var asyncCollection = new Repository<SimpleIdentifiable>();

			// act
			syncCollection.Add("A");
			syncCollection.Add("B");
			syncCollection.Add("C");

			await asyncCollection.AddAsync("A");
			await asyncCollection.AddAsync("B");
			await asyncCollection.AddAsync("C");

			// assert
			Assert.That(syncCollection.Count, Is.EqualTo(3));
			Assert.That(syncCollection.Contains("A"), Is.True);
			Assert.That(syncCollection.Contains("B"), Is.True);
			Assert.That(syncCollection.Contains("C"), Is.True);
			Assert.That(string.Join(string.Empty, syncCollection), Is.EqualTo("ABC"));

			Assert.That(await asyncCollection.GetCountAsync(), Is.EqualTo(3));
			Assert.That(await asyncCollection.ContainsAsync("A"), Is.True);
			Assert.That(await asyncCollection.ContainsAsync("B"), Is.True);
			Assert.That(await asyncCollection.ContainsAsync("C"), Is.True);
			Assert.That(string.Join(string.Empty, asyncCollection), Is.EqualTo("ABC"));
		}

		[Test]
		public async Task GivenNoHandlers_WhenRemove_ThenSucceed()
		{
			// arrange
			var syncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };
			var asyncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };

			// act & assert
			Assert.That(syncCollection.Remove("B"), Is.True);
			Assert.That(syncCollection.Remove("D"), Is.False);
			Assert.That(syncCollection.Count, Is.EqualTo(2));
			Assert.That(syncCollection.Contains("A"), Is.True);
			Assert.That(syncCollection.Contains("B"), Is.False);
			Assert.That(syncCollection.Contains("C"), Is.True);
			Assert.That(string.Join(string.Empty, syncCollection), Is.EqualTo("AC"));

			Assert.That(await asyncCollection.RemoveAsync("B"), Is.True);
			Assert.That(await asyncCollection.RemoveAsync("D"), Is.False);
			Assert.That(await asyncCollection.GetCountAsync(), Is.EqualTo(2));
			Assert.That(await asyncCollection.ContainsAsync("A"), Is.True);
			Assert.That(await asyncCollection.ContainsAsync("B"), Is.False);
			Assert.That(await asyncCollection.ContainsAsync("C"), Is.True);
			Assert.That(string.Join(string.Empty, asyncCollection), Is.EqualTo("AC"));
		}

		[Test]
		public async Task GivenNoHandlers_WhenClear_ThenSucceed()
		{
			// arrange
			var syncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };
			var asyncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };

			// act
			syncCollection.Clear();

			await asyncCollection.ClearAsync();

			// assert
			Assert.That(syncCollection.Count, Is.EqualTo(0));
			Assert.That(syncCollection.Contains("A"), Is.False);
			Assert.That(syncCollection.Contains("B"), Is.False);
			Assert.That(syncCollection.Contains("C"), Is.False);
			Assert.That(string.Join(string.Empty, syncCollection), Is.Empty);

			Assert.That(await asyncCollection.GetCountAsync(), Is.EqualTo(0));
			Assert.That(await asyncCollection.ContainsAsync("A"), Is.False);
			Assert.That(await asyncCollection.ContainsAsync("B"), Is.False);
			Assert.That(await asyncCollection.ContainsAsync("C"), Is.False);
			Assert.That(string.Join(string.Empty, asyncCollection), Is.Empty);
		}

		[Test]
		public async Task GivenDifferentHandlers_WhenTryToAdd_ThenWorkOnlyIfAllowed()
		{
			// arrange
			var syncCollection = new Repository<SimpleIdentifiable>();
			var asyncCollection = new Repository<SimpleIdentifiable>();

			string syncResult = string.Empty;
			string asyncResult = string.Empty;

			syncCollection.ItemAdding += (sender, args) => { args.IsCanceled = args.Item.ID == "B"; };
			syncCollection.ItemAdded += (sender, args) => { syncResult += args.Item; };
			asyncCollection.ItemAdding += (sender, args) => { args.IsCanceled = args.Item.ID == "B"; };
			asyncCollection.ItemAdded += (sender, args) => { asyncResult += args.Item; };

			// act
			syncCollection.Add("A");
			syncCollection.Add("B");
			syncCollection.Add("C");

			await asyncCollection.AddAsync("A");
			await asyncCollection.AddAsync("B");
			await asyncCollection.AddAsync("C");

			// assert
			Assert.That(syncCollection.Count, Is.EqualTo(2));
			Assert.That(syncCollection.Contains("A"), Is.True);
			Assert.That(syncCollection.Contains("B"), Is.False);
			Assert.That(syncCollection.Contains("C"), Is.True);
			Assert.That(syncResult, Is.EqualTo("AC"));

			Assert.That(await asyncCollection.GetCountAsync(), Is.EqualTo(2));
			Assert.That(await asyncCollection.ContainsAsync("A"), Is.True);
			Assert.That(await asyncCollection.ContainsAsync("B"), Is.False);
			Assert.That(await asyncCollection.ContainsAsync("C"), Is.True);
			Assert.That(asyncResult, Is.EqualTo("AC"));
		}

		[Test]
		public async Task GivenDifferentHandlers_WhenTryToRemove_ThenWorkOnlyIfAllowed()
		{
			// arrange
			var syncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };
			var asyncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };

			string syncResult = string.Empty;
			string asyncResult = string.Empty;

			syncCollection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item.ID == "B"; };
			syncCollection.ItemRemoved += (sender, args) => { syncResult += args.Item; };
			asyncCollection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item.ID == "B"; };
			asyncCollection.ItemRemoved += (sender, args) => { asyncResult += args.Item; };

			// act & assert
			Assert.That(syncCollection.Remove("A"), Is.True);
			Assert.That(syncCollection.Remove("B"), Is.False);
			Assert.That(syncCollection.Remove("C"), Is.True);
			Assert.That(syncCollection.Count, Is.EqualTo(1));
			Assert.That(syncCollection.Contains("A"), Is.False);
			Assert.That(syncCollection.Contains("B"), Is.True);
			Assert.That(syncCollection.Contains("C"), Is.False);
			Assert.That(syncResult, Is.EqualTo("AC"));

			Assert.That(await asyncCollection.RemoveAsync("A"), Is.True);
			Assert.That(await asyncCollection.RemoveAsync("B"), Is.False);
			Assert.That(await asyncCollection.RemoveAsync("C"), Is.True);
			Assert.That(await asyncCollection.GetCountAsync(), Is.EqualTo(1));
			Assert.That(await asyncCollection.ContainsAsync("A"), Is.False);
			Assert.That(await asyncCollection.ContainsAsync("B"), Is.True);
			Assert.That(await asyncCollection.ContainsAsync("C"), Is.False);
			Assert.That(asyncResult, Is.EqualTo("AC"));
		}

		[Test]
		public async Task GivenAllowingHandler_WhenClear_ThenSucceed()
		{
			// arrange
			var syncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };
			var asyncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };

			string syncResult = string.Empty;
			string asyncResult = string.Empty;

			syncCollection.ItemRemoving += (sender, args) => { };
			syncCollection.ItemRemoved += (sender, args) => { syncResult += args.Item; };
			asyncCollection.ItemRemoving += (sender, args) => { };
			asyncCollection.ItemRemoved += (sender, args) => { asyncResult += args.Item; };

			// act
			syncCollection.Clear();
			await asyncCollection.ClearAsync();

			// assert
			Assert.That(syncCollection.Count, Is.EqualTo(0));
			Assert.That(syncResult, Is.EqualTo("ABC"));

			Assert.That(asyncCollection.Count, Is.EqualTo(0));
			Assert.That(asyncResult, Is.EqualTo("ABC"));
		}

		[Test]
		public async Task GivenForbiddingHandler_WhenTryToClear_ThenFail()
		{
			// arrange
			var syncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };
			var asyncCollection = new Repository<SimpleIdentifiable> { "A", "B", "C" };

			string syncResult = string.Empty;
			string asyncResult = string.Empty;

			syncCollection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item.ID == "B"; };
			syncCollection.ItemRemoved += (sender, args) => { syncResult += args.Item; };
			asyncCollection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item.ID == "B"; };
			asyncCollection.ItemRemoved += (sender, args) => { asyncResult += args.Item; };

			// act
			var syncError = Assert.Throws<ItemsCantBeRemovedException<SimpleIdentifiable>>(() => syncCollection.Clear());
			var asyncError = Assert.ThrowsAsync<ItemsCantBeRemovedException<SimpleIdentifiable>>(async () => await asyncCollection.ClearAsync());

			// assert
			Assert.That(syncCollection.Count, Is.EqualTo(3));
			Assert.That(string.IsNullOrEmpty(syncResult), Is.True);
			Assert.That(syncError.Items.Single().ToString(), Is.EqualTo("B"));

			Assert.That(asyncCollection.GetCount(), Is.EqualTo(3));
			Assert.That(string.IsNullOrEmpty(asyncResult), Is.True);
			Assert.That(asyncError.Items.Single().ToString(), Is.EqualTo("B"));
		}

		[Test]
		public void GivenInMemoryRepository_WhenCheckIsReadOnly_ThenReturnFalse()
		{
			Assert.That(new Repository<SimpleIdentifiable>().IsReadOnly, Is.False);
			Assert.That(new Repository<SimpleIdentifiable> { "A", "B", "C" }.IsReadOnly, Is.False);
			Assert.That(new Repository<SimpleIdentifiable>(new SimpleIdentifiable[] { "A", "B", "C" }).IsReadOnly, Is.False);
		}

		[Test]
		public void GivenInMemoryRepository_WhenCopyTo_ThenSucceed()
		{
			// arrange
			var collection = new Repository<SimpleIdentifiable> { "A", "B", "C" };
			var array = new SimpleIdentifiable[3];

			// act & assert
			Assert.That(collection.ToArray().SequenceEqual(array), Is.False);

			collection.CopyTo(array, 0);
			Assert.That(collection.SequenceEqual(array), Is.True);
		}

		[Test]
		public async Task GivenInMemoryRepository_WhenCallCollectionMethods_ThenSucceed()
		{
			// arrange
			IConcept concept1, concept2, concept3;
			var concepts = new[]
			{
				concept1 = new Concept("1"),
				concept2 = new Concept("2"),
				concept3 = new Concept("3"),
			};

			var syncCollection = new Repository<IConcept>(concepts);
			var asyncCollection = new Repository<IConcept>(concepts);

			// this[]
			Assert.That(syncCollection["1"], Is.SameAs(concept1));
			Assert.That(syncCollection["2"], Is.SameAs(concept2));
			Assert.That(syncCollection["3"], Is.SameAs(concept3));
			Assert.Throws<KeyNotFoundException>(() => { var _ = syncCollection["4"]; });

			Assert.That(await asyncCollection.GetItemAsync("1"), Is.SameAs(concept1));
			Assert.That(await asyncCollection.GetItemAsync("2"), Is.SameAs(concept2));
			Assert.That(await asyncCollection.GetItemAsync("3"), Is.SameAs(concept3));
			Assert.ThrowsAsync<KeyNotFoundException>(async () => { var _ = await asyncCollection.GetItemAsync("4"); });

			// Keys
			Assert.That(syncCollection.Keys.SequenceEqual(new[] { "1", "2", "3" }), Is.True);
			Assert.That((await syncCollection.GetKeysAsync()).SequenceEqual(new[] { "1", "2", "3" }), Is.True);

			// CopyTo()
			var array = new IConcept[5];
			syncCollection.CopyTo(array, 1);
			Assert.That(array[0], Is.Null);
			Assert.That(array[1], Is.SameAs(syncCollection["1"]));
			Assert.That(array[2], Is.SameAs(syncCollection["2"]));
			Assert.That(array[3], Is.SameAs(syncCollection["3"]));
			Assert.That(array[4], Is.Null);

			// TryGetValue()
			IConcept concept;

			Assert.That(syncCollection.TryGetValue("1", out concept), Is.True);
			Assert.That(concept, Is.SameAs(concept1));
			Assert.That(syncCollection.TryGetValue("2", out concept), Is.True);
			Assert.That(concept, Is.SameAs(concept2));
			Assert.That(syncCollection.TryGetValue("3", out concept), Is.True);
			Assert.That(concept, Is.SameAs(concept3));
			Assert.That(syncCollection.TryGetValue("4", out concept), Is.False);

			Assert.That(await asyncCollection.TryGetValueAsync("1"), Is.EqualTo(new KeyValuePair<bool, IConcept>(true, concept1)));
			Assert.That(await asyncCollection.TryGetValueAsync("2"), Is.EqualTo(new KeyValuePair<bool, IConcept>(true, concept2)));
			Assert.That(await asyncCollection.TryGetValueAsync("3"), Is.EqualTo(new KeyValuePair<bool, IConcept>(true, concept3)));
			Assert.That(await asyncCollection.TryGetValueAsync("4"), Is.EqualTo(new KeyValuePair<bool, IConcept>(false, null)));

			// Clear()
			syncCollection.Clear();
			await asyncCollection.ClearAsync();

			Assert.That(await syncCollection.GetCountAsync(), Is.EqualTo(0));
			Assert.That(await asyncCollection.GetCountAsync(), Is.EqualTo(0));
		}

		[Test]
		public void GivenNoItems_WhenTryToCreateItemsCantBeRemovedException_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => throw new ItemsCantBeRemovedException<int>(null));
		}

		[Test]
#pragma warning disable SYSLIB0050, SYSLIB0051
		public void GivenItemsCantBeRemovedException_WhenSerializeAdDeserialize_ThenSucceed()
		{
			// arrange
			var exception = new ItemsCantBeRemovedException<int>(new[] { 123, 987, 465 });

			var info = new SerializationInfo(typeof(ItemsCantBeRemovedException<int>), new FormatterConverter());
			var context = new StreamingContext(StreamingContextStates.All);

			// act
			exception.GetObjectData(info, context);

			var deserialized = new ItemsCantBeRemovedException<int>(info, context);

			// assert
			Assert.That(exception.Items.SequenceEqual(deserialized.Items), Is.True);
		}
#pragma warning restore SYSLIB0050, SYSLIB0051

		private class TestRepository : IRepository<SimpleIdentifiable>
		{
			private readonly IRepository<SimpleIdentifiable> _collection;

			public TestRepository()
				: this(Array.Empty<SimpleIdentifiable>())
			{ }

			public TestRepository(ICollection<SimpleIdentifiable> items)
			{
				_collection = new Repository<SimpleIdentifiable>(items);
			}

			public IEnumerator<SimpleIdentifiable> GetEnumerator()
			{
				return _collection.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public Task AddAsync(SimpleIdentifiable item, CancellationToken cancellationToken = default)
			{
				return _collection.AddAsync(item, cancellationToken);
			}

			public Task<bool> RemoveAsync(SimpleIdentifiable item, CancellationToken cancellationToken = default)
			{
				return _collection.RemoveAsync(item, cancellationToken);
			}

			public Task ClearAsync(CancellationToken cancellationToken = default)
			{
				return _collection.ClearAsync(cancellationToken);
			}

			public Task<int> GetCountAsync(CancellationToken cancellationToken = default)
			{
				return _collection.GetCountAsync(cancellationToken);
			}

			public Task<SimpleIdentifiable> GetItemAsync(string key, CancellationToken cancellationToken = default)
			{
				return _collection.GetItemAsync(key, cancellationToken);
			}

			public Task<ICollection<string>> GetKeysAsync(CancellationToken cancellationToken = default)
			{
				return _collection.GetKeysAsync(cancellationToken);
			}

			public Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default)
			{
				return _collection.ContainsAsync(key, cancellationToken);
			}

			public Task<KeyValuePair<bool, SimpleIdentifiable>> TryGetValueAsync(string key, CancellationToken cancellationToken = default)
			{
				return _collection.TryGetValueAsync(key, cancellationToken);
			}
		}

		[Test]
		public void CheckAddExtension()
		{
			// arrange
			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>();
			IRepository<SimpleIdentifiable> testRepository = new TestRepository();

			SimpleIdentifiable item = "0";

			// act
			inMemoryRepository.Add(item);
			testRepository.Add(item);

			// assert
			Assert.That(inMemoryRepository.SequenceEqual(testRepository), Is.True);
		}

		[Test]
		public void CheckRemoveExtension()
		{
			// arrange
			SimpleIdentifiable item = "0";

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable> { item };
			IRepository<SimpleIdentifiable> testRepository = new TestRepository { item };

			// act
			bool inMemoryResult = inMemoryRepository.Remove(item);
			bool testResult = testRepository.Remove(item);

			// assert
			Assert.That(inMemoryResult, Is.EqualTo(testResult));
			Assert.That(inMemoryRepository.GetCount(), Is.EqualTo(testRepository.GetCount()));
		}

		[Test]
		public void CheckClearExtension()
		{
			// arrange
			var items = new SimpleIdentifiable[] { "1", "2", "3" };

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>(items);
			IRepository<SimpleIdentifiable> testRepository = new TestRepository(items);

			// act
			inMemoryRepository.Clear();
			testRepository.Clear();

			// assert
			Assert.That(inMemoryRepository.GetCount(), Is.EqualTo(testRepository.GetCount()));
		}

		[Test]
		public void CheckGetCountExtension()
		{
			// arrange
			var items = new SimpleIdentifiable[] { "1", "2", "3" };

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>(items);
			IRepository<SimpleIdentifiable> testRepository = new TestRepository(items);

			// act & assert
			Assert.That(inMemoryRepository.GetCount(), Is.EqualTo(3));
			Assert.That(testRepository.GetCount(), Is.EqualTo(3));
		}

		[Test]
		public void CheckGetItemExtension()
		{
			// arrange
			var items = new SimpleIdentifiable[] { "1", "2", "3" };

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>(items);
			IRepository<SimpleIdentifiable> testRepository = new TestRepository(items);

			// act & assert
			Assert.That(inMemoryRepository.GetItem("2"), Is.SameAs(testRepository.GetItem("2")));
		}

		[Test]
		public void CheckGetKeysExtension()
		{
			// arrange
			var keys = new[] { "1", "2", "3" };
			var items = keys.Select(k => new SimpleIdentifiable(k)).ToArray();

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>(items);
			IRepository<SimpleIdentifiable> testRepository = new TestRepository(items);

			// act & assert
			Assert.That(inMemoryRepository.GetKeys().SequenceEqual(keys));
			Assert.That(testRepository.GetKeys().SequenceEqual(keys));
		}

		[Test]
		public void CheckContainsExtension()
		{
			// arrange
			var items = new SimpleIdentifiable[] { "1", "2", "3" };

			var notFound = new SimpleIdentifiable("4");

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>(items);
			IRepository<SimpleIdentifiable> testRepository = new TestRepository(items);

			// act & assert
			Assert.That(inMemoryRepository.Contains(items[1]), Is.True);
			Assert.That(testRepository.Contains(items[1]), Is.True);

			Assert.That(inMemoryRepository.Contains(notFound), Is.False);
			Assert.That(testRepository.Contains(notFound), Is.False);
		}

		[Test]
		public void CheckContainsByKeyExtension()
		{
			// arrange
			// note: the overload taking a key is the extension itself, while the one taking an item
			// binds to LINQ's Enumerable.Contains - both are checked, in this test and the one above.
			var items = new SimpleIdentifiable[] { "1", "2", "3" };

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>(items);
			IRepository<SimpleIdentifiable> testRepository = new TestRepository(items);

			// act & assert
			Assert.That(inMemoryRepository.Contains("2"), Is.True);
			Assert.That(testRepository.Contains("2"), Is.True);

			Assert.That(inMemoryRepository.Contains("4"), Is.False);
			Assert.That(testRepository.Contains("4"), Is.False);
		}

		[Test]
		public void CheckTryGetValueExtension()
		{
			// arrange
			var items = new SimpleIdentifiable[] { "1", "2", "3" };

			IRepository<SimpleIdentifiable> inMemoryRepository = new Repository<SimpleIdentifiable>(items);
			IRepository<SimpleIdentifiable> testRepository = new TestRepository(items);

			// act & assert
			SimpleIdentifiable found;

			Assert.That(inMemoryRepository.TryGetValue("2", out found), Is.True);
			Assert.That(found, Is.SameAs(items[1]));
			Assert.That(testRepository.TryGetValue("2", out found), Is.True);
			Assert.That(found, Is.SameAs(items[1]));

			Assert.That(inMemoryRepository.TryGetValue("4", out found), Is.False);
			Assert.That(found, Is.Null);
			Assert.That(testRepository.TryGetValue("4", out found), Is.False);
			Assert.That(found, Is.Null);
		}
	}
}
