using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class EventCollectionTest
	{
		[Test]
		public void GivenNoHandlers_WhenAdd_ThenSucceed()
		{
			var collection = new SimpleEventCollection();

			collection.Add("A");
			collection.Add("B");
			collection.Add("C");

			Assert.AreEqual(3, collection.Count);
			Assert.IsTrue(collection.Contains("A"));
			Assert.IsTrue(collection.Contains("B"));
			Assert.IsTrue(collection.Contains("C"));
			Assert.AreEqual("ABC", string.Join(string.Empty, collection));
		}

		[Test]
		public void GivenNoHandlers_WhenRemove_ThenSucceed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };

			Assert.IsTrue(collection.Remove("B"));
			Assert.IsFalse(collection.Remove("D"));
			Assert.AreEqual(2, collection.Count);
			Assert.IsTrue(collection.Contains("A"));
			Assert.IsFalse(collection.Contains("B"));
			Assert.IsTrue(collection.Contains("C"));
			Assert.AreEqual("AC", string.Join(string.Empty, collection));
		}

		[Test]
		public void GivenNoHandlers_WhenClear_ThenSucceed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };

			collection.Clear();

			Assert.AreEqual(0, collection.Count);
			Assert.IsFalse(collection.Contains("A"));
			Assert.IsFalse(collection.Contains("B"));
			Assert.IsFalse(collection.Contains("C"));
			Assert.AreEqual(string.Empty, string.Join(string.Empty, collection));
		}

		[Test]
		public void GivenDifferentHandlers_WhenTryToAdd_ThenWorkOnlyIfAllowed()
		{
			var collection = new SimpleEventCollection();
			string result = string.Empty;
			collection.ItemAdding += (sender, args) => { args.IsCanceled = args.Item == "B"; };
			collection.ItemAdded += (sender, args) => { result += args.Item; };

			collection.Add("A");
			collection.Add("B");
			collection.Add("C");

			Assert.AreEqual(2, collection.Count);
			Assert.IsTrue(collection.Contains("A"));
			Assert.IsFalse(collection.Contains("B"));
			Assert.IsTrue(collection.Contains("C"));
			Assert.AreEqual("AC", result);
		}

		[Test]
		public void GivenDifferentHandlers_WhenTryToRemove_ThenWorkOnlyIfAllowed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };
			string result = string.Empty;
			collection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item == "B"; };
			collection.ItemRemoved += (sender, args) => { result += args.Item; };

			Assert.IsTrue(collection.Remove("A"));
			Assert.IsFalse(collection.Remove("B"));
			Assert.IsTrue(collection.Remove("C"));
			Assert.AreEqual(1, collection.Count);
			Assert.IsFalse(collection.Contains("A"));
			Assert.IsTrue(collection.Contains("B"));
			Assert.IsFalse(collection.Contains("C"));
			Assert.AreEqual("AC", result);
		}

		[Test]
		public void GivenAllowingHandler_WhenClear_ThenSucceed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };
			string result = string.Empty;
			collection.ItemRemoving += (sender, args) => { };
			collection.ItemRemoved += (sender, args) => { result += args.Item; };

			collection.Clear();

			Assert.AreEqual(0, collection.Count);
			Assert.AreEqual("ABC", result);
		}

		[Test]
		public void GivenForbiddingHandler_WhenTryToClear_ThenFail()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };
			string result = string.Empty;
			collection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item == "B"; };
			collection.ItemRemoved += (sender, args) => { result += args.Item; };

			var error = Assert.Throws<ItemsCantBeRemovedException<string>>(() => collection.Clear());

			Assert.AreEqual(3, collection.Count);
			Assert.IsTrue(string.IsNullOrEmpty(result));
			Assert.AreEqual("B", error.Items.Single());
		}

		[Test]
		public void GivenEventCollection_WhenCheckIsReadOnly_ThenReturnFalse()
		{
			Assert.IsFalse(new SimpleEventCollection().IsReadOnly);

			Assert.IsFalse(new SimpleEventCollection { "A", "B", "C" }.IsReadOnly);

			Assert.IsFalse(new SimpleEventCollection(new[] { "A", "B", "C" }).IsReadOnly);
		}

		[Test]
		public void GivenEventCollection_WhenCopyTo_ThenSucceed()
		{
			// arrange
			var collection = new SimpleEventCollection { "A", "B", "C" };
			var array = new string[3];

			// act & assert
			Assert.IsFalse(collection.SequenceEqual(array));

			collection.CopyTo(array, 0);
			Assert.IsTrue(collection.SequenceEqual(array));
		}

		private class SimpleEventCollection : EventCollectionBase<string>
		{
			#region Properties

			private readonly ICollection<string> _collection;

			#endregion

			#region Constructors

			public SimpleEventCollection()
				: this(new List<string>())
			{ }

			public SimpleEventCollection(IEnumerable<string> items)
				: this(items.ToList())
			{ }

			public SimpleEventCollection(List<string> items)
			{
				_collection = items;
			}

			#endregion

			#region Overrides

			public override IEnumerator<string> GetEnumerator()
			{
				return _collection.GetEnumerator();
			}

			public override int Count
			{ get { return _collection.Count; } }

			protected override void AddImplementation(string item)
			{
				_collection.Add(item);
			}

			protected override void ClearImplementation()
			{
				_collection.Clear();
			}

			public override bool Contains(string item)
			{
				return _collection.Contains(item);
			}

			public override void CopyTo(string[] array, int arrayIndex)
			{
				_collection.CopyTo(array, arrayIndex);
			}

			protected override bool RemoveImplementation(string item)
			{
				return _collection.Remove(item);
			}

			#endregion
		}

		[Test]
		public void GivenEventCollection_WhenCallCollectionMethods_ThenSucceed()
		{
			// arrange
			IConcept concept1, concept2, concept3;
			var collection = new EventCollection<IConcept>(new IConcept[]
			{
				concept1 = new Concept("1"),
				concept2 = new Concept("2"),
				concept3 = new Concept("3"),
			});

			// this[]
			Assert.AreSame(concept1, collection["1"]);
			Assert.AreSame(concept2, collection["2"]);
			Assert.AreSame(concept3, collection["3"]);
			Assert.Throws<KeyNotFoundException>(() => { var _ = collection["4"]; });

			// Keys
			Assert.IsTrue(collection.Keys.SequenceEqual(new[] { "1", "2", "3" }));

			// CopyTo()
			var array = new IConcept[5];
			collection.CopyTo(array, 1);
			Assert.AreSame(null, array[0]);
			Assert.AreSame(collection["1"], array[1]);
			Assert.AreSame(collection["2"], array[2]);
			Assert.AreSame(collection["3"], array[3]);
			Assert.AreSame(null, array[4]);

			// TryGetValue()
			IConcept concept;
			Assert.IsTrue(collection.TryGetValue("1", out concept));
			Assert.AreSame(concept1, concept);
			Assert.IsTrue(collection.TryGetValue("2", out concept));
			Assert.AreSame(concept2, concept);
			Assert.IsTrue(collection.TryGetValue("3", out concept));
			Assert.AreSame(concept3, concept);
			Assert.IsFalse(collection.TryGetValue("4", out concept));

			// Clear()
			collection.Clear();
			Assert.AreEqual(0, collection.Count);
		}

		[Test]
		public void GivenNoItems_WhenTryToCreateItemsCantBeRemovedException_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new ItemsCantBeRemovedException<int>(null));
		}

		[Test]
		public void GivenItemsCantBeRemovedException_WhenSerializeAdDeserialize_ThenSucceed()
		{
			// arrange
			var exception = new ItemsCantBeRemovedException<int>(new[] { 123, 987, 465 });

			var formatter = new BinaryFormatter();

			using (var stream = new MemoryStream(new byte[8096]))
			{
				// act
				formatter.Serialize(stream, exception);

				stream.Seek(0, SeekOrigin.Begin);

				var deserialized = (ItemsCantBeRemovedException<int>) formatter.Deserialize(stream);

				// assert
				Assert.IsTrue(exception.Items.SequenceEqual(deserialized.Items));
			}
		}
	}
}
