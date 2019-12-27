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
        private readonly KnowledgeBasePropositionsNode propositions;

        #endregion

        public KnowledgeBaseNode(KnowledgeBase knowledgeBase)
        {
            this.knowledgeBase = knowledgeBase;
            Сhildren.Add(concepts = new KnowledgeBaseConceptsNode(knowledgeBase));
            Сhildren.Add(propositions = new KnowledgeBasePropositionsNode(knowledgeBase));
        }

        public List<ExtendedTreeNode> Find(object obj)
        {
            if (obj is Concept)
            {
                return concepts.Find(obj as Concept, this);
            }
            else if (obj is Proposition)
            {
                return propositions.Find(obj as Proposition, this);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void Clear()
        {
            concepts.Clear();
            propositions.Clear();
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
                ? new List<ExtendedTreeNode> { parent, this, child }
                : new List<ExtendedTreeNode>();
        }
    }

    public sealed class KnowledgeBasePropositionsNode : ExtendedTreeNode
    {
        #region Properties

        public override string Text
        { get { return LanguageEx.CurrentEx.Misc.NameCategoryPropositions; } }

        public override string Tooltip
        { get { return LanguageEx.CurrentEx.Misc.NameCategoryPropositions; } }

        public override ImageSource Icon
        { get { return icon ?? (icon = Resources.Folder.ToSource()); } }

        public KnowledgeBase KnowledgeBase
        { get { return knowledgeBase; } }

        private static ImageSource icon;
        private readonly KnowledgeBase knowledgeBase;

        #endregion

        public KnowledgeBasePropositionsNode(KnowledgeBase knowledgeBase)
        {
            this.knowledgeBase = knowledgeBase;
            foreach (var proposition in knowledgeBase.Propositions)
            {
                Сhildren.Add(new PropositionNode(proposition));
            }
            knowledgeBase.PropositionAdded += propositionAdded;
            knowledgeBase.PropositionRemoved += propositionRemoved;
        }

        private void propositionAdded(IList<Proposition> list, Proposition item)
        {
            Сhildren.Add(new PropositionNode(item));
        }

        private void propositionRemoved(IList<Proposition> list, Proposition item)
        {
            Сhildren.Remove(Сhildren.OfType<PropositionNode>().First(r => r.Proposition == item));
        }

        public void Clear()
        {
            knowledgeBase.PropositionAdded -= propositionAdded;
            knowledgeBase.PropositionRemoved -= propositionRemoved;
        }

        public List<ExtendedTreeNode> Find(Proposition proposition, ExtendedTreeNode parent)
        {
            var child = Сhildren.OfType<PropositionNode>().FirstOrDefault(rn => rn.Proposition == proposition);
            return child != null
                ? new List<ExtendedTreeNode> { parent, this, child }
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

    public sealed class PropositionNode : ExtendedTreeNode
    {
        #region Properties

        public override string Text
        { get { return proposition.DescribeTrue().GetPlainText(); } }

        public override string Tooltip
        { get { return LanguageEx.CurrentEx.Misc.NameProposition; } }

        public override ImageSource Icon
        { get { return icon ?? (icon = Resources.Proposition.ToSource()); } }

        public Proposition Proposition
        { get { return proposition; } }

        private static ImageSource icon;
        private readonly Proposition proposition;

        #endregion

        public PropositionNode(Proposition proposition)
        {
            this.proposition = proposition;
            /*foreach (var concept in proposition.ChildConcepts)
            {
                children.Add(new ConceptNode(concept));
            }*/
        }
    }
}
