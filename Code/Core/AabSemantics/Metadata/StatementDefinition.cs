using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	public delegate void StatementConsistencyCheckerDelegate(ISemanticNetwork semanticNetwork, ITextContainer result);
	public delegate void StatementConsistencyCheckerDelegate<StatementT>(ISemanticNetwork semanticNetwork, ITextContainer result, ICollection<StatementT> statements)
		where StatementT : IStatement;

	public class StatementJsonSerializationSettings : IStatementSerializationSettings, IJsonSerializationSettings
	{
		public Type JsonType
		{ get; }

		private readonly Func<IStatement, Serialization.Json.Statement> _serializer;

		public StatementJsonSerializationSettings(Func<IStatement, Serialization.Json.Statement> serializer, Type jsonType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			JsonType = jsonType.EnsureNotNull(nameof(jsonType)).EnsureContract<Serialization.Json.Statement>(nameof(jsonType));
		}

		public Serialization.Json.Statement GetJson(IStatement statement)
		{
			return _serializer(statement);
		}
	}

	public class StatementXmlSerializationSettings : IStatementSerializationSettings, IXmlSerializationSettings
	{
		public String XmlElementName
		{ get; }

		public Type XmlType
		{ get; }

		private readonly Func<IStatement, Serialization.Xml.Statement> _serializer;

		public StatementXmlSerializationSettings(Func<IStatement, Serialization.Xml.Statement> serializer, Type xmlType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			XmlType = xmlType.EnsureNotNull(nameof(xmlType)).EnsureContract<Serialization.Xml.Statement>(nameof(xmlType));
			XmlElementName = XmlType.Name.Replace("Statement", "");
		}

		public Serialization.Xml.Statement GetXml(IStatement statement)
		{
			return _serializer(statement);
		}
	}

	public class StatementDefinition : MetadataDefinition<IStatementSerializationSettings>
	{
		#region Properties

		private readonly Func<ILanguage, String> _nameGetter;
		private readonly StatementConsistencyCheckerDelegate _consistencyChecker;

		#endregion

		#region Constructors

		public StatementDefinition(
			Type type,
			Func<ILanguage, String> nameGetter,
			StatementConsistencyCheckerDelegate consistencyChecker)
			: base(type, typeof(IStatement))
		{
			_nameGetter = nameGetter.EnsureNotNull(nameof(nameGetter));
			_consistencyChecker = consistencyChecker.EnsureNotNull(nameof(consistencyChecker));
		}

		#endregion

		public String GetName(ILanguage language)
		{
			return _nameGetter(language);
		}

		public void CheckConsistency(ISemanticNetwork semanticNetwork, ITextContainer result)
		{
			_consistencyChecker(semanticNetwork, result);
		}

		public static readonly StatementConsistencyCheckerDelegate NoConsistencyCheck = (semanticNetwork, result) => { };
	}

	public class StatementDefinition<StatementT> : StatementDefinition
		where StatementT : IStatement
	{
		public StatementDefinition(
			Func<ILanguage, String> nameGetter,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			: base(
				typeof(StatementT),
				nameGetter,
				(semanticNetwork, result) => consistencyChecker(semanticNetwork, result, semanticNetwork.Statements.OfType<StatementT>().ToList()))
		{
			consistencyChecker.EnsureNotNull(nameof(consistencyChecker));
		}

		public static readonly StatementConsistencyCheckerDelegate<StatementT> NoConsistencyCheck = (semanticNetwork, result, statements) => { };
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

		public static StatementDefinition SerializeToXml(
			this StatementDefinition metadataDefinition,
			Func<IStatement, Serialization.Xml.Statement> serializer,
			Type xmlType)
		{
			metadataDefinition.SerializationSettings.Add(new StatementXmlSerializationSettings(
				serializer,
				xmlType));
			return metadataDefinition;
		}

		public static StatementDefinition SerializeToJson(
			this StatementDefinition metadataDefinition,
			Func<IStatement, Serialization.Json.Statement> serializer,
			Type jsonType)
		{
			metadataDefinition.SerializationSettings.Add(new StatementJsonSerializationSettings(
				serializer,
				jsonType));
			return metadataDefinition;
		}

		public static StatementDefinition<StatementT> SerializeToXml<StatementT>(
			this StatementDefinition<StatementT> metadataDefinition,
			Func<StatementT, Serialization.Xml.Statement> serializer,
			Type xmlType)
			where StatementT : IStatement
		{
			metadataDefinition.SerializationSettings.Add(new StatementXmlSerializationSettings(
				statement => serializer((StatementT) statement),
				xmlType));
			return metadataDefinition;
		}

		public static StatementDefinition<StatementT> SerializeToJson<StatementT>(
			this StatementDefinition<StatementT> metadataDefinition,
			Func<StatementT, Serialization.Json.Statement> serializer,
			Type jsonType)
			where StatementT : IStatement
		{
			metadataDefinition.SerializationSettings.Add(new StatementJsonSerializationSettings(
				statement => serializer((StatementT) statement),
				jsonType));
			return metadataDefinition;
		}

		public static StatementDefinition<StatementT> SerializeToXml<StatementT, XmlT>(
			this StatementDefinition<StatementT> metadataDefinition,
			Func<StatementT, XmlT> serializer)
			where StatementT : IStatement
			where XmlT : Serialization.Xml.Statement
		{
			return metadataDefinition.SerializeToXml(
				serializer,
				typeof(XmlT));
		}

		public static StatementDefinition<StatementT> SerializeToJson<StatementT, JsonT>(
			this StatementDefinition<StatementT> metadataDefinition,
			Func<StatementT, JsonT> serializer)
			where StatementT : IStatement
			where JsonT : Serialization.Json.Statement
		{
			return metadataDefinition.SerializeToJson(
				serializer,
				typeof(JsonT));
		}
	}
}
