using System;

namespace AabSemantics.Metadata
{
	public interface ISerializationSettings
	{ }

	public interface IXmlSerializationSettings : ISerializationSettings
	{
		Type XmlType
		{ get; }
	}

	public interface IJsonSerializationSettings : ISerializationSettings
	{
		Type JsonType
		{ get; }
	}

	public interface IAttributeSerializationSettings : ISerializationSettings
	{
	}

	public interface IStatementSerializationSettings : ISerializationSettings
	{
	}

	public interface IQuestionSerializationSettings : ISerializationSettings
	{
	}

	public interface IAnswerSerializationSettings : ISerializationSettings
	{
	}
}
