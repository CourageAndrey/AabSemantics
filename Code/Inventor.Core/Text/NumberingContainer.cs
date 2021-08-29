using System.Collections.Generic;

namespace Inventor.Core.Text
{
	public class NumberingContainer : TextContainerBase
	{
		#region Constructors

		public NumberingContainer(IList<IText> items)
			: base(items)
		{ }

		public NumberingContainer(IText item)
			: this(new List<IText> { item })
		{ }

		public NumberingContainer()
			: this(new List<IText>())
		{ }

		#endregion
	}
}
