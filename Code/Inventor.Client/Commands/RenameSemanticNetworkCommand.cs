using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class RenameSemanticNetworkCommand : BaseEditCommand
	{
		#region Properties

		#endregion

		public RenameSemanticNetworkCommand(ISemanticNetwork semanticNetwork)
			: base(semanticNetwork)
		{
		}

		public override void Apply()
		{
			throw new System.NotImplementedException();
		}

		public override void Rollback()
		{
			throw new System.NotImplementedException();
		}
	}
}
