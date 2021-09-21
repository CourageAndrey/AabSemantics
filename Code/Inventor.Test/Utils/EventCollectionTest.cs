using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core.Utils;

namespace Inventor.Test.Utils
{
	[TestFixture]
	public class EventCollectionTest
	{
		[Test]
		public void AddAddsItem()
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
		public void RemoveRemovesItem()
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
		public void ClearRemoveAllItems()
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
		public void AddWorksOnlyIfAllowed()
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
		public void RemoveWorksOnlyIfAllowed()
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
		public void SuccessfulClearWorks()
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
		public void ForbiddenClearFails()
		{
			var collection = new SimpleEventCollection { "A", "B", "C" };
			string result = string.Empty;
			collection.ItemRemoving += (sender, args) => { args.IsCanceled = args.Item == "B"; };
			collection.ItemRemoved += (sender, args) => { result += args.Item; };

			var error = Assert.Throws<ItemsCantBeRemovedException<string>>(() => collection.Clear());

			Assert.AreEqual(3, collection.Count);
			Assert.IsNullOrEmpty(result);
			Assert.AreEqual("B", error.Items.Single());
		}

		[Test]
		public void EventCollectionIsAlwaysEditable()
		{
			Assert.IsFalse(new SimpleEventCollection().IsReadOnly);

			Assert.IsFalse(new SimpleEventCollection { "A", "B", "C" }.IsReadOnly);

			Assert.IsFalse(new SimpleEventCollection(new[] { "A", "B", "C" }).IsReadOnly);
		}

		[Test]
		public void CopyToCopiesItems()
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
	}
}
