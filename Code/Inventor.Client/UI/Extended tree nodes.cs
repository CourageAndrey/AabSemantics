using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Core;
using Inventor.Core.Localization;

using Sef.UI;

namespace Inventor.Client.UI
{
	public sealed class KnowledgeBaseNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _knowledgeBase.Name.Value; } }

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
			Сhildren.Add(_concepts = new KnowledgeBaseConceptsNode(knowledgeBase));
			Сhildren.Add(_statements = new KnowledgeBaseStatementsNode(knowledgeBase));
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
				Сhildren.Add(new ConceptNode(concept));
			}
			knowledgeBase.ConceptAdded += conceptAdded;
			knowledgeBase.ConceptRemoved += conceptRemoved;
		}

		private void conceptAdded(IList<Concept> list, Concept item)
		{
			Сhildren.Add(new ConceptNode(item));
		}

		private void conceptRemoved(IList<Concept> list, Concept item)
		{
			Сhildren.Remove(Сhildren.OfType<ConceptNode>().First(c => c.Concept == item));
		}

		public void Clear()
		{
			_knowledgeBase.ConceptAdded -= conceptAdded;
			_knowledgeBase.ConceptRemoved -= conceptRemoved;
		}

		public List<ExtendedTreeNode> Find(Concept concept, ExtendedTreeNode parent)
		{
			var child = Сhildren.OfType<ConceptNode>().FirstOrDefault(c => c.Concept == concept);
			return child != null
				? new List<ExtendedTreeNode> {parent, this, child}
				: new List<ExtendedTreeNode>();
		}
	}

	public sealed class KnowledgeBaseStatementsNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return LanguageEx.CurrentEx.Misc.NameCategoryStatements; } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameCategoryStatements; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Folder.ToSource()); } }

		public KnowledgeBase KnowledgeBase
		{ get { return _knowledgeBase; } }

		private static ImageSource _icon;
		private readonly KnowledgeBase _knowledgeBase;

		#endregion

		public KnowledgeBaseStatementsNode(KnowledgeBase knowledgeBase)
		{
			_knowledgeBase = knowledgeBase;
			foreach (var statement in knowledgeBase.Statements)
			{
				Сhildren.Add(new StatementNode(statement));
			}
			knowledgeBase.StatementAdded += StatementAdded;
			knowledgeBase.StatementRemoved += StatementRemoved;
		}

		private void StatementAdded(IList<Statement> list, Statement item)
		{
			Сhildren.Add(new StatementNode(item));
		}

		private void StatementRemoved(IList<Statement> list, Statement item)
		{
			Сhildren.Remove(Сhildren.OfType<StatementNode>().First(r => r.Statement == item));
		}

		public void Clear()
		{
			_knowledgeBase.StatementAdded -= StatementAdded;
			_knowledgeBase.StatementRemoved -= StatementRemoved;
		}

		public List<ExtendedTreeNode> Find(Statement statement, ExtendedTreeNode parent)
		{
			var child = Сhildren.OfType<StatementNode>().FirstOrDefault(rn => rn.Statement == statement);
			return child != null
				? new List<ExtendedTreeNode> {parent, this, child}
				: new List<ExtendedTreeNode>();
		}
	}

	public sealed class ConceptNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _concept.Name.Value; } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameConcept; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Concept.ToSource()); } }

		public Concept Concept
		{ get { return _concept; } }

		private static ImageSource _icon;
		private readonly Concept _concept;

		#endregion

		public ConceptNode(Concept concept)
		{
			_concept = concept;
		}
	}

	public sealed class StatementNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _statement.DescribeTrue(LanguageEx.CurrentEx).GetPlainText(); } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameStatement; } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Statement.ToSource()); } }

		public Statement Statement
		{ get { return _statement; } }

		private static ImageSource _icon;
		private readonly Statement _statement;

		#endregion

		public StatementNode(Statement statement)
		{
			_statement = statement;
			/*foreach (var concept in _statement.ChildConcepts)
			{
				children.Add(new ConceptNode(concept));
			}*/
		}
	}
}
