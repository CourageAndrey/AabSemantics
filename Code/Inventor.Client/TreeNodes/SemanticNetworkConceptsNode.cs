using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.Converters;
using Inventor.Core;
using Inventor.Core.Utils;

namespace Inventor.Client.TreeNodes
{
	public class SemanticNetworkConceptsNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _application.CurrentLanguage.Misc.NameCategoryConcepts; } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.Misc.NameCategoryConcepts; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		public ISemanticNetwork SemanticNetwork
		{ get { return _semanticNetwork; } }

		private static ImageSource _icon;
		private readonly ISemanticNetwork _semanticNetwork;
		private readonly InventorApplication _application;

		#endregion

		public SemanticNetworkConceptsNode(ISemanticNetwork semanticNetwork, InventorApplication application)
		{
			_semanticNetwork = semanticNetwork;
			_application = application;
			foreach (var concept in semanticNetwork.Concepts)
			{
				Children.Add(new ConceptNode(concept, application));
			}
			semanticNetwork.ConceptAdded += conceptAdded;
			semanticNetwork.ConceptRemoved += conceptRemoved;
		}

		private void conceptAdded(object sender, ItemEventArgs<IConcept> args)
		{
			Children.Add(new ConceptNode(args.Item, _application));
		}

		private void conceptRemoved(object sender, ItemEventArgs<IConcept> args)
		{
			Children.Remove(Children.OfType<ConceptNode>().First(c => c.Concept == args.Item));
		}

		public void Clear()
		{
			_semanticNetwork.ConceptAdded -= conceptAdded;
			_semanticNetwork.ConceptRemoved -= conceptRemoved;
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