using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core
{
    public abstract class Statement : INamed
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
            return string.Format("{0} \"{1}\"", Strings.TostringStatement, Name);
        }

        protected Statement(Func<string> name)
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
            return new FormattedLine(GetDescriptionText(LanguageEx.CurrentEx.TrueStatementFormatStrings), GetDescriptionParameters());
        }

        public FormattedLine DescribeFalse()
        {
            return new FormattedLine(GetDescriptionText(LanguageEx.CurrentEx.FalseStatementFormatStrings), GetDescriptionParameters());
        }

        public FormattedLine DescribeQuestion()
        {
            return new FormattedLine(GetDescriptionText(LanguageEx.CurrentEx.QuestionStatementFormatStrings), GetDescriptionParameters());
        }

        protected abstract Func<string> GetDescriptionText(ILanguageStatementFormatStrings language);

        protected abstract IDictionary<string, INamed> GetDescriptionParameters();

        #endregion

        public abstract bool CheckUnique(IEnumerable<Statement> statements);

#pragma warning disable 659
        public abstract override bool Equals(object obj);

        public override int GetHashCode()
        {
// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
#pragma warning restore 659
    }

    public abstract class Statement<StatementT> : Statement, IEquatable<StatementT>
        where StatementT : Statement<StatementT>
    {
        protected Statement(Func<string> name) : base(name)
        { }

        public override sealed bool CheckUnique(IEnumerable<Statement> statements)
        {
            return statements.OfType<StatementT>().Count(Equals) == 1;
        }

        public abstract bool Equals(StatementT other);

#pragma warning disable 659
        public sealed override bool Equals(object obj)
        {
            return Equals(obj as StatementT);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
#pragma warning restore 659
    }
}
