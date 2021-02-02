using System;

namespace Inventor.Core
{
	public class StatementDefinition
	{
		#region Properties

		public Type StatementType
		{ get; }

		private readonly Func<ILanguage, String> _statementNameGetter;

		#endregion

		#region Constructors

		public StatementDefinition(Type statementType, Func<ILanguage, String> statementNameGetter)
		{
			StatementType = statementType;
			_statementNameGetter = statementNameGetter;
		}

		#endregion

		public String GetName(ILanguage language)
		{
			return _statementNameGetter(language);
		}
	}
}
