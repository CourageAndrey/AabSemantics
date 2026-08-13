using System.Windows.Media;

using AabSemantics.Extensions.WPF.Converters;
using AabSemantics.Extensions.WPF.Properties;

namespace AabSemantics.Extensions.WPF.TreeNodes
{
	/// <summary>Tree node representing a single statement.</summary>
	public class StatementNode : ExtendedTreeNode
	{
		#region Properties

		/// <summary>Caption shown in the tree.</summary>
		public override string Text
		{ get { return TextRenders.PlainString.RenderText(_statement.DescribeTrue(), _application.CurrentLanguage).ToString(); } }

		/// <summary>Tooltip shown for the node.</summary>
		public override string Tooltip
		{ get { return _statement.Hint?.GetValue(_application.CurrentLanguage); } }

		/// <summary>Icon shown next to the caption.</summary>
		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Statement.ToSource()); } }

		/// <summary>The statement being edited.</summary>
		public IStatement Statement
		{ get { return _statement; } }

		private static ImageSource _icon;
		private readonly IStatement _statement;
		private readonly IInventorApplication _application;

		#endregion

		/// <summary>Creates the node.</summary>
		/// <param name="statement">Statement the node represents.</param>
		/// <param name="application">The hosting application.</param>
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
