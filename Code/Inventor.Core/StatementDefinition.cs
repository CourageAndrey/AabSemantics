using System;

namespace Inventor.Core
{
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

		#endregion

		#region Constructors

		public StatementDefinition(
			Type type,
			Func<ILanguage, String> statementNameGetter,
			Func<IStatement, Xml.Statement> statementXmlGetter,
			Type xmlType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (statementNameGetter == null) throw new ArgumentNullException(nameof(statementNameGetter));
			if (statementXmlGetter == null) throw new ArgumentNullException(nameof(statementXmlGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));

			Type = type;
			_statementNameGetter = statementNameGetter;
			_statementXmlGetter = statementXmlGetter;
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
	}
}
