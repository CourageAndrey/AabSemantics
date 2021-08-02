using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class AddStatementCommand : BaseEditCommand
	{
		#region Properties

		public StatementViewModel ViewModel
		{ get; }

		public IStatement NewItem
		{ get; private set; }

		#endregion

		public AddStatementCommand(StatementViewModel viewModel, ISemanticNetwork semanticNetwork)
			: base(semanticNetwork)
		{
			ViewModel = viewModel;
		}

		public override void Apply()
		{
			if (NewItem == null)
			{
				NewItem = (IStatement) ViewModel.ApplyCreate(SemanticNetwork);
			}
			else
			{
				SemanticNetwork.Statements.Add(NewItem);
			}
		}

		public override void Rollback()
		{
			SemanticNetwork.Statements.Remove(NewItem);
		}
	}
}
