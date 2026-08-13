using System;
using System.Collections.Generic;

namespace AabSemantics.Text.Primitives
{
	/// <summary>A line break.</summary>
	public class LineBreakText : TextBase
	{
		/// <summary>Returns an empty map: a line break references nothing.</summary>
		/// <returns>An empty map.</returns>
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return new Dictionary<String, IKnowledge>();
		}
	}
}
