using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core
{
    public abstract class Proposition : INamed
    {
        #region Properties

        public LocalizedString Name
        { get { return name; } }

        public abstract string Hint
        { get; }

        public abstract IList<Concept> ChildConcepts
        { get; }

        private readonly LocalizedStringConstant name;

        #endregion

        public sealed override string ToString()
        {
            return string.Format("{0} \"{1}\"", Strings.TostringProposition, Name);
        }

        protected Proposition(Func<string> name)
        {
            if (name != null)
            {
                this.name = new LocalizedStringConstant(name);
            }
            else
            {
                throw new ArgumentNullException("name");
            }
        }

        #region Description

        public FormattedLine DescribeTrue()
        {
            return new FormattedLine(GetDescriptionText(LanguageEx.CurrentEx.TruePropositionFormatStrings), GetDescriptionParameters());
        }

        public FormattedLine DescribeFalse()
        {
            return new FormattedLine(GetDescriptionText(LanguageEx.CurrentEx.FalsePropositionFormatStrings), GetDescriptionParameters());
        }

        public FormattedLine DescribeQuestion()
        {
            return new FormattedLine(GetDescriptionText(LanguageEx.CurrentEx.QuestionPropositionFormatStrings), GetDescriptionParameters());
        }

        protected abstract Func<string> GetDescriptionText(ILanguagePropositionFormatStrings language);

        protected abstract IDictionary<string, INamed> GetDescriptionParameters();

        #endregion

        public abstract bool CheckUnique(IEnumerable<Proposition> propositions);

#pragma warning disable 659
        public abstract override bool Equals(object obj);

        public override int GetHashCode()
        {
// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
#pragma warning restore 659
    }

    public abstract class Proposition<PropositionT> : Proposition, IEquatable<PropositionT>
        where PropositionT : Proposition<PropositionT>
    {
        protected Proposition(Func<string> name) : base(name)
        { }

        public override sealed bool CheckUnique(IEnumerable<Proposition> propositions)
        {
            return propositions.OfType<PropositionT>().Count(Equals) == 1;
        }

        public abstract bool Equals(PropositionT other);

#pragma warning disable 659
        public sealed override bool Equals(object obj)
        {
            return Equals(obj as PropositionT);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
#pragma warning restore 659
    }
}
