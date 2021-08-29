﻿using System.Collections.Generic;

namespace Inventor.Core.Text
{
	public class BulletsContainer : TextContainerBase
	{
		#region Constructors

		public BulletsContainer(IList<IText> items)
			: base(items)
		{ }

		public BulletsContainer(IText item)
			: this(new List<IText> { item })
		{ }

		public BulletsContainer()
			: this(new List<IText>())
		{ }

		#endregion
	}
}
