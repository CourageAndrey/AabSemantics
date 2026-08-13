using System.Collections.Generic;

namespace AabSemantics.Statements
{
	/// <summary>
	/// A pair of values a comparison chain proves to stand in incompatible relations to each
	/// other, together with the signs that were derived.
	/// </summary>
	public class Contradiction
	{
		#region  Properties

		/// <summary>First of the two compared values.</summary>
		public IConcept Value1
		{ get; }

		/// <summary>Second of the two compared values.</summary>
		public IConcept Value2
		{ get; }

		/// <summary>The mutually exclusive comparison signs derived for the pair.</summary>
		public List<IConcept> Signs
		{ get; }

		#endregion

		/// <summary>Creates a contradiction record.</summary>
		/// <param name="value1">First compared value.</param>
		/// <param name="value2">Second compared value.</param>
		/// <param name="signs">Conflicting comparison signs; copied into the record.</param>
		public Contradiction(IConcept value1, IConcept value2, IEnumerable<IConcept> signs)
		{
			Value1 = value1;
			Value2 = value2;
			Signs = new List<IConcept>(signs);
		}
	}
}