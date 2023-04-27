using System.Collections.Generic;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Questions
{
	public class QuestionBuilder
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		public IEnumerable<IStatement> Preconditions
		{ get; }

		public QuestionBuilder(ISemanticNetwork semanticNetwork, IEnumerable<IStatement> preconditions)
		{
			SemanticNetwork = semanticNetwork.EnsureNotNull(nameof(semanticNetwork));
			Preconditions = preconditions;
		}

		public QuestionBuilder Ask()
		{
			return this;
		}
	}

	public static class SubjectQuestionExtensions
	{
		public static QuestionBuilder Ask(this ISemanticNetwork semanticNetwork)
		{
			return new QuestionBuilder(semanticNetwork, null);
		}

		public static QuestionBuilder Supposing(this ISemanticNetwork semanticNetwork, IEnumerable<IStatement> preconditions)
		{
			return new QuestionBuilder(semanticNetwork, preconditions);
		}
	}
}
