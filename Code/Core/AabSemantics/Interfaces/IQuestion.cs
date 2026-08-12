using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics
{
	public interface IQuestion
	{
		ICollection<IStatement> Preconditions
		{ get; }

		Task<IAnswer> AskAsync(ISemanticNetworkContext context, ILanguage language = null);
	}

	public static class QuestionExtensions
	{
		public static IAnswer Ask(this IQuestion question, ISemanticNetworkContext context, ILanguage language = null)
		{
			return TaskHelper.AwaitDetached(() => question.AskAsync(context, language));
		}
	}
}
