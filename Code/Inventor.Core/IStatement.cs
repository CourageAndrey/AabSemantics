using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IStatement : IKnowledge
	{
		ILocalizedString Hint
		{ get; }

		IEnumerable<IConcept> GetChildConcepts();

		FormattedLine DescribeTrue(ILanguage language);

		FormattedLine DescribeFalse(ILanguage language);

		FormattedLine DescribeQuestion(ILanguage language);

		Boolean CheckUnique(IEnumerable<IStatement> statements);
	}
}
