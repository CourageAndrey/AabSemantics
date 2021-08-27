using System;
using System.Collections.Generic;

namespace Inventor.Core.Text
{
	public class TextBlock : TextBase
	{
		#region Properties

		public Func<ILanguage, String> Formatter
		{ get { return _formatter; } }

		public IDictionary<String, IKnowledge> Parameters
		{ get { return _parameters; } }

		private readonly Func<ILanguage, String> _formatter;
		private readonly IDictionary<String, IKnowledge> _parameters;

		#endregion

		public TextBlock(Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters)
		{
			_formatter = formatter;
			_parameters = new Dictionary<String, IKnowledge>(parameters);
		}
	}
}