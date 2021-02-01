using System;

namespace Inventor.Core.Statements
{
	public class IsGreaterThanOrEqualToStatement : ComparisonStatement<IsGreaterThanOrEqualToStatement>
	{
		public IsGreaterThanOrEqualToStatement(IConcept leftValue, IConcept rightValue)
			: base(
				leftValue,
				rightValue,
				new Func<ILanguage, String>(language => language.StatementNames.IsGreaterThanOrEqualTo),
				new Func<ILanguage, String>(language => language.StatementHints.IsGreaterThanOrEqualTo))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.IsGreaterThanOrEqualTo;
		}
	}
}
