using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public delegate void StatementConsistencyCheckerDelegate(ISemanticNetwork semanticNetwork, ITextContainer result);
	public delegate void StatementConsistencyCheckerDelegate<StatementT>(ICollection<StatementT> statements, ITextContainer result, ISemanticNetwork semanticNetwork)
		where StatementT : IStatement;

	public class StatementDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		public String XmlElementName
		{ get; }

		public Type XmlType
		{ get; }

		private readonly Func<ILanguage, String> _statementNameGetter;
		private readonly Func<IStatement, Xml.Statement> _statementXmlGetter;
		private readonly StatementConsistencyCheckerDelegate _consistencyChecker;

		#endregion

		#region Constructors

		public StatementDefinition(
			Type type,
			Func<ILanguage, String> statementNameGetter,
			Func<IStatement, Xml.Statement> statementXmlGetter,
			Type xmlType,
			StatementConsistencyCheckerDelegate consistencyChecker)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (statementNameGetter == null) throw new ArgumentNullException(nameof(statementNameGetter));
			if (statementXmlGetter == null) throw new ArgumentNullException(nameof(statementXmlGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));
			if (consistencyChecker == null) throw new ArgumentNullException(nameof(consistencyChecker));

			Type = type;
			_statementNameGetter = statementNameGetter;
			_statementXmlGetter = statementXmlGetter;
			_consistencyChecker = consistencyChecker;
			XmlType = xmlType;
			XmlElementName = XmlType.Name.Replace("Statement", "");
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

		public void CheckConsistency(ISemanticNetwork semanticNetwork, ITextContainer result)
		{
			_consistencyChecker(semanticNetwork, result);
		}

		public static readonly StatementConsistencyCheckerDelegate NoConsistencyCheck = (statements, result) => { };
	}

	public class StatementDefinition<StatementT> : StatementDefinition
		where StatementT: IStatement
	{
		public StatementDefinition(
			Func<ILanguage, String> statementNameGetter,
			Func<StatementT, Xml.Statement> statementXmlGetter,
			Type xmlType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			: base(
				typeof(StatementT),
				statementNameGetter,
				statement => statementXmlGetter((StatementT) statement),
				xmlType,
				(semanticNetwork, result) => consistencyChecker(semanticNetwork.Statements.OfType<StatementT>().ToList(), result, semanticNetwork))
		{
			if (consistencyChecker == null) throw new ArgumentNullException(nameof(consistencyChecker));
		}

		public new static readonly StatementConsistencyCheckerDelegate<StatementT> NoConsistencyCheck = (statements, result, allStatements) => { };
	}
}
