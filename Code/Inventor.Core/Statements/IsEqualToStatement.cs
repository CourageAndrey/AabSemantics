using System;

namespace Inventor.Core.Statements
{
	public class IsEqualToStatement : ComparisonStatement<IsEqualToStatement>
	{
		public IsEqualToStatement(IConcept leftValue, IConcept rightValue)
			: base(
				leftValue,
				rightValue,
				new Func<ILanguage, String>(language => language.StatementNames.IsEqualTo),
				new Func<ILanguage, String>(language => language.StatementHints.IsEqualTo))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.IsEqualTo;
		}
	}
}
