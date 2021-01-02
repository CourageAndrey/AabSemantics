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
		{ get { return _name; } }

		public LocalizedString Hint
		{ get { return _hint; } }

		public abstract IList<Concept> ChildConcepts
		{ get; }

		private readonly LocalizedStringConstant _name;
		private readonly LocalizedStringConstant _hint;

		#endregion

		public override sealed string ToString()
		{
			return string.Format("{0} \"{1}\"", Strings.TostringStatement, Name);
		}

		protected Statement(Func<ILanguageEx, string> name, Func<ILanguageEx, string> hint = null)
		{
			if (name != null)
			{
				_name = new LocalizedStringConstant(name);
			}
			else
			{
				throw new ArgumentNullException("name");
			}

			_hint = new LocalizedStringConstant(hint ?? (language => string.Empty));
		}

		#region Description

		public FormattedLine DescribeTrue(ILanguageEx language)
		{
			return new FormattedLine(GetDescriptionText(language.TrueStatementFormatStrings), GetDescriptionParameters());
		}

		public FormattedLine DescribeFalse(ILanguageEx language)
		{
			return new FormattedLine(GetDescriptionText(language.FalseStatementFormatStrings), GetDescriptionParameters());
		}

		public FormattedLine DescribeQuestion(ILanguageEx language)
		{
			return new FormattedLine(GetDescriptionText(language.QuestionStatementFormatStrings), GetDescriptionParameters());
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
		protected Statement(Func<ILanguageEx, string> name, Func<ILanguageEx, string> hint = null) : base(name, hint)
		{ }

		public override sealed bool CheckUnique(IEnumerable<Statement> statements)
		{
			return statements.OfType<StatementT>().Count(Equals) == 1;
		}

		public abstract bool Equals(StatementT other);

#pragma warning disable 659
		public override sealed bool Equals(object obj)
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
