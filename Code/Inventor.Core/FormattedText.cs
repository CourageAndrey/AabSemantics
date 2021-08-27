using System;
using System.Collections.Generic;
using System.Text;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public class FormattedText : IText
	{
		#region Properties

		private readonly List<IText> _lines = new List<IText>();

		public Int32 LinesCount
		{ get { return _lines.Count; } }

		#endregion

		#region Constructors

		public FormattedText()
		{ }

		public FormattedText(Func<ILanguage, String> formatter, IDictionary<String, INamed> parameters)
		{
			Add(formatter, parameters);
		}

		#endregion

		public override String ToString()
		{
			return Strings.TostringFormatted + " : " + GetPlainText(Language.Default);
		}

		#region Text

		public void Add(IText line)
		{
			_lines.Add(line);
		}

		public void Add(Func<ILanguage, String> formatter, IDictionary<String, INamed> parameters)
		{
			_lines.Add(new FormattedLine(formatter, parameters));
		}

		public void AddEmptyLine()
		{
			Add(language => String.Empty, new Dictionary<String, INamed>());
		}

		public StringBuilder GetPlainText(ILanguage language)
		{
			var result = new StringBuilder();
			foreach (FormattedLine line in _lines)
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
			for (Int32 l = 0; l < _lines.Count; l++)
			{
				result.AppendLine(((FormattedLine) _lines[l]).GetHtml(language, l));
			}
			result.Append(@"</body></html>");
			return result;
		}

		public IDictionary<String, INamed> GetAllParameters()
		{
			var result = new SortedDictionary<String, INamed>();
			for (Int32 l = 0; l < _lines.Count; l++)
			{
				var line = (FormattedLine) _lines[l];
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
