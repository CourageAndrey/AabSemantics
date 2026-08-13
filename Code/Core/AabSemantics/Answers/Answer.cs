using System;

using AabSemantics.Text.Primitives;

namespace AabSemantics.Answers
{
	/// <summary>Base answer, carrying only text and explanation; typed answers derive from it.</summary>
	public class Answer : IAnswer
	{
		#region Properties

		/// <summary>The answer as localizable text.</summary>
		public IText Description
		{ get; }

		/// <summary>Statements the answer was derived from.</summary>
		public IExplanation Explanation
		{ get; }

		/// <summary>Whether the network held no knowledge to answer the question.</summary>
		public Boolean IsEmpty
		{ get; }

		#endregion

		/// <summary>Creates an answer.</summary>
		/// <param name="description">The answer as localizable text.</param>
		/// <param name="explanation">Statements the answer was derived from.</param>
		/// <param name="isEmpty">Whether the answer means "unknown" rather than "no".</param>
		public Answer(IText description, IExplanation explanation, Boolean isEmpty)
		{
			Description = description;
			Explanation = explanation;
			IsEmpty = isEmpty;
		}

		/// <summary>Creates the "unknown" answer, with an empty explanation.</summary>
		/// <returns>An empty answer.</returns>
		public static IAnswer CreateUnknown()
		{
			return new Answer(
				new FormattedText(language => language.Questions.Answers.Unknown),
				new Explanation(Array.Empty<IStatement>()),
				true);
		}
	}
}
