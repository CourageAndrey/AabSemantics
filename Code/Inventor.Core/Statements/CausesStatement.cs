using System;

namespace Inventor.Core.Statements
{
	public class CausesStatement : ProcessesStatement<CausesStatement>
	{
		public CausesStatement(IConcept processA, IConcept processB)
			: base(
				processA,
				processB,
				new Func<ILanguage, String>(language => language.StatementNames.Causes),
				new Func<ILanguage, String>(language => language.StatementHints.Causes))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.Causes;
		}
	}
}
