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
		public string ID
		{ get; set; }

		public abstract IStatement BoundStatement
		{ get; }

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
		public override IStatement BoundStatement
		{ get { return BoundObject;} }

		public StatementT BoundObject
		{ get; protected set; }

		protected readonly ILanguage _language;

		protected StatementViewModel(string id, ILanguage language)
		{
			ID = id;
			_language = language;
		}

		public override IStatement CreateStatement()
		{
			return BoundObject = CreateStatementImplementation();
		}

		protected abstract StatementT CreateStatementImplementation();

		public override string ToString()
		{
			var statement = BoundObject ?? CreateStatementImplementation();
			var text = statement.DescribeTrue();
			return ((Core.Text.TextBlock) text).GetPlainText(_language);
		}
	}
}
