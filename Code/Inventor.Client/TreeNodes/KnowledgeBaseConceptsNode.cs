using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.Converters;
using Inventor.Core;
using Inventor.Core.Utils;

namespace Inventor.Client.TreeNodes
{
	public class KnowledgeBaseConceptsNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _application.CurrentLanguage.Misc.NameCategoryConcepts; } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.Misc.NameCategoryConcepts; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		public IKnowledgeBase KnowledgeBase
		{ get { return _knowledgeBase; } }

		private static ImageSource _icon;
		private readonly IKnowledgeBase _knowledgeBase;
		private readonly InventorApplication _application;

		#endregion

		public KnowledgeBaseConceptsNode(IKnowledgeBase knowledgeBase, InventorApplication application)
		{
			_knowledgeBase = knowledgeBase;
			_application = application;
			foreach (var concept in knowledgeBase.Concepts)
			{
				Children.Add(new ConceptNode(concept, application));
			}
			knowledgeBase.ConceptAdded += conceptAdded;
			knowledgeBase.ConceptRemoved += conceptRemoved;
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
			_knowledgeBase.ConceptAdded -= conceptAdded;
			_knowledgeBase.ConceptRemoved -= conceptRemoved;
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