using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Tests.Text
{
	[TestFixture]
	public class ContainersTest
	{
		[Test]
		public void CreateEmptyBulletsContainer()
		{
			// act
			var container = new BulletsContainer();

			// assert
			Assert.AreEqual(0, container.Items.Count);
		}

		[Test]
		public void CreateOneItemBulletsContainer()
		{
			// arrange
			var item = new SpaceText();

			// act
			var container = new BulletsContainer(item);

			// assert
			Assert.AreSame(item, container.Items.Single());
		}

		[Test]
		public void CreateManyItemsBulletsContainer()
		{
			// arrange
			var items = new List<IText>
			{
				new SpaceText(),
				new LineBreakText(),
				new FormattedText(language => string.Empty),
			};

			// act
			var container = new BulletsContainer(items);

			// assert
			Assert.IsTrue(items.SequenceEqual(container.Items));
		}

		[Test]
		public void CreateEmptyNumberingContainer()
		{
			// act
			var container = new NumberingContainer();

			// assert
			Assert.AreEqual(0, container.Items.Count);
		}

		[Test]
		public void CreateOneItemNumberingContainer()
		{
			// arrange
			var item = new SpaceText();

			// act
			var container = new NumberingContainer(item);

			// assert
			Assert.AreSame(item, container.Items.Single());
		}

		[Test]
		public void CreateManyItemsNumberingContainer()
		{
			// arrange
			var items = new List<IText>
			{
				new SpaceText(),
				new LineBreakText(),
				new FormattedText(language => string.Empty),
			};

			// act
			var container = new NumberingContainer(items);

			// assert
			Assert.IsTrue(items.SequenceEqual(container.Items));
		}

		[Test]
		public void CreateEmptyUnstructuredContainer()
		{
			// act
			var container = new UnstructuredContainer();

			// assert
			Assert.AreEqual(0, container.Items.Count);
		}

		[Test]
		public void CreateOneItemUnstructuredContainer()
		{
			// arrange
			var item = new SpaceText();

			// act
			var container = new UnstructuredContainer(item);

			// assert
			Assert.AreSame(item, container.Items.Single());
		}

		[Test]
		public void CreateManyItemsUnstructuredContainer()
		{
			// arrange
			var items = new List<IText>
			{
				new SpaceText(),
				new LineBreakText(),
				new FormattedText(language => string.Empty),
			};

			// act
			var container = new UnstructuredContainer(items);

			// assert
			Assert.IsTrue(items.SequenceEqual(container.Items));
		}
	}
}
