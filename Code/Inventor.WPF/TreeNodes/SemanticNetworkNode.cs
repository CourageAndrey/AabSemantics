using System;
using System.Collections.Generic;
using System.Windows.Media;

using Inventor.WPF.Properties;
using Inventor.WPF.Converters;
using Inventor.Semantics;

namespace Inventor.WPF.TreeNodes
{
	public class SemanticNetworkNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return SemanticNetwork.Name.GetValue(_application.CurrentLanguage); } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameSemanticNetwork; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.SemanticNetwork.ToSource()); } }
		
		public ISemanticNetwork SemanticNetwork
		{ get { return _application.SemanticNetwork; } }

		public SemanticNetworkConceptsNode Concepts
		{ get; }

		public SemanticNetworkStatementsNode Statements
		{ get; }

		private static ImageSource _icon;
		private readonly IInventorApplication _application;

		#endregion

		public SemanticNetworkNode(IInventorApplication application)
		{
			_application = application;
			Children.Add(Concepts = new SemanticNetworkConceptsNode(application));
			Children.Add(Statements = new SemanticNetworkStatementsNode(application));
		}

		public List<ExtendedTreeNode> Find(object obj)
		{
			if (obj is IConcept)
			{
				return Concepts.Find(obj as IConcept, this);
			}
			else if (obj is IStatement)
			{
				return Statements.Find(obj as IStatement, this);
			}
			else
			{
				throw new NotSupportedException();
			}
		}
	}
}