using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.WPF.Properties;
using Inventor.WPF.Converters;
using Inventor.Semantics;

namespace Inventor.WPF.TreeNodes
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
		{ get { return _application.SemanticNetwork; } }

		private static ImageSource _icon;
		private readonly IInventorApplication _application;

		#endregion

		public SemanticNetworkConceptsNode(IInventorApplication application)
		{
			_application = application;
			foreach (var concept in SemanticNetwork.Concepts)
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