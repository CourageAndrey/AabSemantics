using System.Collections.Generic;

namespace AabSemantics.Text.Containers
{
	/// <summary>Renders its items as an unordered list.</summary>
	public class BulletsContainer : TextContainerBase
	{
		#region Constructors

		/// <summary>Creates a list over an existing collection, used directly rather than copied.</summary>
		/// <param name="items">List entries.</param>
		public BulletsContainer(IList<IText> items)
			: base(items)
		{ }

		/// <summary>Creates a single-entry list.</summary>
		/// <param name="item">The only entry.</param>
		public BulletsContainer(IText item)
			: this(new List<IText> { item })
		{ }

		/// <summary>Creates an empty list, to be filled afterwards.</summary>
		public BulletsContainer()
			: this(new List<IText>())
		{ }

		#endregion
	}
}
