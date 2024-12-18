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

			Assert.That(collection.Count, Is.EqualTo(3));
			Assert.That(collection.Contains("A"), Is.True);
			Assert.That(collection.Contains("B"), Is.True);
			Assert.That(collection.Contains("C"), Is.True);
			Assert.That(string.Join(string.Empty, collection), Is.EqualTo("ABC"));
		}

		[Test]
		public void GivenNoHandlers_WhenRemove_ThenSucceed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };

			Assert.That(collection.Remove("B"), Is.True);
			Assert.That(collection.Remove("D"), Is.False);
			Assert.That(collection.Count, Is.EqualTo(2));
			Assert.That(collection.Contains("A"), Is.True);
			Assert.That(collection.Contains("B"), Is.False);
			Assert.That(collection.Contains("C"), Is.True);
			Assert.That(string.Join(string.Empty, collection), Is.EqualTo("AC"));
		}

		[Test]
		public void GivenNoHandlers_WhenClear_ThenSucceed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };

			collection.Clear();

			Assert.That(collection.Count, Is.EqualTo(0));
			Assert.That(collection.Contains("A"), Is.False);
			Assert.That(collection.Contains("B"), Is.False);
			Assert.That(collection.Contains("C"), Is.False);
			Assert.That(string.Join(string.Empty, collection), Is.Empty);
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

			Assert.That(collection.Count, Is.EqualTo(2));
			Assert.That(collection.Contains("A"), Is.True);
			Assert.That(collection.Contains("B"), Is.False);
			Assert.That(collection.Contains("C"), Is.True);
			Assert.That(result, Is.EqualTo("AC"));
		}

		[Test]
		public void GivenDifferentHandlers_WhenTryToRemove_ThenWorkOnlyIfAllowed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };
			string result = string.Empty;
			collection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item == "B"; };
			collection.ItemRemoved += (sender, args) => { result += args.Item; };

			Assert.That(collection.Remove("A"), Is.True);
			Assert.That(collection.Remove("B"), Is.False);
			Assert.That(collection.Remove("C"), Is.True);
			Assert.That(collection.Count, Is.EqualTo(1));
			Assert.That(collection.Contains("A"), Is.False);
			Assert.That(collection.Contains("B"), Is.True);
			Assert.That(collection.Contains("C"), Is.False);
			Assert.That(result, Is.EqualTo("AC"));
		}

		[Test]
		public void GivenAllowingHandler_WhenClear_ThenSucceed()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };
			string result = string.Empty;
			collection.ItemRemoving += (sender, args) => { };
			collection.ItemRemoved += (sender, args) => { result += args.Item; };

			collection.Clear();

			Assert.That(collection.Count, Is.EqualTo(0));
			Assert.That(result, Is.EqualTo("ABC"));
		}

		[Test]
		public void GivenForbiddingHandler_WhenTryToClear_ThenFail()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };
			string result = string.Empty;
			collection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item == "B"; };
			collection.ItemRemoved += (sender, args) => { result += args.Item; };

			var error = Assert.Throws<ItemsCantBeRemovedException<string>>(() => collection.Clear());

			Assert.That(collection.Count, Is.EqualTo(3));
			Assert.That(string.IsNullOrEmpty(result), Is.True);
			Assert.That(error.Items.Single(), Is.EqualTo("B"));
		}

		[Test]
		public void GivenEventCollection_WhenCheckIsReadOnly_ThenReturnFalse()
		{
			Assert.That(new SimpleEventCollection().IsReadOnly, Is.False);

			Assert.That(new SimpleEventCollection { "A", "B", "C" }.IsReadOnly, Is.False);

			Assert.That(new SimpleEventCollection(new[] { "A", "B", "C" }).IsReadOnly, Is.False);
		}

		[Test]
		public void GivenEventCollection_WhenCopyTo_ThenSucceed()
		{
			// arrange
			var collection = new SimpleEventCollection { "A", "B", "C" };
			var array = new string[3];

			// act & assert
			Assert.That(collection.SequenceEqual(array), Is.False);

			collection.CopyTo(array, 0);
			Assert.That(collection.SequenceEqual(array), Is.True);
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
			Assert.That(collection["1"], Is.SameAs(concept1));
			Assert.That(collection["2"], Is.SameAs(concept2));
			Assert.That(collection["3"], Is.SameAs(concept3));
			Assert.Throws<KeyNotFoundException>(() => { var _ = collection["4"]; });

			// Keys
			Assert.That(collection.Keys.SequenceEqual(new[] { "1", "2", "3" }), Is.True);

			// CopyTo()
			var array = new IConcept[5];
			collection.CopyTo(array, 1);
			Assert.That(array[0], Is.Null);
			Assert.That(array[1], Is.SameAs(collection["1"]));
			Assert.That(array[2], Is.SameAs(collection["2"]));
			Assert.That(array[3], Is.SameAs(collection["3"]));
			Assert.That(array[4], Is.Null);

			// TryGetValue()
			IConcept concept;
			Assert.That(collection.TryGetValue("1", out concept), Is.True);
			Assert.That(concept, Is.SameAs(concept1));
			Assert.That(collection.TryGetValue("2", out concept), Is.True);
			Assert.That(concept, Is.SameAs(concept2));
			Assert.That(collection.TryGetValue("3", out concept), Is.True);
			Assert.That(concept, Is.SameAs(concept3));
			Assert.That(collection.TryGetValue("4", out concept), Is.False);

			// Clear()
			collection.Clear();
			Assert.That(collection.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenNoItems_WhenTryToCreateItemsCantBeRemovedException_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => throw new ItemsCantBeRemovedException<int>(null));
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
				Assert.That(exception.Items.SequenceEqual(deserialized.Items), Is.True);
			}
		}
	}
}
