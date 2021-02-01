using System;

namespace Inventor.Core.Statements
{
	public class IsGreaterThanStatement : ComparisonStatement<IsGreaterThanStatement>
	{
		public IsGreaterThanStatement(IConcept leftValue, IConcept rightValue)
			: base(
				leftValue,
				rightValue,
				new Func<ILanguage, String>(language => language.StatementNames.IsGreaterThan),
				new Func<ILanguage, String>(language => language.StatementHints.IsGreaterThan))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.IsGreaterThan;
		}
	}
}
