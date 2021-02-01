using System;

namespace Inventor.Core.Statements
{
	public class AfterStatement : ProcessesStatement<AfterStatement>
	{
		public AfterStatement(IConcept processA, IConcept processB)
			: base(
				processA,
				processB,
				new Func<ILanguage, String>(language => language.StatementNames.After),
				new Func<ILanguage, String>(language => language.StatementHints.After))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.After;
		}
	}
}
