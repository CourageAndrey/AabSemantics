using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using AabSemantics.Utils;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>
	/// XML serialization plumbing: a process-wide cache of <see cref="XmlSerializer"/> instances,
	/// the type-override mechanism that keeps polymorphic properties working, and read/write
	/// helpers for strings, documents and files.
	/// <para>
	/// The cache exists because constructing an <see cref="XmlSerializer"/> emits an assembly at
	/// run time and is therefore expensive. It is guarded by a lock, but the serializers it holds
	/// are shared, so a type's overrides must be defined before anything of that type is serialized.
	/// </para>
	/// </summary>
	public static class XmlHelper
	{
		#region Serializers cache

		private static readonly Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
		private static readonly Object _serializersLock = new Object();

		/// <summary>
		/// Empties the serializer cache, discarding any custom serializers and type overrides
		/// defined so far. Mainly for tests that re-register metadata.
		/// </summary>
		public static void ResetCache()
		{
			lock (_serializersLock)
			{
				_serializers.Clear();
			}
		}

		/// <summary>Returns the cached serializer for a type, creating it on first use.</summary>
		/// <typeparam name="T">Type to serialize.</typeparam>
		/// <returns>The serializer.</returns>
		public static XmlSerializer AcquireXmlSerializer<T>()
		{
			return AcquireXmlSerializer(typeof(T));
		}

		/// <summary>Returns the cached serializer for a type, creating it on first use.</summary>
		/// <param name="type">Type to serialize.</param>
		/// <returns>The serializer.</returns>
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

		/// <summary>Replaces the cached serializer for a type.</summary>
		/// <param name="type">Type the serializer applies to.</param>
		/// <param name="serializer">Serializer to cache.</param>
		public static void DefineCustomXmlSerializer(this Type type, XmlSerializer serializer)
		{
			lock (_serializersLock)
			{
				_serializers[type] = serializer;
			}
		}

		/// <summary>Replaces the cached serializer for a type.</summary>
		/// <typeparam name="T">Type the serializer applies to.</typeparam>
		/// <param name="serializer">Serializer to cache.</param>
		public static void DefineCustomXmlSerializer<T>(this XmlSerializer serializer)
		{
			typeof(T).DefineCustomXmlSerializer(serializer);
		}

		/// <summary>
		/// Builds and caches a serializer that maps a set of element names to concrete types for
		/// the given properties. This is what lets a property declared as an abstract surrogate
		/// hold any registered implementation.
		/// </summary>
		/// <param name="type">Type whose serializer is being replaced.</param>
		/// <param name="overrides">One entry per polymorphic property.</param>
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

		/// <summary>Builds and caches a serializer with property type overrides.</summary>
		/// <typeparam name="T">Type whose serializer is being replaced.</typeparam>
		/// <param name="overrides">One entry per polymorphic property.</param>
		public static void DefineTypeOverrides<T>(IEnumerable<PropertyTypes> overrides)
		{
			typeof(T).DefineTypeOverrides(overrides);
		}

		/// <summary>Builds and caches a serializer with a single property type override.</summary>
		/// <param name="type">Type whose serializer is being replaced.</param>
		/// <param name="propertyOverride">The polymorphic property to configure.</param>
		public static void DefineTypeOverride(this Type type, PropertyTypes propertyOverride)
		{
			type.DefineTypeOverrides(new[] { propertyOverride });
		}

		/// <summary>Describes one polymorphic property: which element name maps to which concrete type.</summary>
		public class PropertyTypes
		{
			/// <summary>Name of the property being overridden.</summary>
			public String PropertyName
			{ get; }

			/// <summary>Type that declares the property, which may be a base of the serialized type.</summary>
			public Type PropertyType
			{ get; }

			/// <summary>Element name to concrete type, one entry per accepted implementation.</summary>
			public IDictionary<String, Type> Implementations
			{ get; }

			/// <summary>Describes a polymorphic property.</summary>
			/// <param name="propertyName">Name of the property.</param>
			/// <param name="propertyType">Type that declares it.</param>
			/// <param name="implementations">Element name to concrete type.</param>
			public PropertyTypes(string propertyName, Type propertyType, IDictionary<string, Type> implementations)
			{
				PropertyName = propertyName;
				PropertyType = propertyType;
				Implementations = implementations;
			}
		}

		#endregion

		#region Serialization

		/// <summary>Serializes an object to an XML string.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The XML markup.</returns>
		public static String SerializeToXmlString(this Object entity)
		{
			return entity.SerializeToXmlDocument().OuterXml;
		}

		/// <summary>Serializes an object to an XML document, stripping the xsd and xsi namespace declarations.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The document.</returns>
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

		/// <summary>Serializes an object to an XML element.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The document element.</returns>
		public static XmlElement SerializeToXmlElement(this Object entity)
		{
			return entity.SerializeToXmlDocument().DocumentElement;
		}

		/// <summary>Blocking counterpart of <see cref="SerializeToXmlFileAsync"/>.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <param name="fileName">Path to write to.</param>
		public static void SerializeToXmlFile(this Object entity, String fileName)
		{
			TaskHelper.AwaitDetached(() => SerializeToXmlFileAsync(entity, fileName));
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="SerializeToXmlString"/>. The serializer has no
		/// asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The XML markup.</returns>
		public static Task<String> SerializeToXmlStringAsync(this Object entity)
		{
			return TaskHelper.FromSynchronous(() => entity.SerializeToXmlString());
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="SerializeToXmlDocument"/>. The serializer has no
		/// asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The document.</returns>
		public static Task<XmlDocument> SerializeToXmlDocumentAsync(this Object entity)
		{
			return TaskHelper.FromSynchronous(() => entity.SerializeToXmlDocument());
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="SerializeToXmlElement"/>. The serializer has no
		/// asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The document element.</returns>
		public static Task<XmlElement> SerializeToXmlElementAsync(this Object entity)
		{
			return TaskHelper.FromSynchronous(() => entity.SerializeToXmlElement());
		}

		/// <summary>Serializes an object to a file, overwriting it.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <param name="fileName">Path to write to.</param>
		public static async Task SerializeToXmlFileAsync(this Object entity, String fileName)
		{
			var document = entity.SerializeToXmlDocument();

			// the document is written to memory first, as XmlDocument itself cannot save asynchronously
			using (var buffer = new MemoryStream())
			{
				document.Save(buffer);
				await AsyncFile.WriteAllBytesAsync(fileName, buffer.ToArray()).ConfigureAwait(false);
			}
		}

		#endregion

		#region Deserialization

		/// <summary>Deserializes an object from an XML reader.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="reader">Reader positioned at the object.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromXmlStream<T>(this XmlReader reader)
		{
			var serializer = AcquireXmlSerializer<T>();
			return (T) serializer.Deserialize(reader);
		}

		/// <summary>Deserializes an object from XML held in memory.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="bytes">XML markup.</param>
		/// <returns>The deserialized object.</returns>
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

		/// <summary>Blocking counterpart of <see cref="DeserializeFromXmlFileAsync{T}"/>.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="file">Path to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromXmlFile<T>(this String file)
		{
			using (var xmlFile = new XmlTextReader(file))
			{
				return xmlFile.DeserializeFromXmlStream<T>();
			}
		}

		/// <summary>Deserializes an object from an XML string.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="xml">XML markup.</param>
		/// <returns>The deserialized object.</returns>
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

		/// <summary>
		/// Asynchronous counterpart of <see cref="DeserializeFromXmlStream{T}"/>. The serializer has
		/// no asynchronous API, so the reader is drained on the calling thread and the returned task
		/// is completed.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="reader">Reader positioned at the object.</param>
		/// <returns>The deserialized object.</returns>
		public static Task<T> DeserializeFromXmlStreamAsync<T>(this XmlReader reader)
		{
			return TaskHelper.FromSynchronous(() => reader.DeserializeFromXmlStream<T>());
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="DeserializeFromXmlBytes{T}"/>. The serializer has
		/// no asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="bytes">XML markup.</param>
		/// <returns>The deserialized object.</returns>
		public static Task<T> DeserializeFromXmlBytesAsync<T>(this Byte[] bytes)
		{
			return TaskHelper.FromSynchronous(() => bytes.DeserializeFromXmlBytes<T>());
		}

		/// <summary>Deserializes an object from an XML file.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="file">Path to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static async Task<T> DeserializeFromXmlFileAsync<T>(this String file)
		{
			// the file is read into memory first, as the serializer itself cannot read asynchronously
			var bytes = await AsyncFile.ReadAllBytesAsync(file).ConfigureAwait(false);
			return bytes.DeserializeFromXmlBytes<T>();
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="DeserializeFromXmlString{T}"/>. The serializer has
		/// no asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="xml">XML markup.</param>
		/// <returns>The deserialized object.</returns>
		public static Task<T> DeserializeFromXmlStringAsync<T>(this String xml)
		{
			return TaskHelper.FromSynchronous(() => xml.DeserializeFromXmlString<T>());
		}

		#endregion
	}
}
