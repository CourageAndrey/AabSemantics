using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
    public sealed class HasSignStatement : Statement<HasSignStatement>
    {
        #region Properties

        public override string Hint
        { get { return LanguageEx.CurrentEx.StatementHints.HasSign; } }

        public Concept Concept
        { get { return concept; } }

        public Concept Sign
        { get { return sign; } }

        private readonly Concept concept, sign;

        #endregion

        public HasSignStatement(Concept concept, Concept sign)
            : base(() => LanguageEx.CurrentEx.StatementNames.HasSign)
        {
            if (concept == null) throw new ArgumentNullException("concept");
            if (sign == null) throw new ArgumentNullException("sign");

            this.concept = concept;
            this.sign = sign;
        }

        public override IList<Concept> ChildConcepts
        { get { return new List<Concept> { Concept, Sign }.AsReadOnly(); } }

        #region Description

        protected override Func<string> GetDescriptionText(ILanguageStatementFormatStrings language)
        {
            return () => language.HasSign;
        }

        protected override IDictionary<string, INamed> GetDescriptionParameters()
        {
            return new Dictionary<string, INamed>
            {
                { "#CONCEPT#", concept },
                { "#SIGN#", sign },
            };
        }

        #endregion

        #region Consistency checking

        public override bool Equals(HasSignStatement other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other != null)
            {
                return other.concept == concept &&
                       other.sign == sign;
            }
            else return false;
        }

        public bool CheckSignDuplication(IEnumerable<HasSignStatement> hasSigns, IEnumerable<IsStatement> clasifications)
        {
            var signs = hasSigns.Where(hs => hs.Concept == concept).Select(hs => hs.Sign).ToList();
            foreach (var parent in IsStatement.GetParentsTree(clasifications, concept))
            {
                foreach (var parentSign in hasSigns.Where(hs => hs.Concept == parent).Select(hs => hs.Sign))
                {
                    if (signs.Contains(parentSign))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        public static List<HasSignStatement> GetSigns(IEnumerable<Statement> statements, Concept concept, bool recursive)
        {
            var result = new List<HasSignStatement>();
            var hasSigns = statements.OfType<HasSignStatement>().ToList();
            result.AddRange(hasSigns.Where(sv => sv.Concept == concept));
            if (recursive)
            {
                foreach (var parentSigns in IsStatement.GetParentsPlainList(statements, concept).Select(c => GetSigns(statements, c, true)))
                {
                    result.AddRange(parentSigns);
                }
            }
            return result;
        }
    }
}
