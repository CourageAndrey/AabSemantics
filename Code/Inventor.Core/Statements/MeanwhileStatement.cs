using System;

namespace Inventor.Core.Statements
{
	public class MeanwhileStatement : ProcessesStatement<MeanwhileStatement>
	{
		public MeanwhileStatement(IConcept processA, IConcept processB)
			: base(
				processA,
				processB,
				new Func<ILanguage, String>(language => language.StatementNames.Meanwhile),
				new Func<ILanguage, String>(language => language.StatementHints.Meanwhile))
		{ }

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.Meanwhile;
		}
	}
}
