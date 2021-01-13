using System;
using System.Collections.Generic;

using Inventor.Core.Utils;

namespace Inventor.Core
{
	public interface IKnowledgeBase : IKnowledge, IChangeable
	{
		ICollection<IConcept> Concepts
		{ get; }

		ICollection<IStatement> Statements
		{ get; }

		IQuestionRepository QuestionRepository
		{ get; }

		IQuestionProcessingContext AskQuestion(IQuestion question);

		IEnumerable<IKnowledge> EnumerateKnowledge(Func<IContext, Boolean> contextFilter);
		void Add(IKnowledge knowledge);
		Boolean Remove(IKnowledge knowledge);

		event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;
		event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;
		event EventHandler<ItemEventArgs<IStatement>> StatementAdded;
		event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;

		IConcept True
		{ get; }

		IConcept False
		{ get; }

		void Save(String fileName);

		FormattedText DescribeRules(ILanguage language);

		FormattedText CheckConsistensy(ILanguage language);
	}

	public static class KnowledgeBaseHelper
	{
		public static IEnumerable<IKnowledge> EnumerateKnowledge(this IKnowledgeBase knowledgeBase)
		{
			return knowledgeBase.EnumerateKnowledge(context => true);
		}

		public static IEnumerable<IKnowledge> EnumerateKnowledge(this IKnowledgeBase knowledgeBase, IContext certainContext)
		{
			return knowledgeBase.EnumerateKnowledge(context => context == certainContext);
		}

		public static IEnumerable<IKnowledge> EnumerateKnowledge(this IKnowledgeBase knowledgeBase, ICollection<IContext> validContexts)
		{
			return knowledgeBase.EnumerateKnowledge(context => validContexts.Contains(context));
		}
	}
}
