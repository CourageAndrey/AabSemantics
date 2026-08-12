using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static async Task<IAnswer> WhichAncestorsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateAncestorsQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhichDescendantsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateDescendantsQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> IfIsAsync(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			var question = new IsQuestion(child, parent, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichAncestorsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichAncestorsHasAsync(concept));
		}

		public static IAnswer WhichDescendantsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichDescendantsHasAsync(concept));
		}

		public static IAnswer IfIs(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsAsync(child, parent));
		}
	}
}
