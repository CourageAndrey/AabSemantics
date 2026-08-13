using System;

namespace AabSemantics.Metadata
{
	/// <summary>
	/// Marker for one way of persisting a type. A metadata definition holds a list of these,
	/// one per supported format, so support for a new format is added without changing the
	/// definitions themselves.
	/// </summary>
	public interface ISerializationSettings
	{ }

	/// <summary>
	/// Settings for persisting a type as XML.
	/// </summary>
	public interface IXmlSerializationSettings : ISerializationSettings
	{
		/// <summary>
		/// XML surrogate type instances are converted into.
		/// </summary>
		Type XmlType
		{ get; }
	}

	/// <summary>
	/// Settings for persisting a type as JSON.
	/// </summary>
	public interface IJsonSerializationSettings : ISerializationSettings
	{
		/// <summary>
		/// JSON surrogate type instances are converted into, or <c>null</c> when the value
		/// is written without one.
		/// </summary>
		Type JsonType
		{ get; }
	}

	/// <summary>
	/// Marker constraining settings to those describing an attribute.
	/// </summary>
	public interface IAttributeSerializationSettings : ISerializationSettings
	{
	}

	/// <summary>
	/// Marker constraining settings to those describing a statement.
	/// </summary>
	public interface IStatementSerializationSettings : ISerializationSettings
	{
	}

	/// <summary>
	/// Marker constraining settings to those describing a question.
	/// </summary>
	public interface IQuestionSerializationSettings : ISerializationSettings
	{
	}

	/// <summary>
	/// Marker constraining settings to those describing an answer.
	/// </summary>
	public interface IAnswerSerializationSettings : ISerializationSettings
	{
	}
}
