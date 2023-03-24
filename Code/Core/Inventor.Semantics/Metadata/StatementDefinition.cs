﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Semantics.Metadata
{
	public delegate void StatementConsistencyCheckerDelegate(ISemanticNetwork semanticNetwork, ITextContainer result);
	public delegate void StatementConsistencyCheckerDelegate<StatementT>(ICollection<StatementT> statements, ITextContainer result, ISemanticNetwork semanticNetwork)
		where StatementT : IStatement;

	public class StatementJsonSerializationSettings : IStatementSerializationSettings, IJsonSerializationSettings
	{
		public Type JsonType
		{ get; }

		private readonly Func<IStatement, Serialization.Json.Statement> _statementJsonGetter;

		public StatementJsonSerializationSettings(Func<IStatement, Serialization.Json.Statement> statementJsonGetter, Type jsonType)
		{
			if (statementJsonGetter == null) throw new ArgumentNullException(nameof(statementJsonGetter));
			if (jsonType == null) throw new ArgumentNullException(nameof(jsonType));
			if (jsonType.IsAbstract || !typeof(Serialization.Json.Statement).IsAssignableFrom(jsonType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Json.Statement)}.", nameof(jsonType));

			_statementJsonGetter = statementJsonGetter;
			JsonType = jsonType;
		}

		public Serialization.Json.Statement GetJson(IStatement statement)
		{
			return _statementJsonGetter(statement);
		}
	}

	public class StatementXmlSerializationSettings : IStatementSerializationSettings, IXmlSerializationSettings
	{
		public String XmlElementName
		{ get; }

		public Type XmlType
		{ get; }

		private readonly Func<IStatement, Serialization.Xml.Statement> _statementXmlGetter;

		public StatementXmlSerializationSettings(Func<IStatement, Serialization.Xml.Statement> statementXmlGetter, Type xmlType)
		{
			if (statementXmlGetter == null) throw new ArgumentNullException(nameof(statementXmlGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));
			if (xmlType.IsAbstract || !typeof(Serialization.Xml.Statement).IsAssignableFrom(xmlType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Xml.Statement)}.", nameof(xmlType));

			_statementXmlGetter = statementXmlGetter;
			XmlType = xmlType;
			XmlElementName = XmlType.Name.Replace("Statement", "");
		}

		public Serialization.Xml.Statement GetXml(IStatement statement)
		{
			return _statementXmlGetter(statement);
		}
	}

	public class StatementDefinition : MetadataDefinition<IStatementSerializationSettings>
	{
		#region Properties

		private readonly Func<ILanguage, String> _statementNameGetter;
		private readonly StatementConsistencyCheckerDelegate _consistencyChecker;

		#endregion

		#region Constructors

		public StatementDefinition(
			Type type,
			Func<ILanguage, String> statementNameGetter,
			Func<IStatement, Serialization.Xml.Statement> statementXmlGetter,
			Func<IStatement, Serialization.Json.Statement> statementJsonGetter,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate consistencyChecker)
			: base(type, typeof(IStatement))
		{
			if (statementNameGetter == null) throw new ArgumentNullException(nameof(statementNameGetter));
			if (consistencyChecker == null) throw new ArgumentNullException(nameof(consistencyChecker));

			_statementNameGetter = statementNameGetter;
			_consistencyChecker = consistencyChecker;

			SerializationSettings.Add(new StatementJsonSerializationSettings(statementJsonGetter, jsonType));
			SerializationSettings.Add(new StatementXmlSerializationSettings(statementXmlGetter, xmlType));
		}

		#endregion

		public String GetName(ILanguage language)
		{
			return _statementNameGetter(language);
		}

		public void CheckConsistency(ISemanticNetwork semanticNetwork, ITextContainer result)
		{
			_consistencyChecker(semanticNetwork, result);
		}

		public static readonly StatementConsistencyCheckerDelegate NoConsistencyCheck = (statements, result) => { };
	}

	public static class StatementDefinitionExtensions
	{
		public static IXmlSerializationSettings GetXmlSerializationSettings(this StatementDefinition metadataDefinition)
		{
			return metadataDefinition.GetXmlSerializationSettings<StatementXmlSerializationSettings>();
		}

		public static SettingsT GetXmlSerializationSettings<SettingsT>(this StatementDefinition metadataDefinition)
			where SettingsT : IXmlSerializationSettings, IStatementSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}

		public static IJsonSerializationSettings GetJsonSerializationSettings(this StatementDefinition metadataDefinition)
		{
			return metadataDefinition.GetJsonSerializationSettings<StatementJsonSerializationSettings>();
		}

		public static SettingsT GetJsonSerializationSettings<SettingsT>(this StatementDefinition metadataDefinition)
			where SettingsT : IJsonSerializationSettings, IStatementSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}
	}

	public class StatementDefinition<StatementT> : StatementDefinition
		where StatementT: IStatement
	{
		public StatementDefinition(
			Func<ILanguage, String> statementNameGetter,
			Func<StatementT, Serialization.Xml.Statement> statementXmlGetter,
			Func<StatementT, Serialization.Json.Statement> statementJsonGetter,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			: base(
				typeof(StatementT),
				statementNameGetter,
				statement => statementXmlGetter((StatementT) statement),
				statement => statementJsonGetter((StatementT) statement),
				xmlType,
				jsonType,
				(semanticNetwork, result) => consistencyChecker(semanticNetwork.Statements.OfType<StatementT>().ToList(), result, semanticNetwork))
		{
			if (statementXmlGetter == null) throw new ArgumentNullException(nameof(statementXmlGetter));
			if (statementJsonGetter == null) throw new ArgumentNullException(nameof(statementJsonGetter));
			if (consistencyChecker == null) throw new ArgumentNullException(nameof(consistencyChecker));
		}

		public new static readonly StatementConsistencyCheckerDelegate<StatementT> NoConsistencyCheck = (statements, result, allStatements) => { };
	}

	public class StatementDefinition<StatementT, XmlT, JsonT> : StatementDefinition<StatementT>
		where StatementT: IStatement
		where XmlT : Serialization.Xml.Statement
		where JsonT : Serialization.Json.Statement
	{
		public StatementDefinition(
			Func<ILanguage, String> statementNameGetter,
			Func<StatementT, XmlT> statementXmlGetter,
			Func<StatementT, JsonT> statementJsonGetter,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			: base(
				statementNameGetter,
				statementXmlGetter,
				statementJsonGetter,
				typeof(XmlT),
				typeof(JsonT),
				consistencyChecker)
		{
			if (statementXmlGetter == null) throw new ArgumentNullException(nameof(statementXmlGetter));
			if (statementJsonGetter == null) throw new ArgumentNullException(nameof(statementJsonGetter));
			if (consistencyChecker == null) throw new ArgumentNullException(nameof(consistencyChecker));
		}

		public new static readonly StatementConsistencyCheckerDelegate<StatementT> NoConsistencyCheck = (statements, result, allStatements) => { };
	}
}
