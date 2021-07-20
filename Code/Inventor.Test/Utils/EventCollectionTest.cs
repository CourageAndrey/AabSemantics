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
			var collection = new EventCollection<string>();

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
			var collection = new EventCollection<string> { "A", "B", "C" };

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
			var collection = new EventCollection<string> { "A", "B", "C" };

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
			var collection = new EventCollection<string>();
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
			var collection = new EventCollection<string> { "A", "B", "C" };
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
			var collection = new EventCollection<string> { "A", "B", "C" };
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
			var collection = new EventCollection<string> { "A", "B", "C" };
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
			Assert.IsFalse(new EventCollection<string>().IsReadOnly);

			Assert.IsFalse(new EventCollection<string> { "A", "B", "C" }.IsReadOnly);

			Assert.IsFalse(new EventCollection<string>(new[] { "A", "B", "C" }).IsReadOnly);
		}

		[Test]
		public void CopyToCopiesItems()
		{
			// arrange
			var collection = new EventCollection<string> {"A", "B", "C"};
			var array = new string[3];

			// act & assert
			Assert.IsFalse(collection.SequenceEqual(array));

			collection.CopyTo(array, 0);
			Assert.IsTrue(collection.SequenceEqual(array));
		}
	}
}
