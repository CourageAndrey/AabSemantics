using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Inventor.Algorithms.Test
{
	[TestFixture]
	public class HuffmanTest
	{
		[Test]
		public void GivenNullItemsWhenTryToEncodeThenThrowError()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => Huffman.HuffmanEncode<SimpleNode, string>(null, getWeight, string.Empty, appendLeft, appendRight));
		}

		[Test]
		public void GivenNullGetWeightWhenTryToEncodeThenThrowError()
		{
			// arrange
			var items = new List<SimpleNode> { new SimpleNode() };

			// act & assert
			Assert.Throws<ArgumentNullException>(() => items.HuffmanEncode(null, string.Empty, appendLeft, appendRight));
		}

		[Test]
		public void GivenNullAppentLeftWhenTryToEncodeThenThrowError()
		{
			// arrange
			var items = new List<SimpleNode> { new SimpleNode() };

			// act & assert
			Assert.Throws<ArgumentNullException>(() => items.HuffmanEncode(getWeight, string.Empty, null, appendRight));
		}

		[Test]
		public void GivenNullAppendRightWhenTryToEncodeThenThrowError()
		{
			// arrange
			var items = new List<SimpleNode> { new SimpleNode() };

			// act & assert
			Assert.Throws<ArgumentNullException>(() => items.HuffmanEncode(getWeight, string.Empty, appendLeft, null));
		}

		[Test]
		public void Given0ItemsWhenEncodeThenReturnEmpty()
		{
			// arrange
			var items = new List<SimpleNode>();

			// act
			var codes = items.HuffmanEncode(getWeight, string.Empty, appendLeft, appendRight);

			// assert
			Assert.That(codes.Count, Is.EqualTo(0));
		}

		[Test]
		public void Given1ItemWhenEncodeThenReturnSimple()
		{
			// arrange
			var items = new List<SimpleNode> { new SimpleNode() };

			// act
			var codes = items.HuffmanEncode(getWeight, string.Empty, appendLeft, appendRight);

			// assert
			Assert.That(codes.Keys.Single(), Is.SameAs(items[0]));
		}

		[Test]
		public void Given2ItemsWithSameWeightWhenEncodeThenReturnSimple()
		{
			// arrange
			var items = new List<SimpleNode>
			{
				new SimpleNode("L", 1),
				new SimpleNode("R", 1),
			};

			// act
			var codes = items.HuffmanEncode(getWeight, string.Empty, appendLeft, appendRight);

			// assert
			Assert.That(codes.Count, Is.EqualTo(2));
			foreach (var item in items)
			{
				Assert.That(codes[item], Is.EqualTo(item.Name));
			}
		}

		[Test]
		public void Given2ItemsWithDifferentWeightWhenEncodeThenReturnSimple()
		{
			// arrange
			var items = new List<SimpleNode>
			{
				new SimpleNode("L", 1),
				new SimpleNode("R", 1000),
			};

			// act
			var codes = items.HuffmanEncode(getWeight, string.Empty, appendLeft, appendRight);

			// assert
			Assert.That(codes.Count, Is.EqualTo(2));
			foreach (var item in items)
			{
				Assert.That(codes[item], Is.EqualTo(item.Name));
			}
		}

		[Test]
		public void GivenItemsWithTheSameWeightWhenEncodeThenReturnBalanceTree()
		{
			// arrange
			var items = new List<SimpleNode>
			{
				new SimpleNode("LLL", 1),
				new SimpleNode("LLR", 1),
				new SimpleNode("LRL", 1),
				new SimpleNode("LRR", 1),
				new SimpleNode("RLL", 1),
				new SimpleNode("RLR", 1),
				new SimpleNode("RRL", 1),
				new SimpleNode("RRR", 1),
			};

			// act
			var codes = items.HuffmanEncode(getWeight, string.Empty, appendLeft, appendRight);

			// assert
			Assert.That(codes.Count, Is.EqualTo(8));
			foreach (var item in items)
			{
				Assert.That(codes[item], Is.EqualTo(item.Name));
			}
		}

		[Test]
		public void GivenItemsWithDisperseWeightWhenEncodeThenReturnOneSideTree()
		{
			// arrange
			var items = new List<SimpleNode>
			{
				new SimpleNode("LLL", 1),
				new SimpleNode("LLR", 10),
				new SimpleNode("LR", 100),
				new SimpleNode("R", 1000),
			};

			// act
			var codes = items.HuffmanEncode(getWeight, string.Empty, appendLeft, appendRight);

			// assert
			Assert.That(codes.Count, Is.EqualTo(4));
			foreach (var item in items)
			{
				Assert.That(codes[item], Is.EqualTo(item.Name));
			}
		}

		private static ulong getWeight(SimpleNode item)
		{
			return item.Weight;
		}

		private static string appendLeft(string code)
		{
			return code + "L";
		}

		private static string appendRight(string code)
		{
			return code + "R";
		}

		private class SimpleNode
		{
			public string Name
			{ get; }

			public ulong Weight
			{ get; }

			public SimpleNode(string name, ulong weight)
			{
				Name = name;
				Weight = weight;
			}

			public SimpleNode()
				: this(string.Empty, 0)
			{ }

			public override string ToString()
			{
				return $"{Name}:{Weight}";
			}
		}
	}
}
