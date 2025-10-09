using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Mathematics.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static async Task<IAnswer> HowComparedAsync(this QuestionBuilder builder, IConcept leftValue, IConcept rightValue)
		{
			var question = new ComparisonQuestion(leftValue, rightValue, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context).ConfigureAwait(false);
		}

		public static IAnswer HowCompared(this QuestionBuilder builder, IConcept leftValue, IConcept rightValue)
		{
			return builder.HowComparedAsync(leftValue, rightValue).Await();
		}
	}
}
