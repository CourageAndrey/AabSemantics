using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Core;
using Inventor.Core.Localization;

using Sef.UI;

namespace Inventor.Client.UI.Nodes
{
	public sealed class StatementNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _statement.DescribeTrue(LanguageEx.CurrentEx).GetPlainText(); } }

		public override string Tooltip
		{ get { return _statement.Hint?.Value; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Statement.ToSource()); } }

		public Statement Statement
		{ get { return _statement; } }

		private static ImageSource _icon;
		private readonly Statement _statement;

		#endregion

		public StatementNode(Statement statement)
		{
			_statement = statement;
			/*foreach (var concept in _statement.ChildConcepts)
			{
				children.Add(new ConceptNode(concept));
			}*/
		}
	}
}
