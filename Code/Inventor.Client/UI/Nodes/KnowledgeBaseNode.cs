using System;
using System.Collections.Generic;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Core;
using Inventor.Core.Localization;

using Sef.UI;

namespace Inventor.Client.UI.Nodes
{
	public sealed class KnowledgeBaseNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _knowledgeBase.Name.GetValue(LanguageEx.CurrentEx); } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameKnowledgeBase; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.KnowledgeBase.ToSource()); } }

		public KnowledgeBase KnowledgeBase
		{ get { return _knowledgeBase; } }

		private static ImageSource _icon;
		private readonly KnowledgeBase _knowledgeBase;
		private readonly KnowledgeBaseConceptsNode _concepts;
		private readonly KnowledgeBaseStatementsNode _statements;

		#endregion

		public KnowledgeBaseNode(KnowledgeBase knowledgeBase)
		{
			_knowledgeBase = knowledgeBase;
			Children.Add(_concepts = new KnowledgeBaseConceptsNode(knowledgeBase));
			Children.Add(_statements = new KnowledgeBaseStatementsNode(knowledgeBase));
		}

		public List<ExtendedTreeNode> Find(object obj)
		{
			if (obj is Concept)
			{
				return _concepts.Find(obj as Concept, this);
			}
			else if (obj is Statement)
			{
				return _statements.Find(obj as Statement, this);
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