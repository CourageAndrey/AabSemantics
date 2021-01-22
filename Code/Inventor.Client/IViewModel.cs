using System.Windows;

using Inventor.Core;

namespace Inventor.Client
{
	public interface IViewModel
	{
		Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language);

		void ApplyCreate(IKnowledgeBase knowledgeBase);

		void ApplyUpdate();
	}

	public abstract class StatementViewModel : IViewModel
	{
		public abstract Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language);

		public void ApplyCreate(IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(CreateStatement());
		}

		public abstract void ApplyUpdate();

		public abstract IStatement CreateStatement();
	}

	public abstract class StatementViewModel<StatementT> : StatementViewModel
		where StatementT : IStatement
	{
		protected StatementT _boundObject;

		public override IStatement CreateStatement()
		{
			return _boundObject = CreateStatementImplementation();
		}

		protected abstract StatementT CreateStatementImplementation();
	}
}
