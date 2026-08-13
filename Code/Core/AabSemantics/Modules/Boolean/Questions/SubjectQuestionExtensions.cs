using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Boolean.Questions
{
	/// <summary>Fluent verbs for the boolean module's questions.</summary>
	public static class SubjectQuestionExtensions
	{
		/// <summary>Asks whether a statement holds.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="statement">Statement to check.</param>
		/// <returns>A yes/no answer.</returns>
		public static async Task<IAnswer> IsTrueThatAsync(this QuestionBuilder builder, IStatement statement)
		{
			var question = new CheckStatementQuestion(statement, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		/// <summary>Blocking counterpart of <see cref="IsTrueThatAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="statement">Statement to check.</param>
		/// <returns>A yes/no answer.</returns>
		public static IAnswer IsTrueThat(this QuestionBuilder builder, IStatement statement)
		{
			return TaskHelper.AwaitDetached(() => builder.IsTrueThatAsync(statement));
		}
	}
}
