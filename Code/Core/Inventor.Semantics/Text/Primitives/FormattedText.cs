using System;
using System.Collections.Generic;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Text.Primitives
{
	public class FormattedText : TextBase
	{
		#region Properties

		public Func<ILanguage, String> Formatter
		{ get; }

		public IDictionary<String, IKnowledge> Parameters
		{ get; }

		#endregion

		public FormattedText(Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters = null)
		{
			Formatter = formatter.EnsureNotNull(nameof(formatter));
			Parameters = parameters != null
				? new Dictionary<String, IKnowledge>(parameters)
				: new Dictionary<String, IKnowledge>();
		}

		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return Parameters;
		}
	}
}