using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Propositions
{
    public sealed class SubjectArea : Proposition<SubjectArea>
    {
        #region Properties

        public override string Hint
        { get { return LanguageEx.CurrentEx.PropositionHints.SubjectArea; } }

        public Concept Area
        { get { return area; } }

        public Concept Concept
        { get { return concept; } }

        private readonly Concept area;
        private readonly Concept concept;

        #endregion

        public SubjectArea(Concept area, Concept concept)
            : base(() => LanguageEx.CurrentEx.PropositionNames.SubjectArea)
        {
            if (area == null) throw new ArgumentNullException("area");
            if (concept == null) throw new ArgumentNullException("concept");

            this.area = area;
            this.concept = concept;
        }

        public override IList<Concept> ChildConcepts
        { get { return new List<Concept> { area, concept }.AsReadOnly(); } }

        #region Description

        protected override Func<string> GetDescriptionText(ILanguagePropositionFormatStrings language)
        {
            return () => language.SubjectArea;
        }

        protected override IDictionary<string, INamed> GetDescriptionParameters()
        {
            return new Dictionary<string, INamed>
            {
                { "#AREA#", area },
                { "#CONCEPT#", concept },
            };
        }

        #endregion

        #region Consistency checking

        public override bool Equals(SubjectArea other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other != null)
            {
                return other.area == area &&
                       other.concept == concept;
            }
            else return false;
        }

        #endregion
    }
}
