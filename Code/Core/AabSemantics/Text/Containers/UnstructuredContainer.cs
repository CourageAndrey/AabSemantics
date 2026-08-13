using System.Collections.Generic;

namespace AabSemantics.Text.Containers
{
	/// <summary>Renders its items one after another, without any list markup.</summary>
	public class UnstructuredContainer : TextContainerBase
	{
		#region Constructors

		/// <summary>Creates a container over an existing collection, used directly rather than copied.</summary>
		/// <param name="items">Nested texts.</param>
		public UnstructuredContainer(IList<IText> items)
			: base(items)
		{ }

		/// <summary>Creates a container holding one text.</summary>
		/// <param name="item">The only nested text.</param>
		public UnstructuredContainer(IText item)
			: this(new List<IText> { item })
		{ }

		/// <summary>Creates an empty container, to be filled afterwards.</summary>
		public UnstructuredContainer()
			: this(new List<IText>())
		{ }

		#endregion
	}
}
