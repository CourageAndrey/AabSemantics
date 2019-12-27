using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Propositions
{
    public sealed class Composition : Proposition<Composition>
    {
        #region Properties

        public override string Hint
        { get { return LanguageEx.CurrentEx.PropositionHints.Composition; } }

        public Concept Parent
        { get { return parent; } }

        public Concept Child
        { get { return child; } }

        private readonly Concept parent, child;

        #endregion

        public Composition(Concept parent, Concept child)
            : base(() => LanguageEx.CurrentEx.PropositionNames.Composition)
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
            return () => language.Composition;
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

        public override bool Equals(Composition other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other != null)
            {
                return other.parent == parent &&
                       other.child == child;
            }
            else return false;
        }

        #endregion

        #region Lookup

        public static List<Concept> GetContainingParents(IEnumerable<Proposition> relapropositions, Concept concept)
        {
            return relapropositions.OfType<Composition>().Where(c => c.Child == concept).Select(c => c.Parent).ToList();
        }

        public static List<Concept> GetContainingParts(IEnumerable<Proposition> propositions, Concept concept)
        {
            return propositions.OfType<Composition>().Where(c => c.Parent == concept).Select(c => c.Child).ToList();
        }

        #endregion
    }
}
