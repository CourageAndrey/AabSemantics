using System;
using System.Collections.Generic;

namespace AabSemantics.Text.Primitives
{
	/// <summary>A single space.</summary>
	public class SpaceText : TextBase
	{
		/// <summary>Returns an empty map: a space references nothing.</summary>
		/// <returns>An empty map.</returns>
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return new Dictionary<String, IKnowledge>();
		}
	}
}
