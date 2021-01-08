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
		{ get; }

		public LocalizedString Hint
		{ get; }

		#endregion

		public abstract IEnumerable<Concept> GetChildConcepts();

		public override sealed string ToString()
		{
			return string.Format("{0} \"{1}\"", Strings.TostringStatement, Name);
		}

		protected Statement(LocalizedString name, LocalizedString hint = null)
		{
			if (name != null)
			{
				Name = name;
			}
			else
			{
				throw new ArgumentNullException("name");
			}

			Hint = hint ?? LocalizedString.Empty;
		}

		#region Description

		public FormattedLine DescribeTrue(ILanguage language)
		{
			return new FormattedLine(GetDescriptionText(language.TrueStatementFormatStrings), GetDescriptionParameters());
		}

		public FormattedLine DescribeFalse(ILanguage language)
		{
			return new FormattedLine(GetDescriptionText(language.FalseStatementFormatStrings), GetDescriptionParameters());
		}

		public FormattedLine DescribeQuestion(ILanguage language)
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
		protected Statement(LocalizedString name, LocalizedString hint = null) : base(name, hint)
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
