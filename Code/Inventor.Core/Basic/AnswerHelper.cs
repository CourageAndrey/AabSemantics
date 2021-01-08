using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public static class AnswerHelper
	{
		public static FormattedText CreateUnknown(ILanguage language)
		{
			return new FormattedText(() => language.Answers.Unknown, new Dictionary<string, INamed>());
		}
	}
}