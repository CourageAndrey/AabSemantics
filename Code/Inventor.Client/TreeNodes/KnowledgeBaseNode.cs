using System;
using System.Collections.Generic;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Client.Converters;
using Inventor.Core;

namespace Inventor.Client.TreeNodes
{
	public class KnowledgeBaseNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _knowledgeBase.Name.GetValue(_application.CurrentLanguage); } }

		public override string Tooltip
		{ get { return _application.CurrentLanguage.Misc.NameKnowledgeBase; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.KnowledgeBase.ToSource()); } }
		
		public IKnowledgeBase KnowledgeBase
		{ get { return _knowledgeBase; } }

		private static ImageSource _icon;
		private readonly IKnowledgeBase _knowledgeBase;
		private readonly KnowledgeBaseConceptsNode _concepts;
		private readonly KnowledgeBaseStatementsNode _statements;
		private readonly InventorApplication _application;

		#endregion

		public KnowledgeBaseNode(IKnowledgeBase knowledgeBase, InventorApplication application)
		{
			_knowledgeBase = knowledgeBase;
			_application = application;
			Children.Add(_concepts = new KnowledgeBaseConceptsNode(knowledgeBase, application));
			Children.Add(_statements = new KnowledgeBaseStatementsNode(knowledgeBase, application));
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