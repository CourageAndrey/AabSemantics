using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class DeleteStatementCommand : BaseEditCommand
	{
		#region Properties

		public IStatement Statement
		{ get; }

		#endregion

		public DeleteStatementCommand(IStatement statement, ISemanticNetwork semanticNetwork)
			: base(semanticNetwork)
		{
			Statement = statement;
		}

		public override void Apply()
		{
			SemanticNetwork.Statements.Remove(Statement);
		}

		public override void Rollback()
		{
			SemanticNetwork.Statements.Add(Statement);
		}
	}
}
