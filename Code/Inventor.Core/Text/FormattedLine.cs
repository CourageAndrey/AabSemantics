using System;
using System.Collections.Generic;
using System.Web;

namespace Inventor.Core.Text
{
	public class FormattedLine : IText
	{
		#region Properties

		public Func<ILanguage, String> Formatter
		{ get { return _formatter; } }

		public IDictionary<String, INamed> Parameters
		{ get { return _parameters; } }

		private readonly Func<ILanguage, String> _formatter;
		private readonly IDictionary<String, INamed> _parameters;

		#endregion

		public FormattedLine(Func<ILanguage, String> formatter, IDictionary<String, INamed> parameters)
		{
			_formatter = formatter;
			_parameters = new Dictionary<String, INamed>(parameters);
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

		public String GetHtml(ILanguage language, Int32 lineNumber)
		{
			String result = _formatter(language);
			foreach (var parameter in _parameters)
			{
				result = result.Replace(
					parameter.Key,
					String.Format("<a href=\"{0}\">{1}</a>", GetParam(lineNumber, parameter.Key), HttpUtility.HtmlEncode(parameter.Value.Name.GetValue(language))));
			}
			return result + @"<br/><br/>";
		}

		public static String GetParam(Int32 line, String param)
		{
			return String.Format("#{0:00000000}{1}", line, param.Remove(0, 1));
		}
	}
}