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
		public void GivenNoItems_WhenCreateBulletsContainer_ThenSucceed()
		{
			// act
			var container = new BulletsContainer();

			// assert
			Assert.That(container.Items.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSingleItem_WhenCreateBulletsContainer_ThenSucceed()
		{
			// arrange
			var item = new SpaceText();

			// act
			var container = new BulletsContainer(item);

			// assert
			Assert.That(container.Items.Single(), Is.SameAs(item));
		}

		[Test]
		public void GivenManyItems_WhenCreateBulletsContainer_ThenSucceed()
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
			Assert.That(items.SequenceEqual(container.Items), Is.True);
		}

		[Test]
		public void GivenNoItems_WhenCreateNumberingContainer_ThenSucceed()
		{
			// act
			var container = new NumberingContainer();

			// assert
			Assert.That(container.Items.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSingleItem_WhenCreateNumberingContainer_ThenSucceed()
		{
			// arrange
			var item = new SpaceText();

			// act
			var container = new NumberingContainer(item);

			// assert
			Assert.That(container.Items.Single(), Is.SameAs(item));
		}

		[Test]
		public void GivenManyItems_WhenCreateNumberingContainer_ThenSucceed()
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
			Assert.That(items.SequenceEqual(container.Items), Is.True);
		}

		[Test]
		public void GivenNoItems_WhenCreateUnstructuredContainer_ThenSucceed()
		{
			// act
			var container = new UnstructuredContainer();

			// assert
			Assert.That(container.Items.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSingleItem_WhenCreateUnstructuredContainer_ThenSucceed()
		{
			// arrange
			var item = new SpaceText();

			// act
			var container = new UnstructuredContainer(item);

			// assert
			Assert.That(container.Items.Single(), Is.SameAs(item));
		}

		[Test]
		public void GivenManyItems_WhenCreateUnstructuredContainer_ThenSucceed()
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
			Assert.That(items.SequenceEqual(container.Items), Is.True);
		}
	}
}
