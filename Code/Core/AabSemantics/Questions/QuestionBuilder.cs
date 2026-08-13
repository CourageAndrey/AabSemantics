using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics.Questions
{
	/// <summary>
	/// Carries a network and its preconditions between the two halves of the fluent question
	/// syntax. Modules extend it with the questions they support.
	/// </summary>
	public class QuestionBuilder
	{
		/// <summary>Network the question will be asked of.</summary>
		public ISemanticNetwork SemanticNetwork
		{ get; }

		/// <summary>Hypothetical statements to assume while answering; may be <c>null</c>.</summary>
		public IEnumerable<IStatement> Preconditions
		{ get; }

		/// <summary>Creates a builder.</summary>
		/// <param name="semanticNetwork">Network the question will be asked of.</param>
		/// <param name="preconditions">Hypothetical statements; <c>null</c> means none.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="semanticNetwork"/> is <c>null</c>.</exception>
		public QuestionBuilder(ISemanticNetwork semanticNetwork, IEnumerable<IStatement> preconditions)
		{
			SemanticNetwork = semanticNetwork.EnsureNotNull(nameof(semanticNetwork));
			Preconditions = preconditions;
		}

		/// <summary>Reads as English after <c>Supposing(...)</c>; returns the builder unchanged.</summary>
		/// <returns>This builder.</returns>
		public QuestionBuilder Ask()
		{
			return this;
		}
	}

	/// <summary>Entry points of the fluent question syntax.</summary>
	public static class SubjectQuestionExtensions
	{
		/// <summary>Begins a question against the network's own knowledge.</summary>
		/// <param name="semanticNetwork">Network to ask.</param>
		/// <returns>A builder the question is then called on.</returns>
		public static QuestionBuilder Ask(this ISemanticNetwork semanticNetwork)
		{
			return new QuestionBuilder(semanticNetwork, null);
		}

		/// <summary>Begins a hypothetical question, assuming extra statements that are never stored.</summary>
		/// <param name="semanticNetwork">Network to ask.</param>
		/// <param name="preconditions">Statements to assume true while answering.</param>
		/// <returns>A builder the question is then called on.</returns>
		public static QuestionBuilder Supposing(this ISemanticNetwork semanticNetwork, IEnumerable<IStatement> preconditions)
		{
			return new QuestionBuilder(semanticNetwork, preconditions);
		}
	}
}
