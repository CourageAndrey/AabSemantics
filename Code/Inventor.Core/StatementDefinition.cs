using System;

namespace Inventor.Core
{
	public class StatementDefinition
	{
		#region Properties

		public Type StatementType
		{ get; }

		private readonly Func<ILanguage, String> _statementNameGetter;
		private readonly Func<IStatement, Xml.Statement> _statementXmlGetter;

		#endregion

		#region Constructors

		public StatementDefinition(Type statementType, Func<ILanguage, String> statementNameGetter, Func<IStatement, Xml.Statement> statementXmlGetter)
		{
			StatementType = statementType;
			_statementNameGetter = statementNameGetter;
			_statementXmlGetter = statementXmlGetter;
		}

		#endregion

		public String GetName(ILanguage language)
		{
			return _statementNameGetter(language);
		}

		public Xml.Statement GetXml(IStatement statement)
		{
			return _statementXmlGetter(statement);
		}
	}
}
