using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Inventor.Algorithms.Test
{
	[TestFixture]
	public class FordFulkersonTest
	{
		[Test]
		public void GivenNullArcsWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var source = new SimpleNode("START");
			var destination = new SimpleNode("DESTINATION");

			// act & assert
			Assert.Throws<ArgumentNullException>(() => FordFulkerson.FindMaximumFlow(null, source, destination));
			Assert.Throws<ArgumentNullException>(() => FordFulkerson.FindMaximumFlow<IArcWithFlow<SimpleNode>>(null));
		}

		[Test]
		public void GivenNullSourceWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var arcs = new List<IArcWithFlow<SimpleNode>>();
			var destination = new SimpleNode("DESTINATION");

			// act & assert
			Assert.Throws<ArgumentNullException>(() => arcs.FindMaximumFlow(null, destination));
		}

		[Test]
		public void GivenNullDestinationWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var arcs = new List<IArcWithFlow<SimpleNode>>();
			var source = new SimpleNode("START");

			// act & assert
			Assert.Throws<ArgumentNullException>(() => arcs.FindMaximumFlow(source, null));
		}

		[Test]
		public void GivenOneArcWhenFindMaximumFlowThenReturnItsFlow()
		{
			// arrange
			var source = new SimpleNode("START");
			var destination = new SimpleNode("DESTINATION");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(source, destination, 123),
			};

			// act
			double maxFlow = arcs.FindMaximumFlow(source, destination);

			// assert
			Assert.That(maxFlow, Is.EqualTo(123));
		}

		[Test]
		public void GivenLinearChainWhenFindMaximumFlowThenReturnMinimumFlow()
		{
			// arrange
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");
			var d = new SimpleNode("D");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(a, b, 567),
				new SimpleArcWithFlow<SimpleNode>(b, c, 123),
				new SimpleArcWithFlow<SimpleNode>(c, d, 234),
			};

			// act
			double maxFlow = arcs.FindMaximumFlow(a, d);

			// assert
			Assert.That(maxFlow, Is.EqualTo(123));
		}

		[Test]
		public void GivenParallelArcsWhenFindMaximumFlowThenReturnTheirSummary()
		{
			// arrange
			var source = new SimpleNode("START");
			var destination = new SimpleNode("DESTINATION");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(source, destination, 123),
				new SimpleArcWithFlow<SimpleNode>(source, destination, 456),
				new SimpleArcWithFlow<SimpleNode>(source, destination, 789),
			};

			// act
			double maxFlow = arcs.FindMaximumFlow(source, destination);

			// assert
			Assert.That(maxFlow, Is.EqualTo(123 + 456 + 789));
		}

		[Test]
		public void GivenComplexGraphWhenFindMaximumFlowThenReturnIt()
		{
			// arrange
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");
			var d = new SimpleNode("D");
			var e = new SimpleNode("E");
			var f = new SimpleNode("F");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(a, b, 7),
				new SimpleArcWithFlow<SimpleNode>(a, c, 4),
				new SimpleArcWithFlow<SimpleNode>(b, c, 2),
				new SimpleArcWithFlow<SimpleNode>(b, e, 4),
				new SimpleArcWithFlow<SimpleNode>(c, d, 4),
				new SimpleArcWithFlow<SimpleNode>(c, e, 8),
				new SimpleArcWithFlow<SimpleNode>(d, f, 12),
				new SimpleArcWithFlow<SimpleNode>(e, d, 4),
				new SimpleArcWithFlow<SimpleNode>(e, f, 5),
			};

			// act
			double maxFlow = arcs.FindMaximumFlow(a, f);

			// assert
			Assert.That(maxFlow, Is.EqualTo(10));
		}

		/*[Test]
		public void Given1111111111111111WhenFindMaximumFlowThenReturn222222222222222()
		{
			// arrange
			var source = new SimpleNode("START");
			var destination = new SimpleNode("DESTINATION");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(, , ),
			};

			// act
			double maxFlow = arcs.FindMaximumFlow(source, destination);

			// assert
			Assert.That(maxFlow, Is.EqualTo());
		}*/

		[Test]
		public void GivenNoArcsWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var arcs = new List<IArcWithFlow<SimpleNode>>();

			// act & assert
			Assert.Throws<ArgumentException>(() => arcs.FindMaximumFlow());
		}

		[Test]
		public void GivenNoSourceLoopsWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(a, b, 1),
				new SimpleArcWithFlow<SimpleNode>(a, c, 1),
				new SimpleArcWithFlow<SimpleNode>(c, a, 1),
			};

			// act & assert
			Assert.Throws<ArgumentException>(() => arcs.FindMaximumFlow());
		}

		[Test]
		public void GivenNoDestinationLoopsWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");
			var c = new SimpleNode("C");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(a, b, 1),
				new SimpleArcWithFlow<SimpleNode>(b, c, 1),
				new SimpleArcWithFlow<SimpleNode>(c, b, 1),
			};

			// act & assert
			Assert.Throws<ArgumentException>(() => arcs.FindMaximumFlow());
		}

		[Test]
		public void GivenCycledLoopsWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var a = new SimpleNode("A");
			var b = new SimpleNode("B");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(a, b, 1),
				new SimpleArcWithFlow<SimpleNode>(b, a, 1),
			};

			// act & assert
			Assert.Throws<ArgumentException>(() => arcs.FindMaximumFlow());
		}

		[Test]
		public void GivenTwoSourcesWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var source1 = new SimpleNode("START_1");
			var source2 = new SimpleNode("START_2");
			var destination = new SimpleNode("DESTINATION");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(source1, destination, 1),
				new SimpleArcWithFlow<SimpleNode>(source2, destination, 1),
			};

			// act & assert
			Assert.Throws<ArgumentException>(() => arcs.FindMaximumFlow());
		}

		[Test]
		public void GivenTwoDestinationsWhenTryToFindMaximumFlowThenThrowError()
		{
			// arrange
			var source = new SimpleNode("START");
			var destination1 = new SimpleNode("DESTINATION_1");
			var destination2 = new SimpleNode("DESTINATION_2");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(source, destination1, 1),
				new SimpleArcWithFlow<SimpleNode>(source, destination2, 1),
			};

			// act & assert
			Assert.Throws<ArgumentException>(() => arcs.FindMaximumFlow());
		}

		[Test]
		public void GivenSingleSourceSingleDestinationWhenFindMaximumFlowThenWorksFine()
		{
			// arrange
			var source = new SimpleNode("START");
			var destination = new SimpleNode("DESTINATION");

			var arcs = new List<IArcWithFlow<SimpleNode>>
			{
				new SimpleArcWithFlow<SimpleNode>(source, destination, 1),
			};

			// act & assert
			Assert.DoesNotThrow(() => arcs.FindMaximumFlow());
		}
	}
}
