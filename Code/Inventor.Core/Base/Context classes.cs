using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Base
{
	public abstract class Context : IContext
	{
		#region Properties

		public ILanguage Language
		{ get; }

		public ICollection<IStatement> Scope
		{ get; }

		public IContext Parent
		{ get; }

		public ICollection<IContext> Children
		{ get; }

		public abstract Boolean IsSystem
		{ get; }

		#endregion

		protected Context(ILanguage language, IContext parent)
		{
			Language = language;
			Scope = new List<IStatement>();
			Children = new List<IContext>();

			Parent = parent;
			if (parent != null)
			{
				parent.Children.Add(this);
			}
		}
	}

	public class SystemContext : Context, ISystemContext
	{
		#region Properties

		public override Boolean IsSystem
		{ get { return true; } }

		#endregion

		internal SystemContext(ILanguage language)
			: base(language, null)
		{ }

		public IKnowledgeBaseContext Instantiate(IKnowledgeBase knowledgeBase, IQuestionRepository questionRepository)
		{
			if (Children.Count > 0) throw new InvalidOperationException("Impossible to instantiate system context more than once.");

			return new KnowledgeBaseContext(Language, this, knowledgeBase, questionRepository);
		}
	}

	public class KnowledgeBaseContext : Context, IKnowledgeBaseContext
	{
		#region Properties

		public IKnowledgeBase KnowledgeBase
		{ get; }

		public IQuestionRepository QuestionRepository
		{ get; }

		public override Boolean IsSystem
		{ get { return false; } }

		#endregion

		public KnowledgeBaseContext(ILanguage language, IContext parent, IKnowledgeBase knowledgeBase, IQuestionRepository questionRepository)
			: base(language, parent)
		{
			KnowledgeBase = knowledgeBase;
			QuestionRepository = questionRepository;
		}

		public IQuestionProcessingContext AskQuestion(IQuestion question)
		{
			return new QuestionProcessingContext(this, question);
		}
	}

	public class QuestionProcessingContext : KnowledgeBaseContext, IQuestionProcessingContext
	{
		#region Properties

		public IQuestion Question
		{ get; }

		#endregion

		internal QuestionProcessingContext(IKnowledgeBaseContext parent, IQuestion question)
			: base(parent.Language, parent, parent.KnowledgeBase, parent.QuestionRepository)
		{
			Question = question;
		}

		private Boolean _disposed;

		public void Dispose()
		{
			if (!_disposed)
			{
				foreach (var child in Children.OfType<QuestionProcessingContext>())
				{
					if (!child._disposed) throw new InvalidOperationException("Impossible to dispose question context because it has running child contexts.");
				}

				foreach (var knowledge in Scope)
				{
					KnowledgeBase.Statements.Remove(knowledge);
				}

				_disposed = true;

				GC.SuppressFinalize(this);
			}
		}
	}

	public class QuestionProcessingContext<QuestionT> : QuestionProcessingContext, IQuestionProcessingContext<QuestionT>
		where QuestionT : IQuestion
	{
		public QuestionT QuestionX
		{ get; }

		internal QuestionProcessingContext(IQuestionProcessingContext untyped)
			: base(untyped.Parent as IKnowledgeBaseContext, untyped.Question)
		{
			QuestionX = (QuestionT) untyped.Question;
			Parent.Children.Remove(untyped);
		}
	}
}
