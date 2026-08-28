using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Questions
{
	/// <summary>Fluent verbs for the classification module's questions.</summary>
	public static class SubjectQuestionExtensions
	{
		/// <summary>Asks which concepts the given one is a kind of.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept whose ancestors are asked for.</param>
		/// <returns>A concept-list answer.</returns>
		public static Task<IAnswer> WhichAncestorsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateAncestorsQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks which concepts are a kind of the given one.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept whose descendants are asked for.</param>
		/// <returns>A concept-list answer.</returns>
		public static Task<IAnswer> WhichDescendantsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateDescendantsQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks whether one concept is a kind of another.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="child">Concept asked about.</param>
		/// <param name="parent">Concept it is checked against.</param>
		/// <returns>A yes/no answer.</returns>
		public static Task<IAnswer> IfIsAsync(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			var question = new IsQuestion(child, parent, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Blocking counterpart of <see cref="WhichAncestorsHasAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept whose ancestors are asked for.</param>
		/// <returns>A concept-list answer.</returns>
		public static IAnswer WhichAncestorsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichAncestorsHasAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="WhichDescendantsHasAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept whose descendants are asked for.</param>
		/// <returns>A concept-list answer.</returns>
		public static IAnswer WhichDescendantsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichDescendantsHasAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="IfIsAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="child">Concept asked about.</param>
		/// <param name="parent">Concept it is checked against.</param>
		/// <returns>A yes/no answer.</returns>
		public static IAnswer IfIs(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsAsync(child, parent));
		}
	}
}
