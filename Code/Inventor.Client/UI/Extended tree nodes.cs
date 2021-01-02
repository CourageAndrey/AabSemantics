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
		{ get { return knowledgeBase.Name.Value; } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameKnowledgeBase; } }

		public override ImageSource Icon
		{ get { return icon ?? (icon = Resources.KnowledgeBase.ToSource()); } }

		public KnowledgeBase KnowledgeBase
		{ get { return knowledgeBase; } }

		private static ImageSource icon;
		private readonly KnowledgeBase knowledgeBase;
		private readonly KnowledgeBaseConceptsNode concepts;
		private readonly KnowledgeBaseStatementsNode _statements;

		#endregion

		public KnowledgeBaseNode(KnowledgeBase knowledgeBase)
		{
			this.knowledgeBase = knowledgeBase;
			Сhildren.Add(concepts = new KnowledgeBaseConceptsNode(knowledgeBase));
			Сhildren.Add(_statements = new KnowledgeBaseStatementsNode(knowledgeBase));
		}

		public List<ExtendedTreeNode> Find(object obj)
		{
			if (obj is Concept)
			{
				return concepts.Find(obj as Concept, this);
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
			concepts.Clear();
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
		{ get { return icon ?? (icon = Resources.Folder.ToSource()); } }

		public KnowledgeBase KnowledgeBase
		{ get { return knowledgeBase; } }

		private static ImageSource icon;
		private readonly KnowledgeBase knowledgeBase;

		#endregion

		public KnowledgeBaseConceptsNode(KnowledgeBase knowledgeBase)
		{
			this.knowledgeBase = knowledgeBase;
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
			knowledgeBase.ConceptAdded -= conceptAdded;
			knowledgeBase.ConceptRemoved -= conceptRemoved;
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
		{ get { return icon ?? (icon = Resources.Folder.ToSource()); } }

		public KnowledgeBase KnowledgeBase
		{ get { return knowledgeBase; } }

		private static ImageSource icon;
		private readonly KnowledgeBase knowledgeBase;

		#endregion

		public KnowledgeBaseStatementsNode(KnowledgeBase knowledgeBase)
		{
			this.knowledgeBase = knowledgeBase;
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
			knowledgeBase.StatementAdded -= StatementAdded;
			knowledgeBase.StatementRemoved -= StatementRemoved;
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
		{ get { return concept.Name.Value; } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameConcept; } }

		public override ImageSource Icon
		{ get { return icon ?? (icon = Resources.Concept.ToSource()); } }

		public Concept Concept
		{ get { return concept; } }

		private static ImageSource icon;
		private readonly Concept concept;

		#endregion

		public ConceptNode(Concept concept)
		{
			this.concept = concept;
		}
	}

	public sealed class StatementNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return statement.DescribeTrue().GetPlainText(); } }

		public override string Tooltip
		{ get { return LanguageEx.CurrentEx.Misc.NameStatement; } }

		public override ImageSource Icon
		{ get { return icon ?? (icon = Resources.Statement.ToSource()); } }

		public Statement Statement
		{ get { return statement; } }

		private static ImageSource icon;
		private readonly Statement statement;

		#endregion

		public StatementNode(Statement statement)
		{
			this.statement = statement;
			/*foreach (var concept in statement.ChildConcepts)
			{
				children.Add(new ConceptNode(concept));
			}*/
		}
	}
}
