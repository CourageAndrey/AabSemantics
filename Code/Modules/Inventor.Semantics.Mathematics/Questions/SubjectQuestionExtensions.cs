using Inventor.Semantics.Questions;

namespace Inventor.Semantics.Mathematics.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static IAnswer HowCompared(this QuestionBuilder builder, IConcept leftValue, IConcept rightValue)
		{
			var question = new ComparisonQuestion(leftValue, rightValue, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}
	}
}
