using Inventor.Semantics;
using Inventor.Semantics.Questions;

namespace Samples._05.CustomStatement
{
	public static class SubjectQuestionExtensions
	{
		public static IAnswer WhoIsTallerThan(this QuestionBuilder builder, IConcept person)
		{
			var question = new GetTallerQuestion(person);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhoIsShorterThan(this QuestionBuilder builder, IConcept person)
		{
			var question = new GetShorterQuestion(person);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IsTallerThan(this QuestionBuilder builder, IConcept taller, IConcept shorter)
		{
			var question = new IsTallerQuestion(taller, shorter);
			return question.Ask(builder.SemanticNetwork.Context);
		}
	}
}
