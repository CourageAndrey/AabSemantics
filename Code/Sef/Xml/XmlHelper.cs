using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

using Sef.Localization;

namespace Sef.Xml
{
	public static class XmlHelper
	{
		#region DOM

		#region Constants

		public const String UsedXmlVersion = "1.0";

		public const String UsedXmlEncoding = "UTF-8";

		#endregion

		#region Document

		public static XmlDocument CreateNewDocument(String rootName)
		{
			var document = CreateNewDocument();
			document.AppendChild(document.CreateElement(rootName));
			return document;
		}

		public static XmlDocument CreateNewDocument()
		{
			var document = new XmlDocument();
			document.AppendChild(document.CreateXmlDeclaration(UsedXmlVersion, UsedXmlEncoding, null));
			return document;
		}

		public static XmlElement GetRoot(this XmlDocument document)
		{
			return document.DocumentElement;
		}

		#endregion

		#region Child elements

		public static IList<XmlElement> SelectChildren(this XmlElement element, String name = null)
		{
			var elements = element.ChildNodes.OfType<XmlElement>();
			return (String.IsNullOrEmpty(name) ? elements : elements.Where(e => (e.LocalName == name))).ToList();
		}

		#endregion

		#region Text

		private static readonly Char[] trimmedChars = { ' ', '\r', '\n', '\t' };

		public static String GetElementText(this XmlElement element, Boolean trim)
		{
			foreach (var textNode in element.ChildNodes.OfType<XmlText>())
			{
				return trim ? textNode.Value.Trim(trimmedChars) : textNode.Value;
			}
			return null;
		}

		public static void SetElementText(this XmlElement element, XmlDocument document, String text)
		{
			element.AppendChild(document.CreateTextNode(text));
		}

		#endregion

		#region Attributes

		#region Work methods

		public static void SetAttributeEx<T>(this XmlElement element, String attribute, T value, IXmlConvertor<T> convertor = null)
		{
			element.SetAttribute(attribute, getConvertor(convertor).ConvertToString(value));
		}

		public static T GetAttributeEx<T>(this XmlElement element, String attribute, IXmlConvertor<T> convertor = null)
		{
			return getConvertor(convertor).ParseEx(element.GetAttribute(attribute));
		}

		public static void SetAttributeEnum<EnumT>(this XmlElement element, String attribute, EnumT value)
		  where EnumT : struct
		{
			element.SetAttribute(attribute, value.ToString());
		}

		public static EnumT GetAttributeEnum<EnumT>(this XmlElement element, String attribute)
		  where EnumT : struct
		{
			return (EnumT)Enum.Parse(typeof(EnumT), element.GetAttribute(attribute), true);
		}

		#endregion

		#region Conversion mechanism

		public static ICollection<Type> GetSupportedAttributeTypes()
		{
			var resultList = new List<Type>();
			resultList.AddRange(customConvertors.Keys);
			resultList.AddRange(defaultConvertors.Keys.Where(t => !resultList.Contains(t)));
			return resultList.AsReadOnly();
		}

		public static void RegisterCustomConvertor<T>(IXmlConvertor<T> convertor)
		{
			if (convertor != null)
			{
				customConvertors[typeof(T)] = convertor;
			}
			else
			{
				throw new ArgumentNullException("convertor");
			}
		}

		public static void RemoveCustomConvertor<T>()
		{
			if (customConvertors.ContainsKey(typeof(T)))
			{
				customConvertors.Remove(typeof(T));
			}
			else
			{
				throw new TypeNotSupportedException(typeof(T));
			}
		}

		private static IXmlConvertor<T> getConvertor<T>(IXmlConvertor<T> convertor)
		{
			if (convertor == null)
			{
				IXmlConvertor c;
				if (customConvertors.TryGetValue(typeof(T), out c) || defaultConvertors.TryGetValue(typeof(T), out c))
				{
					convertor = (IXmlConvertor<T>) c;
				}
				else
				{
					throw new TypeNotSupportedException(typeof(T));
				}
			}
			return convertor;
		}

		private static readonly Dictionary<Type, IXmlConvertor> defaultConvertors = new Dictionary<Type, IXmlConvertor>();
		private static readonly Dictionary<Type, IXmlConvertor> customConvertors = new Dictionary<Type, IXmlConvertor>();

		static XmlHelper()
		{
            IEnumerable<Type> types = Assembly.GetAssembly(typeof(IXmlConvertor)).GetTypes();
            types = types.Where(t => t.IsClass && t.IsSealed && typeof(IXmlConvertor).IsAssignableFrom(t) && (t.BaseType != null));

            foreach (var convertorType in types)
			{
			    if (convertorType.BaseType != null)
			    {
                    defaultConvertors[convertorType.BaseType.GetGenericArguments().Single()] = (IXmlConvertor) Activator.CreateInstance(convertorType);
			    }
			}
		}

		private class TypeNotSupportedException : NotSupportedException
		{
			public TypeNotSupportedException(Type type)
				: base(String.Format(Language.Current.Errors.TypeNotSerializable, type.FullName))
			{ }
		}

		#endregion

		#endregion

		#region Images

		public static Image LoadImage(this XmlElement element)
		{
			return Image.FromStream(new MemoryStream(Convert.FromBase64String(element.InnerText.discardWhiteSpaces())));
		}

		public static void SaveImage(this XmlElement element, Image image, ImageFormat imageFormat)
		{
			var stream = new MemoryStream();
			image.Save(stream, imageFormat);
			element.InnerText = Convert.ToBase64String(stream.ToArray());
		}

		private static String discardWhiteSpaces(this String text)
		{
			if (!String.IsNullOrEmpty(text))
			{
				var result = new Char[text.Count(c => !Char.IsWhiteSpace(c))];
				Int32 index = 0;
				foreach (var c in text)
				{
					if (!Char.IsWhiteSpace(c))
					{
						result[index++] = c;
					}
				}
				return new String(result);
			}
			else
			{
				return text;
			}
		}

		#endregion

		#endregion

		#region Simple attributes

		#region Serializers cache

		private static readonly Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();

		private static readonly Object serializersLock = new Object();

		public static XmlSerializer AcquireSerializer<T>()
		{
			return AcquireSerializer(typeof(T));
		}

		public static XmlSerializer AcquireSerializer(Type type)
		{
			lock (serializersLock)
			{
				XmlSerializer serializer;
				if (!serializers.TryGetValue(type, out serializer))
				{
					serializer = serializers[type] = new XmlSerializer(type);
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

		#endregion
	}
}
