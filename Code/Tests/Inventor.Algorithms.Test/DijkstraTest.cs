using System;
using System.Linq;

using NUnit.Framework;

namespace Inventor.Algorithms.Test
{
	[TestFixture]
	public class DijkstraTest
	{
		[Test]
		public void GivenNullArcsWhenTryToFindShortestPathsThenThrowError()
		{
			// arrange
			var start = new SimpleNode("START");

			// act & assert
			Assert.Throws<ArgumentNullException>(() => Dijkstra.FindShortestPaths(null, start));
		}

		[Test]
		public void GivenNullFromNodeWhenTryToFindShortestPathsThenThrowError()
		{
			// arrange
			var arcs = Array.Empty<IArc<SimpleNode>>();

			// act & assert
			Assert.Throws<ArgumentNullException>(() => arcs.FindShortestPaths(null));
		}

		[Test]
		public void GivenNegaativeLenghtArcWhenTryToFindShortestPathsThenThrowError()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");

			var arcs = new IArc<SimpleNode>[]
			{
				new SimpleArc<SimpleNode>(start, a, -1),
			};

			// act & assert
			Assert.Throws<InvalidOperationException>(() => arcs.FindShortestPaths(start));
		}

		[Test]
		public void GivenNoArcsWhenFindShortestPathsThenReturnEmpty()
		{
			// arrange
			var start = new SimpleNode("START");

			var arcs = Array.Empty<IArc<SimpleNode>>();

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.AreEqual(0, foundPaths.Count);
		}

		[Test]
		public void GivenNoArcsFromStartWhenFindShortestPathsThenReturnEmpty()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");

			var arcs = new IArc<SimpleNode>[]
			{
				new SimpleArc<SimpleNode>(a, b, 10),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.AreEqual(0, foundPaths.Count);
		}

		[Test]
		public void GivenSingleStepPathsWhenFindShortestPathsThenReturnSimple()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArc<SimpleNode>[]
			{
				new SimpleArc<SimpleNode>(start, a, 1),
				new SimpleArc<SimpleNode>(start, b, 2),
				new SimpleArc<SimpleNode>(start, c, 3),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.AreEqual(3, foundPaths.Count);
			Assert.IsTrue(foundPaths.All(p => p.From == start && p.Path.Count == 1));
			Assert.AreEqual(1, foundPaths.First(p => p.To == a).Lenght);
			Assert.AreEqual(2, foundPaths.First(p => p.To == b).Lenght);
			Assert.AreEqual(3, foundPaths.First(p => p.To == c).Lenght);
		}

		[Test]
		public void GivenChainWhenFindShortestPathsThenReturnSimple()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArc<SimpleNode>[]
			{
				new SimpleArc<SimpleNode>(start, a, 1),
				new SimpleArc<SimpleNode>(a, b, 2),
				new SimpleArc<SimpleNode>(b, c, 3),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.AreEqual(3, foundPaths.Count);
			Assert.AreEqual(1, foundPaths.First(p => p.To == a).Lenght);
			Assert.AreEqual(3, foundPaths.First(p => p.To == b).Lenght);
			Assert.AreEqual(6, foundPaths.First(p => p.To == c).Lenght);
		}

		[Test]
		public void GivenManyPathsWhenFindShortestPathsThenReturnShortest()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArc<SimpleNode>[]
			{
				new SimpleArc<SimpleNode>(start, a, 1),
				new SimpleArc<SimpleNode>(start, a, 2),
				new SimpleArc<SimpleNode>(start, a, 3),
				new SimpleArc<SimpleNode>(a, b, 1),
				new SimpleArc<SimpleNode>(a, b, 2),
				new SimpleArc<SimpleNode>(a, b, 3),
				new SimpleArc<SimpleNode>(b, c, 1),
				new SimpleArc<SimpleNode>(b, c, 2),
				new SimpleArc<SimpleNode>(b, c, 3),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.AreEqual(3, foundPaths.Count);
			Assert.AreEqual(1, foundPaths.First(p => p.To == a).Lenght);
			Assert.AreEqual(2, foundPaths.First(p => p.To == b).Lenght);
			Assert.AreEqual(3, foundPaths.First(p => p.To == c).Lenght);
		}

		[Test]
		public void GivenCyclesWhenFindShortestPathsThenWorksFine()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArc<SimpleNode>[]
			{
				new SimpleArc<SimpleNode>(start, a, 1),
				new SimpleArc<SimpleNode>(start, b, 10),
				new SimpleArc<SimpleNode>(start, c, 10),
				new SimpleArc<SimpleNode>(a, b, 1),
				new SimpleArc<SimpleNode>(b, c, 1),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.AreEqual(3, foundPaths.Count);
			Assert.AreEqual(1, foundPaths.First(p => p.To == a).Lenght);
			Assert.AreEqual(2, foundPaths.First(p => p.To == b).Lenght);
			Assert.AreEqual(3, foundPaths.First(p => p.To == c).Lenght);
		}

		private class SimpleNode
		{
			public string Name
			{ get; }

			public SimpleNode(string name)
			{
				Name = name;
			}

			public SimpleNode()
				: this(string.Empty)
			{ }

			public override string ToString()
			{
				return Name;
			}
		}
	}
}
