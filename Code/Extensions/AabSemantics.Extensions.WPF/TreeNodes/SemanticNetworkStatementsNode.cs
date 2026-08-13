using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using AabSemantics.Extensions.WPF.Converters;
using AabSemantics.Extensions.WPF.Properties;

namespace AabSemantics.Extensions.WPF.TreeNodes
{
	/// <summary>Tree node representing the statements category.</summary>
	public class SemanticNetworkStatementsNode : ExtendedTreeNode
	{
		#region Properties

		/// <summary>Caption shown in the tree.</summary>
		public override string Text
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryStatements; } }

		/// <summary>Tooltip shown for the node.</summary>
		public override string Tooltip
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryStatements; } }

		/// <summary>Icon shown next to the caption.</summary>
		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		/// <summary>The knowledge base this node belongs to.</summary>
		public ISemanticNetwork SemanticNetwork
		{ get { return _application.SemanticNetwork; } }

		private static ImageSource _icon;
		private readonly IInventorApplication _application;

		#endregion

		/// <summary>Creates the statements category node.</summary>
		/// <param name="application">The hosting application.</param>
		public SemanticNetworkStatementsNode(IInventorApplication application)
		{
			_application = application;
			foreach (var statement in SemanticNetwork.Statements)
			{
				Children.Add(new StatementNode(statement, application));
			}
		}

		/// <summary>Finds the path of nodes leading to a statement.</summary>
		/// <param name="statement">Statement to look for.</param>
		/// <param name="parent">Node the returned path starts from.</param>
		/// <returns>The path, or an empty list when the statement is not in the tree.</returns>
		public List<ExtendedTreeNode> Find(IStatement statement, ExtendedTreeNode parent)
		{
			var child = Children.OfType<StatementNode>().FirstOrDefault(rn => rn.Statement == statement);
			return child != null
				? new List<ExtendedTreeNode> { parent, this, child }
				: new List<ExtendedTreeNode>();
		}
	}
}