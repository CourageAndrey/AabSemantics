using System;

namespace Inventor.Core.Statements
{
	public class IsNotEqualToStatement : ComparisonStatement<IsNotEqualToStatement>
	{
		public IsNotEqualToStatement(IConcept leftValue, IConcept rightValue)
			: base(
				leftValue,
				rightValue,
				new Func<ILanguage, String>(language => language.StatementNames.IsNotEqualTo),
				new Func<ILanguage, String>(language => language.StatementHints.IsNotEqualTo))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.IsNotEqualTo;
		}
	}
}
