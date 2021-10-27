using Inventor.Semantics.Questions;

namespace Inventor.Semantics.Modules.Boolean.Questions
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
