using System.Windows;

using Inventor.Core;

namespace Inventor.Client
{
	public interface IKnowledgeViewModel
	{
		Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language);

		object ApplyCreate(ISemanticNetwork semanticNetwork);

		void ApplyUpdate();
	}

	public abstract class StatementViewModel : IKnowledgeViewModel
	{
		public abstract Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language);

		public abstract StatementViewModel Clone();

		public object ApplyCreate(ISemanticNetwork semanticNetwork)
		{
			var statement = CreateStatement();
			semanticNetwork.Statements.Add(statement);
			return statement;
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
