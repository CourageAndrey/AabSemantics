using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Inventor.Core.Xml;

namespace Inventor.Core.Utils
{
	public static class XmlHelper
	{
		#region Serializers cache

		private static readonly Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
		private static readonly Object _serializersLock = new Object();

		public static XmlSerializer AcquireSerializer<T>()
		{
			return AcquireSerializer(typeof(T));
		}

		public static XmlSerializer AcquireSerializer(Type type)
		{
			lock (_serializersLock)
			{
				XmlSerializer serializer;
				if (!_serializers.TryGetValue(type, out serializer))
				{
					serializer = _serializers[type] = new XmlSerializer(type);
				}
				return serializer;
			}
		}

		#endregion

		#region Serialization

		public static XmlDocument SerializeToDocument(this Object entity)
		{
			var serializer = AcquireSerializer(entity.GetType());
			var document = new XmlDocument();
			using (var writer = new StringWriter())
			{
				serializer.Serialize(writer, entity);
				document.LoadXml(writer.ToString());
			}
			if (document.DocumentElement != null)
			{
				document.DocumentElement.RemoveAttribute("xmlns:xsd");
				document.DocumentElement.RemoveAttribute("xmlns:xsi");
			}
			return document;
		}

		public static XmlElement SerializeToElement(this Object entity)
		{
			return entity.SerializeToDocument().DocumentElement;
		}

		public static void SerializeToFile(this Object entity, String fileName)
		{
			entity.SerializeToDocument().Save(fileName);
		}

		#endregion

		#region Deserialization

		public static T DeserializeFromStream<T>(this XmlReader reader)
		{
			return (T) AcquireSerializer<T>().Deserialize(reader);
		}

		public static T Deserialize<T>(this Byte[] bytes)
		{
			using (var stream = new MemoryStream(bytes))
			{
				using (var reader = XmlReader.Create(stream))
				{
					return reader.DeserializeFromStream<T>();
				}
			}
		}

		public static T DeserializeFromFile<T>(this String file)
		{
			using (var xmlFile = new XmlTextReader(file))
			{
				return DeserializeFromStream<T>(xmlFile);
			}
		}

		public static T DeserializeFromText<T>(this String xml)
		{
			using (var stringReader = new StringReader(xml))
			{
				using (var xmlStringReader = new XmlTextReader(stringReader))
				{
					return xmlStringReader.DeserializeFromStream<T>();
				}
			}
		}

		#endregion

		public static void InitializeSemanticNetworkSerializer()
		{
			lock (_serializersLock)
			{
				var rootType = typeof(SemanticNetwork);
				var attributeOverrides = new XmlAttributeOverrides();

				var attributeAttributes = new XmlAttributes();
				foreach (var definition in Repositories.Attributes.Definitions.Values)
				{
					attributeAttributes.XmlElements.Add(new XmlElementAttribute(definition.XmlElementName, definition.XmlType));
				}
				attributeOverrides.Add(typeof(Concept), "Attributes", attributeAttributes);

				var statementAttributes = new XmlAttributes();
				foreach (var definition in Repositories.Statements.Definitions.Values)
				{
					statementAttributes.XmlElements.Add(new XmlElementAttribute(definition.XmlElementName, definition.XmlType));
				}
				attributeOverrides.Add(typeof(SemanticNetwork), "Statements", statementAttributes);

				_serializers[rootType] = new XmlSerializer(rootType, attributeOverrides);
			}
		}
	}
}
