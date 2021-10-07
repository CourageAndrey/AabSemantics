using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.Converters;
using Inventor.Core;

namespace Inventor.Client.TreeNodes
{
	public class SemanticNetworkConceptsNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryConcepts; } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryConcepts; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		public ISemanticNetwork SemanticNetwork
		{ get { return _semanticNetwork; } }

		private static ImageSource _icon;
		private readonly ISemanticNetwork _semanticNetwork;
		private readonly IInventorApplication _application;

		#endregion

		public SemanticNetworkConceptsNode(ISemanticNetwork semanticNetwork, IInventorApplication application)
		{
			_semanticNetwork = semanticNetwork;
			_application = application;
			foreach (var concept in semanticNetwork.Concepts)
			{
				Children.Add(new ConceptNode(concept, application));
			}
		}

		public List<ExtendedTreeNode> Find(IConcept concept, ExtendedTreeNode parent)
		{
			var child = Children.OfType<ConceptNode>().FirstOrDefault(c => c.Concept == concept);
			return child != null
				? new List<ExtendedTreeNode> { parent, this, child }
				: new List<ExtendedTreeNode>();
		}
	}
}