using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Inventor.Semantics.Utils
{
	public static class XmlHelper
	{
		#region Serializers cache

		private static readonly Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
		private static readonly Object _serializersLock = new Object();

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

		#endregion

		#region Serialization

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

		public static T DeserializeFromXmlText<T>(this String xml)
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
