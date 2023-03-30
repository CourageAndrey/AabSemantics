using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Inventor.Semantics.Serialization.Json
{
	public static class JsonHelper
	{
		public static readonly Encoding Encoding = Encoding.UTF8;

		#region Serializers cache

		private static readonly Dictionary<Type, DataContractJsonSerializer> _serializers = new Dictionary<Type, DataContractJsonSerializer>();
		private static readonly Object _serializersLock = new Object();

		public static DataContractJsonSerializer AcquireJsonSerializer<T>()
		{
			return AcquireJsonSerializer(typeof(T));
		}

		public static DataContractJsonSerializer AcquireJsonSerializer(this Type type)
		{
			lock (_serializersLock)
			{
				DataContractJsonSerializer serializer;
				if (!_serializers.TryGetValue(type, out serializer))
				{
					serializer = _serializers[type] = new DataContractJsonSerializer(type);
				}
				return serializer;
			}
		}

		public static void DefineCustomJsonSerializer(this Type type, DataContractJsonSerializer serializer)
		{
			lock (_serializersLock)
			{
				_serializers[type] = serializer;
			}
		}

		public static void DefineCustomJsonSerializer<T>(this DataContractJsonSerializer serializer)
		{
			typeof(T).DefineCustomJsonSerializer(serializer);
		}

		#endregion

		#region Serialization

		public static String SerializeToJsonString(this Object entity)
		{
			Byte[] jsonBytes;
			using (var memoryStream = new MemoryStream())
			{
				AcquireJsonSerializer(entity.GetType()).WriteObject(memoryStream, entity);
				jsonBytes = memoryStream.ToArray();
			}
			return Encoding.GetString(jsonBytes, 0, jsonBytes.Length);
		}

		public static void SerializeToJsonFile(this Object entity, String fileName)
		{
			File.WriteAllText(fileName, entity.SerializeToJsonString());
		}

		#endregion

		#region Deserialization

		public static T DeserializeFromJsonStream<T>(this Stream stream)
		{
			return (T) AcquireJsonSerializer<T>().ReadObject(stream);
		}

		public static T DeserializeFromJsonBytes<T>(this Byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				return memoryStream.DeserializeFromJsonStream<T>();
			}
		}

		public static T DeserializeFromJsonFile<T>(this String file)
		{
			return File.ReadAllBytes(file).DeserializeFromJsonBytes<T>();
		}

		public static T DeserializeFromJsonText<T>(this String json)
		{
			return Encoding.GetBytes(json).DeserializeFromJsonBytes<T>();
		}

		#endregion
	}
}
