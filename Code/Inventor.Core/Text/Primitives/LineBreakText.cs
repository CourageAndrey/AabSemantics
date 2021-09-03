using System;
using System.Collections.Generic;

namespace Inventor.Core.Text.Primitives
{
	public class LineBreakText : TextBase
	{
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return new Dictionary<string, IKnowledge>();
		}
	}
}
