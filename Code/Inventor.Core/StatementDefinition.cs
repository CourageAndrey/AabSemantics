using System;

namespace Inventor.Core
{
	public class StatementDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		private readonly Func<ILanguage, String> _statementNameGetter;
		private readonly Func<IStatement, Xml.Statement> _statementXmlGetter;

		#endregion

		#region Constructors

		public StatementDefinition(Type type, Func<ILanguage, String> statementNameGetter, Func<IStatement, Xml.Statement> statementXmlGetter)
		{
			Type = type;
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
