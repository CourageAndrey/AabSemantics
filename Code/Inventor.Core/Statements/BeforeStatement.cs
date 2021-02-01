using System;

namespace Inventor.Core.Statements
{
	public class BeforeStatement : ProcessesStatement<BeforeStatement>
	{
		public BeforeStatement(IConcept processA, IConcept processB)
			: base(
				processA,
				processB,
				new Func<ILanguage, String>(language => language.StatementNames.Before),
				new Func<ILanguage, String>(language => language.StatementHints.Before))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.Before;
		}
	}
}
