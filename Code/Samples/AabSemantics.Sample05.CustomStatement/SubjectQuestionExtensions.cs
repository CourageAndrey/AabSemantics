using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Sample05.CustomStatement
{
	public static class SubjectQuestionExtensions
	{
		public static async Task<IAnswer> WhoIsTallerThanAsync(this QuestionBuilder builder, IConcept person)
		{
			var question = new GetTallerQuestion(person);
			return await question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		public static async Task<IAnswer> WhoIsShorterThanAsync(this QuestionBuilder builder, IConcept person)
		{
			var question = new GetShorterQuestion(person);
			return await question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		public static async Task<IAnswer> IsTallerThanAsync(this QuestionBuilder builder, IConcept taller, IConcept shorter)
		{
			var question = new IsTallerQuestion(taller, shorter);
			return await question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		public static IAnswer WhoIsTallerThan(this QuestionBuilder builder, IConcept person)
		{
			return builder.WhoIsTallerThanAsync(person).Await();
		}

		public static IAnswer WhoIsShorterThan(this QuestionBuilder builder, IConcept person)
		{
			return builder.WhoIsShorterThanAsync(person).Await();
		}

		public static IAnswer IsTallerThan(this QuestionBuilder builder, IConcept taller, IConcept shorter)
		{
			return builder.IsTallerThanAsync(taller, shorter).Await();
		}
	}
}
