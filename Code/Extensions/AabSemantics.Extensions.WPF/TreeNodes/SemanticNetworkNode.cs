using System;
using System.Collections.Generic;
using System.Windows.Media;

using AabSemantics.Extensions.WPF.Converters;
using AabSemantics.Extensions.WPF.Properties;

namespace AabSemantics.Extensions.WPF.TreeNodes
{
	/// <summary>Tree node representing the knowledge base itself.</summary>
	public class SemanticNetworkNode : ExtendedTreeNode
	{
		#region Properties

		/// <summary>Caption shown in the tree.</summary>
		public override string Text
		{ get { return SemanticNetwork.Name.GetValue(_application.CurrentLanguage); } }

		/// <summary>Tooltip shown for the node.</summary>
		public override string Tooltip
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameSemanticNetwork; } }

		/// <summary>Icon shown next to the caption.</summary>
		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.SemanticNetwork.ToSource()); } }
		
		/// <summary>The knowledge base this node belongs to.</summary>
		public ISemanticNetwork SemanticNetwork
		{ get { return _application.SemanticNetwork; } }

		/// <summary>Concepts filling the kind's roles.</summary>
		public SemanticNetworkConceptsNode Concepts
		{ get; }

		/// <summary>Child node grouping the statements.</summary>
		public SemanticNetworkStatementsNode Statements
		{ get; }

		private static ImageSource _icon;
		private readonly IInventorApplication _application;

		#endregion

		/// <summary>Creates the root node over the application's current knowledge base.</summary>
		/// <param name="application">The hosting application.</param>
		public SemanticNetworkNode(IInventorApplication application)
		{
			_application = application;
			Children.Add(Concepts = new SemanticNetworkConceptsNode(application));
			Children.Add(Statements = new SemanticNetworkStatementsNode(application));
		}

		/// <summary>Finds the path of nodes leading to the item, root first.</summary>
		/// <param name="obj">Concept or statement to look for.</param>
		/// <returns>The path, or an empty list when the item is not in the tree.</returns>
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