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
        { get { return formatter; } }

        public IDictionary<string, INamed> Parameters
        { get { return parameters; } }

        private readonly Func<string> formatter;
        private readonly IDictionary<string, INamed> parameters;

        #endregion

        public FormattedLine(Func<string> formatter, IDictionary<string, INamed> parameters)
        {
            this.formatter = formatter;
            this.parameters = new Dictionary<string, INamed>(parameters);
        }

        public string GetPlainText()
        {
            string result = formatter();
            foreach (var parameter in parameters)
            {
                result = result.Replace(parameter.Key, string.Format("\"{0}\"", parameter.Value.Name.Value));
            }
            return result;
        }

        public string GetHtml(int lineNumber)
        {
            string result = formatter();
            foreach (var parameter in parameters)
            {
                result = result.Replace(
                    parameter.Key,
                    String.Format("<a href=\"{0}\">{1}</a>", GetParam(lineNumber, parameter.Key), HttpUtility.HtmlEncode(parameter.Value.Name.Value)));
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

        private readonly List<FormattedLine> lines = new List<FormattedLine>();

        public int LinesCount
        { get { return lines.Count; } }

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
            return Strings.TostringFormatted + " : " + GetPlainText();
        }

        #region Text

        public void Add(FormattedLine line)
        {
            lines.Add(line);
        }

        public void Add(Func<string> formatter, IDictionary<string, INamed> parameters)
        {
            lines.Add(new FormattedLine(formatter, parameters));
        }

        public StringBuilder GetPlainText()
        {
            var result = new StringBuilder();
            foreach (var line in lines)
            {
                result.AppendLine(line.GetPlainText());
                result.AppendLine();
            }
            return result;
        }

        public StringBuilder GetHtml()
        {
            var result = new StringBuilder(@"<html><head><title>Inventor</title></head><body>");
            result.AppendLine();
            for (int l = 0; l < lines.Count; l++)
            {
                result.AppendLine(lines[l].GetHtml(l));
            }
            result.Append(@"</body></html>");
            return result;
        }

        public IDictionary<string, INamed> GetAllParameters()
        {
            var result = new SortedDictionary<string, INamed>();
            for (int l = 0; l < lines.Count; l++)
            {
                var line = lines[l];
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
