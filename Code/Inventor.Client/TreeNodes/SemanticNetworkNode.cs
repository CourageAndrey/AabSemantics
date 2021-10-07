using System;
using System.Collections.Generic;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.Converters;
using Inventor.Core;

namespace Inventor.Client.TreeNodes
{
	public class SemanticNetworkNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _semanticNetwork.Name.GetValue(_application.CurrentLanguage); } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameSemanticNetwork; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.SemanticNetwork.ToSource()); } }
		
		public ISemanticNetwork SemanticNetwork
		{ get { return _semanticNetwork; } }

		public SemanticNetworkConceptsNode Concepts
		{ get; }

		public SemanticNetworkStatementsNode Statements
		{ get; }

		private static ImageSource _icon;
		private readonly ISemanticNetwork _semanticNetwork;
		private readonly IInventorApplication _application;

		#endregion

		public SemanticNetworkNode(ISemanticNetwork semanticNetwork, IInventorApplication application)
		{
			_semanticNetwork = semanticNetwork;
			_application = application;
			Children.Add(Concepts = new SemanticNetworkConceptsNode(semanticNetwork, application));
			Children.Add(Statements = new SemanticNetworkStatementsNode(semanticNetwork, application));
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