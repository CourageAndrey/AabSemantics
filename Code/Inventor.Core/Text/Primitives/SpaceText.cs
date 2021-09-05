using System;
using System.Collections.Generic;

namespace Inventor.Core.Text.Primitives
{
	public class SpaceText : TextBase
	{
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return new Dictionary<String, IKnowledge>();
		}
	}
}
