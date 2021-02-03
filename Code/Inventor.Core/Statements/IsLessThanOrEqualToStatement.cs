using System;

namespace Inventor.Core.Statements
{
	public class IsLessThanOrEqualToStatement : ComparisonStatement<IsLessThanOrEqualToStatement>
	{
		public IsLessThanOrEqualToStatement(IConcept leftValue, IConcept rightValue)
			: base(
				leftValue,
				rightValue,
				new Func<ILanguage, String>(language => language.StatementNames.IsLessThanOrEqualTo),
				new Func<ILanguage, String>(language => language.StatementHints.IsLessThanOrEqualTo))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.IsLessThanOrEqualTo;
		}
	}
}
