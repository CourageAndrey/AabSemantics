using System.Windows;

using Inventor.Core;

namespace Inventor.Client
{
	public interface IKnowledgeViewModel
	{
		Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language);

		void ApplyCreate(IKnowledgeBase knowledgeBase);

		void ApplyUpdate();
	}

	public abstract class StatementViewModel : IKnowledgeViewModel
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
		where StatementT : class, IStatement
	{
		protected StatementT _boundObject;
		protected readonly ILanguage _language;

		protected StatementViewModel(ILanguage language)
		{
			_language = language;
		}

		public override IStatement CreateStatement()
		{
			return _boundObject = CreateStatementImplementation();
		}

		protected abstract StatementT CreateStatementImplementation();

		public override string ToString()
		{
			var statement = _boundObject ?? CreateStatementImplementation();
			var text = statement.DescribeTrue(_language);
			return text.GetPlainText(_language);
		}
	}
}
