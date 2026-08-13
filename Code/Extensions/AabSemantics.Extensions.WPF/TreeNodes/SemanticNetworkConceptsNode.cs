using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using AabSemantics.Extensions.WPF.Converters;
using AabSemantics.Extensions.WPF.Properties;

namespace AabSemantics.Extensions.WPF.TreeNodes
{
	/// <summary>Tree node representing the concepts category.</summary>
	public class SemanticNetworkConceptsNode : ExtendedTreeNode
	{
		#region Properties

		/// <summary>Caption shown in the tree.</summary>
		public override string Text
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryConcepts; } }

		/// <summary>Tooltip shown for the node.</summary>
		public override string Tooltip
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryConcepts; } }

		/// <summary>Icon shown next to the caption.</summary>
		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		/// <summary>The knowledge base this node belongs to.</summary>
		public ISemanticNetwork SemanticNetwork
		{ get { return _application.SemanticNetwork; } }

		private static ImageSource _icon;
		private readonly IInventorApplication _application;

		#endregion

		/// <summary>Creates the concepts category node.</summary>
		/// <param name="application">The hosting application.</param>
		public SemanticNetworkConceptsNode(IInventorApplication application)
		{
			_application = application;
			foreach (var concept in SemanticNetwork.Concepts)
			{
				Children.Add(new ConceptNode(concept, application));
			}
		}

		/// <summary>Finds the path of nodes leading to a concept.</summary>
		/// <param name="concept">Concept to look for.</param>
		/// <param name="parent">Node the returned path starts from.</param>
		/// <returns>The path, or an empty list when the concept is not in the tree.</returns>
		public List<ExtendedTreeNode> Find(IConcept concept, ExtendedTreeNode parent)
		{
			var child = Children.OfType<ConceptNode>().FirstOrDefault(c => c.Concept == concept);
			return child != null
				? new List<ExtendedTreeNode> { parent, this, child }
				: new List<ExtendedTreeNode>();
		}
	}
}