using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
    public sealed class SignValue : Statement<SignValue>
    {
        #region Properties

        public override string Hint
        { get { return LanguageEx.CurrentEx.StatementHints.SignValue; } }

        public Concept Concept
        { get { return concept; } }

        public Concept Sign
        { get { return sign; } }

        public Concept Value
        { get { return value; } }

        private readonly Concept concept, sign, value;

        #endregion

        public SignValue(Concept concept, Concept sign, Concept value)
            : base(() => LanguageEx.CurrentEx.StatementNames.SignValue)
        {
            if (concept == null) throw new ArgumentNullException("concept");
            if (sign == null) throw new ArgumentNullException("sign");
            if (value == null) throw new ArgumentNullException("value");

            this.concept = concept;
            this.sign = sign;
            this.value = value;
        }

        public override IList<Concept> ChildConcepts
        { get { return new List<Concept> { Concept, Sign, Value }.AsReadOnly(); } }

        #region Description

        protected override Func<string> GetDescriptionText(ILanguageStatementFormatStrings language)
        {
            return () => language.SignValue;
        }

        protected override IDictionary<string, INamed> GetDescriptionParameters()
        {
            return new Dictionary<string, INamed>
            {
                { "#CONCEPT#", concept },
                { "#SIGN#", sign },
                { "#VALUE#", value },
            };
        }

        #endregion

        #region Consistency checking

        public override bool Equals(SignValue other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other != null)
            {
                return other.concept == concept &&
                       other.sign == sign &&
                       other.value == value;
            }
            else return false;
        }

        public bool CheckHasSign(IEnumerable<Statement> statements)
        {
            return HasSign.GetSigns(statements, concept, true).Select(hs => hs.Sign).Contains(sign);
        }

        #endregion

        public static SignValue GetSignValue(IEnumerable<Statement> statements, Concept concept, Concept sign)
        {
            var signValues = statements.OfType<SignValue>().ToList();
            var signValue = signValues.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign);
            if (signValue != null)
            {
                return signValue;
            }
            else
            {
                foreach (var parent in Clasification.GetParentsPlainList(statements, concept))
                {
                    var parentValue = GetSignValue(statements, parent, sign);
                    if (parentValue != null)
                    {
                        return parentValue;
                    }
                }
                return null;
            }
        }

        public static List<SignValue> GetSignValues(IEnumerable<Statement> statements, Concept concept, bool recursive)
        {
            var result = new List<SignValue>();
            var signValues = statements.OfType<SignValue>().ToList();
            result.AddRange(signValues.Where(sv => sv.Concept == concept));
            if (recursive)
            {
                foreach (var parentSigns in Clasification.GetParentsPlainList(statements, concept).Select(c => GetSignValues(statements, c, true)))
                {
                    foreach (var signValue in parentSigns)
                    {
                        if (result.FirstOrDefault(sv => sv.Sign == signValue.Sign) == null)
                        {
                            result.AddRange(parentSigns);
                        }
                    }
                }
            }
            return result;
        }
    }
}
