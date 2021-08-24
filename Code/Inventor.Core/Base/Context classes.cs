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
		{ get; protected set; }

		public ICollection<IStatement> Scope
		{ get; }

		public IContext Parent
		{ get; }

		public ICollection<IContext> ActiveContexts
		{ get { return _activeContexts ?? (_activeContexts = GetHierarchy()); } }

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

		public ICollection<IContext> GetHierarchy()
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

		public ISemanticNetworkContext Instantiate(ISemanticNetwork semanticNetwork, IStatementRepository statementRepository, IQuestionRepository questionRepository, IAttributeRepository attributeRepository)
		{
			if (Children.Count > 0) throw new InvalidOperationException("Impossible to instantiate system context more than once.");

			return new SemanticNetworkContext(Language, this, semanticNetwork, statementRepository, questionRepository, attributeRepository);
		}
	}

	public class SemanticNetworkContext : Context, ISemanticNetworkContext
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
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

		public SemanticNetworkContext(ILanguage language, IContext parent, ISemanticNetwork semanticNetwork, IStatementRepository statementRepository, IQuestionRepository questionRepository, IAttributeRepository attributeRepository)
			: base(language, parent)
		{
			SemanticNetwork = semanticNetwork;
			StatementRepository = statementRepository;
			QuestionRepository = questionRepository;
			AttributeRepository = attributeRepository;
		}

		public IQuestionProcessingContext CreateQuestionContext(IQuestion question, ILanguage language = null)
		{
			var concreteContextType = typeof(QuestionProcessingContext<>).MakeGenericType(question.GetType());
			var contextConstructor = concreteContextType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
			var resultContext = contextConstructor.Invoke(new Object[] { this, question, language }) as IQuestionProcessingContext;
			foreach (var statement in question.Preconditions)
			{
				statement.Context = resultContext;
				resultContext.SemanticNetwork.Statements.Add(statement);
			}
			return resultContext;
		}
	}

	public class DisposableProcessingContext : SemanticNetworkContext, IDisposable
	{
		internal DisposableProcessingContext(ISemanticNetworkContext parent)
			: base(parent.Language, parent, parent.SemanticNetwork, parent.StatementRepository, parent.QuestionRepository, parent.AttributeRepository)
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
					SemanticNetwork.Statements.Remove(knowledge);
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

		internal QuestionProcessingContext(ISemanticNetworkContext parent, QuestionT question, ILanguage language = null)
			: base(parent)
		{
			_question = question;
			if (language != null)
			{
				Language = language;
			}
		}
	}
}
