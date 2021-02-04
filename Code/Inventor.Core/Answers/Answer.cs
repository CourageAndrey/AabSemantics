using System;
using System.Collections.Generic;

using Inventor.Core.Base;

namespace Inventor.Core.Answers
{
	public class Answer : IAnswer
	{
		#region Properties

		public FormattedText Description
		{ get; }

		public IExplanation Explanation
		{ get; }

		#endregion

		public Answer(FormattedText description, IExplanation explanation)
		{
			Description = description;
			Explanation = explanation;
		}

		public static IAnswer CreateUnknown(ILanguage language)
		{
			return new Answer(
				new FormattedText(() => language.Answers.Unknown, new Dictionary<String, INamed>()),
				new Explanation(new IStatement[0]));
		}
	}
}
