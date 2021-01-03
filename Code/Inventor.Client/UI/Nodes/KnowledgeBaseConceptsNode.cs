using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Core;
using Inventor.Core.Localization;

namespace Inventor.Client.UI.Nodes
{
	public sealed class KnowledgeBaseConceptsNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return LanguageEx.CurrentEx.Misc.NameCategoryConcepts; } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameCategoryConcepts; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		public KnowledgeBase KnowledgeBase
		{ get { return _knowledgeBase; } }

		private static ImageSource _icon;
		private readonly KnowledgeBase _knowledgeBase;

		#endregion

		public KnowledgeBaseConceptsNode(KnowledgeBase knowledgeBase)
		{
			_knowledgeBase = knowledgeBase;
			foreach (var concept in knowledgeBase.Concepts)
			{
				Children.Add(new ConceptNode(concept));
			}
			knowledgeBase.ConceptAdded += conceptAdded;
			knowledgeBase.ConceptRemoved += conceptRemoved;
		}

		private void conceptAdded(IList<Concept> list, Concept item)
		{
			Children.Add(new ConceptNode(item));
		}

		private void conceptRemoved(IList<Concept> list, Concept item)
		{
			Children.Remove(Children.OfType<ConceptNode>().First(c => c.Concept == item));
		}

		public void Clear()
		{
			_knowledgeBase.ConceptAdded -= conceptAdded;
			_knowledgeBase.ConceptRemoved -= conceptRemoved;
		}

		public List<ExtendedTreeNode> Find(Concept concept, ExtendedTreeNode parent)
		{
			var child = Children.OfType<ConceptNode>().FirstOrDefault(c => c.Concept == concept);
			return child != null
				? new List<ExtendedTreeNode> { parent, this, child }
				: new List<ExtendedTreeNode>();
		}
	}
}