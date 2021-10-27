using System.Collections.Generic;

namespace Inventor.Semantics.Statements
{
	public class Contradiction
	{
		#region  Properties

		public IConcept Value1
		{ get; }

		public IConcept Value2
		{ get; }

		public List<IConcept> Signs
		{ get; }

		#endregion

		public Contradiction(IConcept value1, IConcept value2, IEnumerable<IConcept> signs)
		{
			Value1 = value1;
			Value2 = value2;
			Signs = new List<IConcept>(signs);
		}
	}
}