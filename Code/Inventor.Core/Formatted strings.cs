using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public sealed class FormattedLine
	{
		#region Properties

		public Func<string> Formatter
		{ get { return _formatter; } }

		public IDictionary<string, INamed> Parameters
		{ get { return _parameters; } }

		private readonly Func<string> _formatter;
		private readonly IDictionary<string, INamed> _parameters;

		#endregion

		public FormattedLine(Func<string> formatter, IDictionary<string, INamed> parameters)
		{
			_formatter = formatter;
			_parameters = new Dictionary<string, INamed>(parameters);
		}

		public string GetPlainText(ILanguage language)
		{
			string result = _formatter();
			foreach (var parameter in _parameters)
			{
				result = result.Replace(parameter.Key, string.Format("\"{0}\"", parameter.Value.Name.GetValue(language)));
			}
			return result;
		}

		public string GetHtml(ILanguage language, int lineNumber)
		{
			string result = _formatter();
			foreach (var parameter in _parameters)
			{
				result = result.Replace(
					parameter.Key,
					String.Format("<a href=\"{0}\">{1}</a>", GetParam(lineNumber, parameter.Key), HttpUtility.HtmlEncode(parameter.Value.Name.GetValue(language))));
			}
			return result + @"<br/><br/>";
		}

		public static string GetParam(int line, string param)
		{
			return String.Format("#{0:00000000}{1}", line, param.Remove(0, 1));
		}
	}

	public sealed class FormattedText
	{
		#region Properties

		private readonly List<FormattedLine> _lines = new List<FormattedLine>();

		public int LinesCount
		{ get { return _lines.Count; } }

		#endregion

		#region Constructors

		public FormattedText()
		{ }

		public FormattedText(Func<string> formatter, IDictionary<string, INamed> parameters)
		{
			Add(formatter, parameters);
		}

		#endregion

		public override string ToString()
		{
			return Strings.TostringFormatted + " : " + GetPlainText(Language.Default);
		}

		#region Text

		public void Add(FormattedLine line)
		{
			_lines.Add(line);
		}

		public void Add(Func<string> formatter, IDictionary<string, INamed> parameters)
		{
			_lines.Add(new FormattedLine(formatter, parameters));
		}

		public StringBuilder GetPlainText(ILanguage language)
		{
			var result = new StringBuilder();
			foreach (var line in _lines)
			{
				result.AppendLine(line.GetPlainText(language));
				result.AppendLine();
			}
			return result;
		}

		public StringBuilder GetHtml(ILanguage language)
		{
			var result = new StringBuilder(@"<html><head><title>Inventor</title></head><body>");
			result.AppendLine();
			for (int l = 0; l < _lines.Count; l++)
			{
				result.AppendLine(_lines[l].GetHtml(language, l));
			}
			result.Append(@"</body></html>");
			return result;
		}

		public IDictionary<string, INamed> GetAllParameters()
		{
			var result = new SortedDictionary<string, INamed>();
			for (int l = 0; l < _lines.Count; l++)
			{
				var line = _lines[l];
				foreach (var parameter in line.Parameters)
				{
					result[FormattedLine.GetParam(l, parameter.Key)] = parameter.Value;
				}
			}
			return result;
		}

		#endregion
	}
}
