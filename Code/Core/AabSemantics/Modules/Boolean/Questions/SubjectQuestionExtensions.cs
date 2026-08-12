using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Boolean.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static async Task<IAnswer> IsTrueThatAsync(this QuestionBuilder builder, IStatement statement)
		{
			var question = new CheckStatementQuestion(statement, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static IAnswer IsTrueThat(this QuestionBuilder builder, IStatement statement)
		{
			return TaskHelper.AwaitDetached(() => builder.IsTrueThatAsync(statement));
		}
	}
}
