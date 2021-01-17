using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.UI;
using Inventor.Core;
using Inventor.Core.Utils;

namespace Inventor.Client.TreeNodes
{
	public sealed class KnowledgeBaseStatementsNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _application.CurrentLanguage.Misc.NameCategoryStatements; } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.Misc.NameCategoryStatements; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		public IKnowledgeBase KnowledgeBase
		{ get { return _knowledgeBase; } }

		private static ImageSource _icon;
		private readonly IKnowledgeBase _knowledgeBase;
		private readonly InventorApplication _application;

		#endregion

		public KnowledgeBaseStatementsNode(IKnowledgeBase knowledgeBase, InventorApplication application)
		{
			_knowledgeBase = knowledgeBase;
			_application = application;
			foreach (var statement in knowledgeBase.Statements)
			{
				Children.Add(new StatementNode(statement, application));
			}
			knowledgeBase.StatementAdded += StatementAdded;
			knowledgeBase.StatementRemoved += StatementRemoved;
		}

		private void StatementAdded(object sender, ItemEventArgs<IStatement> args)
		{
			Children.Add(new StatementNode(args.Item, _application));
		}

		private void StatementRemoved(object sender, ItemEventArgs<IStatement> args)
		{
			Children.Remove(Children.OfType<StatementNode>().First(r => r.Statement == args.Item));
		}

		public void Clear()
		{
			_knowledgeBase.StatementAdded -= StatementAdded;
			_knowledgeBase.StatementRemoved -= StatementRemoved;
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