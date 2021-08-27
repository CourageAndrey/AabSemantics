﻿using System;
using System.Collections.Generic;

namespace Inventor.Core.Text
{
	public class TextContainer : TextBase, ITextContainer
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

		public TextContainer(Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters)
		{
			Add(formatter, parameters);
		}

		#endregion

		#region Text

		public void Add(IText line)
		{
			_lines.Add(line);
		}

		public void Add(Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters)
		{
			_lines.Add(new TextBlock(formatter, parameters));
		}

		public void AddEmptyLine()
		{
			Add(language => String.Empty, new Dictionary<String, IKnowledge>());
		}

		public IDictionary<String, IKnowledge> GetAllParameters()
		{
			var result = new SortedDictionary<String, IKnowledge>();
			foreach (TextBlock line in _lines)
			{
				foreach (var parameter in line.Parameters)
				{
					result[parameter.Value.ID] = parameter.Value;
				}
			}
			return result;
		}

		#endregion
	}
}