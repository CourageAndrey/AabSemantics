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
			var arcs = Array.Empty<IArcWithLength<SimpleNode>>();

			// act & assert
			Assert.Throws<ArgumentNullException>(() => arcs.FindShortestPaths(null));
		}

		[Test]
		public void GivenNegativeLengthArcWhenTryToFindShortestPathsThenThrowError()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");

			var arcs = new IArcWithLength<SimpleNode>[]
			{
				new SimpleArcWithLength<SimpleNode>(start, a, -1),
			};

			// act & assert
			Assert.Throws<InvalidOperationException>(() => arcs.FindShortestPaths(start));
		}

		[Test]
		public void GivenNoArcsWhenFindShortestPathsThenReturnEmpty()
		{
			// arrange
			var start = new SimpleNode("START");

			var arcs = Array.Empty<IArcWithLength<SimpleNode>>();

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.That(foundPaths.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenNoArcsFromStartWhenFindShortestPathsThenReturnEmpty()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");

			var arcs = new IArcWithLength<SimpleNode>[]
			{
				new SimpleArcWithLength<SimpleNode>(a, b, 10),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.That(foundPaths.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSingleStepPathsWhenFindShortestPathsThenReturnSimple()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArcWithLength<SimpleNode>[]
			{
				new SimpleArcWithLength<SimpleNode>(start, a, 1),
				new SimpleArcWithLength<SimpleNode>(start, b, 2),
				new SimpleArcWithLength<SimpleNode>(start, c, 3),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.That(foundPaths.Count, Is.EqualTo(3));
			Assert.That(foundPaths.All(p => p.From == start && p.Path.Count == 1), Is.True);
			Assert.That(foundPaths.First(p => p.To == a).Lenght, Is.EqualTo(1));
			Assert.That(foundPaths.First(p => p.To == b).Lenght, Is.EqualTo(2));
			Assert.That(foundPaths.First(p => p.To == c).Lenght, Is.EqualTo(3));
		}

		[Test]
		public void GivenChainWhenFindShortestPathsThenReturnSimple()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArcWithLength<SimpleNode>[]
			{
				new SimpleArcWithLength<SimpleNode>(start, a, 1),
				new SimpleArcWithLength<SimpleNode>(a, b, 2),
				new SimpleArcWithLength<SimpleNode>(b, c, 3),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.That(foundPaths.Count, Is.EqualTo(3));
			Assert.That(foundPaths.First(p => p.To == a).Lenght, Is.EqualTo(1));
			Assert.That(foundPaths.First(p => p.To == b).Lenght, Is.EqualTo(3));
			Assert.That(foundPaths.First(p => p.To == c).Lenght, Is.EqualTo(6));
		}

		[Test]
		public void GivenManyPathsWhenFindShortestPathsThenReturnShortest()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArcWithLength<SimpleNode>[]
			{
				new SimpleArcWithLength<SimpleNode>(start, a, 1),
				new SimpleArcWithLength<SimpleNode>(start, a, 2),
				new SimpleArcWithLength<SimpleNode>(start, a, 3),
				new SimpleArcWithLength<SimpleNode>(a, b, 1),
				new SimpleArcWithLength<SimpleNode>(a, b, 2),
				new SimpleArcWithLength<SimpleNode>(a, b, 3),
				new SimpleArcWithLength<SimpleNode>(b, c, 1),
				new SimpleArcWithLength<SimpleNode>(b, c, 2),
				new SimpleArcWithLength<SimpleNode>(b, c, 3),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.That(foundPaths.Count, Is.EqualTo(3));
			Assert.That(foundPaths.First(p => p.To == a).Lenght, Is.EqualTo(1));
			Assert.That(foundPaths.First(p => p.To == b).Lenght, Is.EqualTo(2));
			Assert.That(foundPaths.First(p => p.To == c).Lenght, Is.EqualTo(3));
		}

		[Test]
		public void GivenCyclesWhenFindShortestPathsThenWorksFine()
		{
			// arrange
			var start = new SimpleNode("START");
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new IArcWithLength<SimpleNode>[]
			{
				new SimpleArcWithLength<SimpleNode>(start, a, 1),
				new SimpleArcWithLength<SimpleNode>(start, b, 10),
				new SimpleArcWithLength<SimpleNode>(start, c, 10),
				new SimpleArcWithLength<SimpleNode>(a, b, 1),
				new SimpleArcWithLength<SimpleNode>(b, c, 1),
			};

			// act
			var foundPaths = arcs.FindShortestPaths(start);

			// assert
			Assert.That(foundPaths.Count, Is.EqualTo(3));
			Assert.That(foundPaths.First(p => p.To == a).Lenght, Is.EqualTo(1));
			Assert.That(foundPaths.First(p => p.To == b).Lenght, Is.EqualTo(2));
			Assert.That(foundPaths.First(p => p.To == c).Lenght, Is.EqualTo(3));
		}
	}
}
