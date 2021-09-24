using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.Converters;
using Inventor.Core;

namespace Inventor.Client.TreeNodes
{
	public class SemanticNetworkStatementsNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryStatements; } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryStatements; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		public ISemanticNetwork SemanticNetwork
		{ get { return _semanticNetwork; } }

		private static ImageSource _icon;
		private readonly ISemanticNetwork _semanticNetwork;
		private readonly InventorApplication _application;

		#endregion

		public SemanticNetworkStatementsNode(ISemanticNetwork semanticNetwork, InventorApplication application)
		{
			_semanticNetwork = semanticNetwork;
			_application = application;
			foreach (var statement in semanticNetwork.Statements)
			{
				Children.Add(new StatementNode(statement, application));
			}
		}

		public List<ExtendedTreeNode> Find(IStatement statement, ExtendedTreeNode parent)
		{
			var child = Children.OfType<StatementNode>().FirstOrDefault(rn => rn.Statement == statement);
			return child != null
				? new List<ExtendedTreeNode> { parent, this, child }
				: new List<ExtendedTreeNode>();
		}
	}
}