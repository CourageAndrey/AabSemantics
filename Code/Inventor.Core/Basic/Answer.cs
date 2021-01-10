using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public class Answer : IAnswer
	{
		#region Properties

		public Object Result
		{ get; }

		public FormattedText Description
		{ get; }

		public IExplanation Explanation
		{ get; }

		#endregion

		public Answer(Object result, FormattedText description, IExplanation explanation)
		{
			Result = result;
			Description = description;
			Explanation = explanation;
		}

		public static IAnswer CreateUnknown(ILanguage language)
		{
			return new Answer(
				null,
				new FormattedText(() => language.Answers.Unknown, new Dictionary<String, INamed>()),
				new Explanation(new Statement[0]));
		}
	}
}
