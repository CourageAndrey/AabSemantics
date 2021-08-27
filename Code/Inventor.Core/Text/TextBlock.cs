using System;
using System.Collections.Generic;

namespace Inventor.Core.Text
{
	public class TextBlock : TextBase
	{
		#region Properties

		public Func<ILanguage, String> Formatter
		{ get; }

		public IDictionary<String, IKnowledge> Parameters
		{ get; }

		#endregion

		public TextBlock(Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters)
		{
			Formatter = formatter;
			Parameters = new Dictionary<String, IKnowledge>(parameters);
		}

		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return Parameters;
		}
	}
}