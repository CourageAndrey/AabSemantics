using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Propositions
{
    public sealed class Clasification : Proposition<Clasification>
    {
        #region Properties

        public override string Hint
        { get { return LanguageEx.CurrentEx.PropositionHints.Clasification; } }

        public Concept Parent
        { get { return parent; } }

        public Concept Child
        { get { return child; } }

        private readonly Concept parent, child;

        #endregion

        public Clasification(Concept parent, Concept child)
            : base(() => LanguageEx.CurrentEx.PropositionNames.Clasification)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (child == null) throw new ArgumentNullException("child");

            this.parent = parent;
            this.child = child;
        }

        public override IList<Concept> ChildConcepts
        { get { return new List<Concept> { Parent, Child }.AsReadOnly(); } }

        #region Description

        protected override Func<string> GetDescriptionText(ILanguagePropositionFormatStrings language)
        {
            return () => language.Clasification;
        }

        protected override IDictionary<string, INamed> GetDescriptionParameters()
        {
            return new Dictionary<string, INamed>
            {
                { "#PARENT#", parent },
                { "#CHILD#", child },
            };
        }

        #endregion

        #region Consistency checking

        public override bool Equals(Clasification other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other != null)
            {
                return other.parent == parent &&
                       other.child == child;
            }
            else return false;
        }

        public bool CheckCyclic(IEnumerable<Clasification> propositions)
        {
            return !isCyclic(propositions, child, new List<Concept>());
        }

        private bool isCyclic(IEnumerable<Clasification> allClasifications, Concept concept, List<Concept> chain)
        {
            if (chain.Contains(concept)) return true;

            var clasifications = allClasifications.Where(c => c.Child == concept).ToList();
            if (clasifications.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (var clasification in clasifications)
                {
                    if (isCyclic(allClasifications, clasification.Parent, new List<Concept>(chain) { clasification.Child }))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion

        #region Lookup

        public static List<Concept> GetParentsTree(IEnumerable<Proposition> propositions, Concept concept)
        {
            return GetParentsTree(propositions.OfType<Clasification>(), concept);
        }

        public static List<Concept> GetParentsPlainList(IEnumerable<Proposition> propositions, Concept concept)
        {
            return GetParentsPlainList(propositions.OfType<Clasification>(), concept);
        }

        public static List<Concept> GetParentsTree(IEnumerable<Clasification> clasifications, Concept concept)
        {
            var result = new List<Concept>();
            foreach (var parent in GetParentsPlainList(clasifications, concept))
            {
                var list = new List<Concept> { parent };
                list.AddRange(GetParentsTree(clasifications, parent));
                list.RemoveAll(result.Contains);
                result.AddRange(list);
            }
            return result;
        }

        public static List<Concept> GetParentsPlainList(IEnumerable<Clasification> clasifications, Concept concept)
        {
            return clasifications.Where(c => c.Child == concept).Select(c => c.Parent).ToList();
        }

        #endregion
    }
}
