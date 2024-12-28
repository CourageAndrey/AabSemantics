using System;

namespace AabSemantics.Modules.NaturalScience.Attributes
{
	public enum Block
	{
		s,
		p,
		d,
		f,
		fBlock
	}

	public class IsChemicalElementAttribute : IAttribute
	{
		public Byte AtomicNumber
		{ get; }

		public String Symbol
		{ get; }

		public String Name
		{ get; }

		public Byte Group
		{ get; }

		public Byte Period
		{ get; }

		public Block Block
		{ get; }

		public IsChemicalElementAttribute(Byte atomicNumber, String symbol, String name, Byte group, Byte period, Block block)
		{
			AtomicNumber = atomicNumber;
			Symbol = symbol;
			Name = name;
			Group = group;
			Period = period;
			Block = block;
		}
	}
}
