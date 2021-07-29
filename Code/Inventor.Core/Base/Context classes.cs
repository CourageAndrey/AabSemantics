using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

		public ICollection<IContext> ActiveContexts
		{ get { return _activeContexts ?? (_activeContexts = getHierarchy()); } }

		private ICollection<IContext> _activeContexts;

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

		public ICollection<IContext> getHierarchy()
		{
			IContext context = this;
			var hierarchy = new HashSet<IContext>();
			while (context != null)
			{
				hierarchy.Add(context);
				context = context.Parent;
			}
			return hierarchy;
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

		public IKnowledgeBaseContext Instantiate(IKnowledgeBase knowledgeBase, IStatementRepository statementRepository, IQuestionRepository questionRepository, IAttributeRepository attributeRepository)
		{
			if (Children.Count > 0) throw new InvalidOperationException("Impossible to instantiate system context more than once.");

			return new KnowledgeBaseContext(Language, this, knowledgeBase, statementRepository, questionRepository, attributeRepository);
		}
	}

	public class KnowledgeBaseContext : Context, IKnowledgeBaseContext
	{
		#region Properties

		public IKnowledgeBase KnowledgeBase
		{ get; }

		public IStatementRepository StatementRepository
		{ get; }

		public IQuestionRepository QuestionRepository
		{ get; }

		public IAttributeRepository AttributeRepository
		{ get; }

		public override Boolean IsSystem
		{ get { return false; } }

		#endregion

		public KnowledgeBaseContext(ILanguage language, IContext parent, IKnowledgeBase knowledgeBase, IStatementRepository statementRepository, IQuestionRepository questionRepository, IAttributeRepository attributeRepository)
			: base(language, parent)
		{
			KnowledgeBase = knowledgeBase;
			StatementRepository = statementRepository;
			QuestionRepository = questionRepository;
			AttributeRepository = attributeRepository;
		}

		public IQuestionProcessingContext CreateQuestionContext(IQuestion question)
		{
			var concreteContextType = typeof(QuestionProcessingContext<>).MakeGenericType(question.GetType());
			var contextConstructor = concreteContextType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
			var resultContext = contextConstructor.Invoke(new Object[] { this, question }) as IQuestionProcessingContext;
			foreach (var statement in question.Preconditions)
			{
				statement.Context = resultContext;
				resultContext.KnowledgeBase.Statements.Add(statement);
			}
			return resultContext;
		}
	}

	public class DisposableProcessingContext : KnowledgeBaseContext, IDisposable
	{
		internal DisposableProcessingContext(IKnowledgeBaseContext parent)
			: base(parent.Language, parent, parent.KnowledgeBase, parent.StatementRepository, parent.QuestionRepository, parent.AttributeRepository)
		{ }

		private Boolean _disposed;

		public void Dispose()
		{
			if (!_disposed)
			{
				foreach (var child in Children.OfType<DisposableProcessingContext>())
				{
					if (!child._disposed) throw new InvalidOperationException("Impossible to dispose question context because it has running child contexts.");
				}

				foreach (var knowledge in Scope)
				{
					KnowledgeBase.Statements.Remove(knowledge);
				}

				Parent.Children.Remove(this);

				_disposed = true;

				GC.SuppressFinalize(this);
			}
		}
	}

	public class QuestionProcessingContext<QuestionT> : DisposableProcessingContext, IQuestionProcessingContext<QuestionT>
		where QuestionT : IQuestion
	{
		#region Properties

		IQuestion IQuestionProcessingContext.Question
		{ get { return _question; } }

		public QuestionT Question
		{ get { return _question; } }

		private readonly QuestionT _question;

		#endregion

		internal QuestionProcessingContext(IKnowledgeBaseContext parent, QuestionT question)
			: base(parent)
		{
			_question = question;
		}
	}
}
