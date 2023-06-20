using AabSemantics.Questions;

namespace AabSemantics.Modules.Classification.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static IAnswer WhichAncestorsHas(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateAncestorsQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichDescendantsHas(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateDescendantsQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfIs(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			var question = new IsQuestion(child, parent, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}
	}
}
