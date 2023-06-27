using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AabSemantics.Serialization.Xml
{
	public static class XmlHelper
	{
		#region Serializers cache

		private static readonly Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
		private static readonly Object _serializersLock = new Object();

		public static void ResetCache()
		{
			lock (_serializersLock)
			{
				_serializers.Clear();
			}
		}

		public static XmlSerializer AcquireXmlSerializer<T>()
		{
			return AcquireXmlSerializer(typeof(T));
		}

		public static XmlSerializer AcquireXmlSerializer(this Type type)
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

		public static void DefineCustomXmlSerializer(this Type type, XmlSerializer serializer)
		{
			lock (_serializersLock)
			{
				_serializers[type] = serializer;
			}
		}

		public static void DefineCustomXmlSerializer<T>(this XmlSerializer serializer)
		{
			typeof(T).DefineCustomXmlSerializer(serializer);
		}

		public static void DefineTypeOverrides(this Type type, IEnumerable<PropertyTypes> overrides)
		{
			var attributeOverrides = new XmlAttributeOverrides();

			foreach (var propertyOverride in overrides)
			{
				var statementAttributes = new XmlAttributes();
				foreach (var implementation in propertyOverride.Implementations)
				{
					statementAttributes.XmlElements.Add(new XmlElementAttribute(implementation.Key, implementation.Value));
				}
				attributeOverrides.Add(propertyOverride.PropertyType, propertyOverride.PropertyName, statementAttributes);
			}

			var serializer = new XmlSerializer(type, attributeOverrides);
			type.DefineCustomXmlSerializer(serializer);
		}

		public static void DefineTypeOverrides<T>(IEnumerable<PropertyTypes> overrides)
		{
			typeof(T).DefineTypeOverrides(overrides);
		}

		public static void DefineTypeOverride(this Type type, PropertyTypes propertyOverride)
		{
			type.DefineTypeOverrides(new[] { propertyOverride });
		}

		public static void DefineTypeOverride<T>(PropertyTypes propertyOverride)
		{
			typeof(T).DefineTypeOverride(propertyOverride);
		}

		public class PropertyTypes
		{
			public String PropertyName
			{ get; }

			public Type PropertyType
			{ get; }

			public IDictionary<String, Type> Implementations
			{ get; }

			public PropertyTypes(string propertyName, Type propertyType, IDictionary<string, Type> implementations)
			{
				PropertyName = propertyName;
				PropertyType = propertyType;
				Implementations = implementations;
			}
		}

		#endregion

		#region Serialization

		public static String SerializeToXmlString(this Object entity)
		{
			return entity.SerializeToXmlDocument().OuterXml;
		}

		public static XmlDocument SerializeToXmlDocument(this Object entity)
		{
			var serializer = AcquireXmlSerializer(entity.GetType());
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

		public static XmlElement SerializeToXmlElement(this Object entity)
		{
			return entity.SerializeToXmlDocument().DocumentElement;
		}

		public static void SerializeToXmlFile(this Object entity, String fileName)
		{
			entity.SerializeToXmlDocument().Save(fileName);
		}

		#endregion

		#region Deserialization

		public static T DeserializeFromXmlStream<T>(this XmlReader reader)
		{
			return (T) AcquireXmlSerializer<T>().Deserialize(reader);
		}

		public static T DeserializeFromXmlBytes<T>(this Byte[] bytes)
		{
			using (var stream = new MemoryStream(bytes))
			{
				using (var reader = XmlReader.Create(stream))
				{
					return reader.DeserializeFromXmlStream<T>();
				}
			}
		}

		public static T DeserializeFromXmlFile<T>(this String file)
		{
			using (var xmlFile = new XmlTextReader(file))
			{
				return DeserializeFromXmlStream<T>(xmlFile);
			}
		}

		public static T DeserializeFromXmlString<T>(this String xml)
		{
			using (var stringReader = new StringReader(xml))
			{
				using (var xmlStringReader = new XmlTextReader(stringReader))
				{
					return xmlStringReader.DeserializeFromXmlStream<T>();
				}
			}
		}

		#endregion
	}
}
