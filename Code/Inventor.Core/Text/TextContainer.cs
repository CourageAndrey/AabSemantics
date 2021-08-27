﻿using System;
using System.Collections.Generic;
using System.Text;

using Inventor.Core.Localization;

namespace Inventor.Core.Text
{
	public class TextContainer : ITextContainer
	{
		#region Properties

		public IList<IText> Children
		{ get { return _lines; } }

#warning Merge into property above
		private readonly List<IText> _lines = new List<IText>();

		#endregion

		#region Constructors

		public TextContainer()
		{ }

		public TextContainer(Func<ILanguage, String> formatter, IDictionary<String, INamed> parameters)
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
			_lines.Add(new TextBlock(formatter, parameters));
		}

		public void AddEmptyLine()
		{
			Add(language => String.Empty, new Dictionary<String, INamed>());
		}

		public StringBuilder GetPlainText(ILanguage language)
		{
			var result = new StringBuilder();
			foreach (TextBlock line in _lines)
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
				result.AppendLine(((TextBlock) _lines[l]).GetHtml(language, l));
			}
			result.Append(@"</body></html>");
			return result;
		}

		public IDictionary<String, INamed> GetAllParameters()
		{
			var result = new SortedDictionary<String, INamed>();
			for (Int32 l = 0; l < _lines.Count; l++)
			{
				var line = (TextBlock) _lines[l];
				foreach (var parameter in line.Parameters)
				{
					result[TextBlock.GetParam(l, parameter.Key)] = parameter.Value;
				}
			}
			return result;
		}

		#endregion
	}
}