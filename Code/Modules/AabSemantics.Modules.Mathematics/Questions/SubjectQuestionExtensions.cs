using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Mathematics.Questions
{
	/// <summary>Fluent verbs for the mathematics module's questions.</summary>
	public static class SubjectQuestionExtensions
	{
		/// <summary>Asks how two values compare.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="leftValue">The left-hand value.</param>
		/// <param name="rightValue">The right-hand value.</param>
		/// <returns>An answer naming the comparison sign.</returns>
		public static Task<IAnswer> HowComparedAsync(this QuestionBuilder builder, IConcept leftValue, IConcept rightValue)
		{
			var question = new ComparisonQuestion(leftValue, rightValue, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Blocking counterpart of <see cref="HowComparedAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="leftValue">The left-hand value.</param>
		/// <param name="rightValue">The right-hand value.</param>
		/// <returns>An answer naming the comparison sign.</returns>
		public static IAnswer HowCompared(this QuestionBuilder builder, IConcept leftValue, IConcept rightValue)
		{
			return TaskHelper.AwaitDetached(() => builder.HowComparedAsync(leftValue, rightValue));
		}
	}
}
