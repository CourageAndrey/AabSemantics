using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.Converters;
using Inventor.Core;

namespace Inventor.Client.TreeNodes
{
	public class StatementNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return TextRepresenters.PlainString.RepresentText(_statement.DescribeTrue(), _application.CurrentLanguage).ToString(); } }

		public override string Tooltip
		{ get { return _statement.Hint?.GetValue(_application.CurrentLanguage); } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Statement.ToSource()); } }

		public IStatement Statement
		{ get { return _statement; } }

		private static ImageSource _icon;
		private readonly IStatement _statement;
		private readonly IInventorApplication _application;

		#endregion

		public StatementNode(IStatement statement, IInventorApplication application)
		{
			_statement = statement;
			_application = application;
			/*foreach (var concept in _statement.GetChildConcepts)
			{
				children.Add(new ConceptNode(concept));
			}*/
		}
	}
}
