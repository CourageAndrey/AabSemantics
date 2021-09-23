using Inventor.Core.Questions;

namespace Inventor.Core.Modules.Boolean.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static IAnswer IsTrueThat(this QuestionBuilder builder, IStatement statement)
		{
			var question = new CheckStatementQuestion(statement, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}
	}
}
