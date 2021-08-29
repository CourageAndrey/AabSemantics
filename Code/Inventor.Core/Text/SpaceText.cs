using System;
using System.Collections.Generic;

namespace Inventor.Core.Text
{
	public class SpaceText : TextBase
	{
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return new Dictionary<string, IKnowledge>();
		}
	}
}
