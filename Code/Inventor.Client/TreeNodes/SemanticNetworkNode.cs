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

		private static ImageSource _icon;
		private readonly ISemanticNetwork _semanticNetwork;
		private readonly SemanticNetworkConceptsNode _concepts;
		private readonly SemanticNetworkStatementsNode _statements;
		private readonly InventorApplication _application;

		#endregion

		public SemanticNetworkNode(ISemanticNetwork semanticNetwork, InventorApplication application)
		{
			_semanticNetwork = semanticNetwork;
			_application = application;
			Children.Add(_concepts = new SemanticNetworkConceptsNode(semanticNetwork, application));
			Children.Add(_statements = new SemanticNetworkStatementsNode(semanticNetwork, application));
		}

		public List<ExtendedTreeNode> Find(object obj)
		{
			if (obj is IConcept)
			{
				return _concepts.Find(obj as IConcept, this);
			}
			else if (obj is IStatement)
			{
				return _statements.Find(obj as IStatement, this);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public void Clear()
		{
			_concepts.Clear();
			_statements.Clear();
		}
	}
}