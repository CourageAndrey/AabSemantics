﻿using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public class Answer
	{
		#region Properties

		public Object Result
		{ get; }

		public FormattedText Description
		{ get; }

		public Explanation Explanation
		{ get; }

		#endregion

		public Answer(Object result, FormattedText description, Explanation explanation)
		{
			Result = result;
			Description = description;
			Explanation = explanation;
		}

		public static Answer CreateUnknown(ILanguage language)
		{
			return new Answer(
				null,
				new FormattedText(() => language.Answers.Unknown, new Dictionary<string, INamed>()),
				new Explanation(new Statement[0]));
		}
	}
}
