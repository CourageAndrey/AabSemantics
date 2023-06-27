using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Tests.Text
{
	[TestFixture]
	public class NullArgumentsConstructorTest
	{
		[Test]
		public void GivenDifferentDecorators_WhenTryToCreateWithoutRequiredArguments_ThenFail()
		{
			// arrange
			IText text = null;

			// act & assert
			Assert.Throws<ArgumentNullException>(() => text.MakeBold());
			Assert.Throws<ArgumentNullException>(() => text.MakeItalic());
			Assert.Throws<ArgumentNullException>(() => text.MakeUnderline());
			Assert.Throws<ArgumentNullException>(() => text.MakeStrikeout());
			Assert.Throws<ArgumentNullException>(() => text.MakeSubscript());
			Assert.Throws<ArgumentNullException>(() => text.MakeSuperscript());
			Assert.Throws<ArgumentNullException>(() => text.MakeHeader(0));
			Assert.Throws<ArgumentNullException>(() => text.MakeParagraph());
		}

		[Test]
		public void GivenDifferentContainers_WhenTryToCreateWithoutRequiredArguments_ThenFail()
		{
			// arrange
			IList<IText> items = null;

			// act & assert
			Assert.Throws<ArgumentNullException>(() => new UnstructuredContainer(items));
			Assert.Throws<ArgumentNullException>(() => new BulletsContainer(items));
			Assert.Throws<ArgumentNullException>(() => new NumberingContainer(items));
		}

		[Test]
		public void GivenFormattedText_WhenTryToCreateWithoutRequiredArguments_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new FormattedText(null, new Dictionary<string, IKnowledge>()));
		}
	}
}
