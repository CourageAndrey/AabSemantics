using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public class QuestionBuilder
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		public IEnumerable<IStatement> Preconditions
		{ get; }

		public QuestionBuilder(ISemanticNetwork semanticNetwork, IEnumerable<IStatement> preconditions)
		{
			if (semanticNetwork == null) throw new ArgumentNullException(nameof(semanticNetwork));

			SemanticNetwork = semanticNetwork;
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
