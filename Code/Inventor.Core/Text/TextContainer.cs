﻿using System;
using System.Collections.Generic;

namespace Inventor.Core.Text
{
	public class TextContainer : TextBase, ITextContainer
	{
		#region Properties

		public IList<IText> Children
		{ get; } = new List<IText>();

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
			Children.Add(line);
		}

		public void Add(Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters)
		{
			Children.Add(new TextBlock(formatter, parameters));
		}

		public void AddEmptyLine()
		{
			Add(language => String.Empty, new Dictionary<String, IKnowledge>());
		}

		public override IDictionary<String, IKnowledge> GetParameters()
		{
			var result = new SortedDictionary<String, IKnowledge>();
			foreach (var line in Children)
			{
				foreach (var parameter in line.GetParameters())
				{
					result[parameter.Value.ID] = parameter.Value;
				}
			}
			return result;
		}

		#endregion
	}
}