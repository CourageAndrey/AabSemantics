namespace Inventor.Algorithms.Test
{
	internal class SimpleNode
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
