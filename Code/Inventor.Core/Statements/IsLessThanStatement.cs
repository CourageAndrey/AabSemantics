using System;

namespace Inventor.Core.Statements
{
	public class IsLessThanStatement : ComparisonStatement<IsLessThanStatement>
	{
		public IsLessThanStatement(IConcept leftValue, IConcept rightValue)
			: base(
				leftValue,
				rightValue,
				new Func<ILanguage, String>(language => language.StatementNames.IsLessThan),
				new Func<ILanguage, String>(language => language.StatementHints.IsLessThan))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.IsLessThan;
		}
	}
}
