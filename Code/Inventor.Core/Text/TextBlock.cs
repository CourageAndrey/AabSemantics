using System;
using System.Collections.Generic;
using System.Web;

namespace Inventor.Core.Text
{
	public class TextBlock : IText
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

		public String GetPlainText(ILanguage language)
		{
			String result = _formatter(language);
			foreach (var parameter in _parameters)
			{
				result = result.Replace(parameter.Key, String.Format("\"{0}\"", parameter.Value.Name.GetValue(language)));
			}
			return result;
		}

		public String GetHtml(ILanguage language)
		{
			String result = _formatter(language);
			foreach (var parameter in _parameters)
			{
				result = result.Replace(
					parameter.Key,
					String.Format("<a href=\"{0}\">{1}</a>", parameter.Value.ID, HttpUtility.HtmlEncode(parameter.Value.Name.GetValue(language))));
			}
			return result + @"<br/><br/>";
		}
	}
}