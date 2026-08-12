using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics.Serialization.Json
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

		public static async Task<String> SerializeToJsonStringAsync(this Object entity)
		{
			Byte[] jsonBytes;
			using (var memoryStream = new MemoryStream())
			{
				var serializer = AcquireJsonSerializer(entity.GetType());
				await Task.Run(() => serializer.WriteObject(memoryStream, entity)).ConfigureAwait(false);
				jsonBytes = await Task.Run(() => memoryStream.ToArray()).ConfigureAwait(false);
			}
			return await Task.Run(() => Encoding.GetString(jsonBytes, 0, jsonBytes.Length)).ConfigureAwait(false);
		}

		public static async Task SerializeToJsonFileAsync(this Object entity, String fileName)
		{
			string json = await entity.SerializeToJsonStringAsync();
			await Task.Run(() => File.WriteAllText(fileName, json)).ConfigureAwait(false);
		}

		public static String SerializeToJsonString(this Object entity)
		{
			return TaskHelper.AwaitDetached(() => SerializeToJsonStringAsync(entity));
		}

		public static void SerializeToJsonFile(this Object entity, String fileName)
		{
			TaskHelper.AwaitDetached(() => SerializeToJsonFileAsync(entity, fileName));
		}

		#endregion

		#region Deserialization

		public static async Task<T> DeserializeFromJsonStreamAsync<T>(this Stream stream)
		{
			var serializer = AcquireJsonSerializer<T>();
			return await Task.Run(() => (T) serializer.ReadObject(stream)).ConfigureAwait(false);
		}

		public static async Task<T> DeserializeFromJsonBytesAsync<T>(this Byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				return await memoryStream.DeserializeFromJsonStreamAsync<T>();
			}
		}

		public static async Task<T> DeserializeFromJsonFileAsync<T>(this String file)
		{
			return await File.ReadAllBytes(file).DeserializeFromJsonBytesAsync<T>();
		}

		public static async Task<T> DeserializeFromJsonStringAsync<T>(this String json)
		{
			return await Encoding.GetBytes(json).DeserializeFromJsonBytesAsync<T>();
		}

		public static T DeserializeFromJsonStream<T>(this Stream stream)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonStreamAsync<T>(stream));
		}

		public static T DeserializeFromJsonBytes<T>(this Byte[] bytes)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonBytesAsync<T>(bytes));
		}

		public static T DeserializeFromJsonFile<T>(this String file)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonFileAsync<T>(file));
		}

		public static T DeserializeFromJsonString<T>(this String json)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonStringAsync<T>(json));
		}

		#endregion
	}
}
