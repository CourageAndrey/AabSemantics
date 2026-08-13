using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics.Serialization.Json
{
	/// <summary>JSON serialization plumbing, mirroring <see cref="AabSemantics.Serialization.Xml.XmlHelper"/>: a locked process-wide serializer cache plus read/write helpers.</summary>
	public static class JsonHelper
	{
		/// <summary>Encoding used for every JSON string and file.</summary>
		public static readonly Encoding Encoding = Encoding.UTF8;

		#region Serializers cache

		private static readonly Dictionary<Type, DataContractJsonSerializer> _serializers = new Dictionary<Type, DataContractJsonSerializer>();
		private static readonly Object _serializersLock = new Object();

		/// <summary>Returns the cached serializer for a type, creating it on first use.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <returns>The serializer.</returns>
		public static DataContractJsonSerializer AcquireJsonSerializer<T>()
		{
			return AcquireJsonSerializer(typeof(T));
		}

		/// <summary>Returns the cached serializer for a type, creating it on first use.</summary>
		/// <param name="type">Type to serialize.</param>
		/// <returns>The serializer.</returns>
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

		/// <summary>Replaces the cached serializer for a type, e.g. to teach it extra known types.</summary>
		/// <param name="type">Type the serializer applies to.</param>
		/// <param name="serializer">Serializer to cache.</param>
		public static void DefineCustomJsonSerializer(this Type type, DataContractJsonSerializer serializer)
		{
			lock (_serializersLock)
			{
				_serializers[type] = serializer;
			}
		}

		/// <summary>Replaces the cached serializer for a type.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="serializer">Serializer to cache.</param>
		public static void DefineCustomJsonSerializer<T>(this DataContractJsonSerializer serializer)
		{
			typeof(T).DefineCustomJsonSerializer(serializer);
		}

		#endregion

		#region Serialization

		/// <summary>Serializes an object to a JSON string.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The JSON text.</returns>
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

		/// <summary>Serializes an object to a file, overwriting it.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <param name="fileName">Path to write to.</param>
		public static async Task SerializeToJsonFileAsync(this Object entity, String fileName)
		{
			string json = await entity.SerializeToJsonStringAsync();
			await Task.Run(() => File.WriteAllText(fileName, json)).ConfigureAwait(false);
		}

		/// <summary>Blocking counterpart of <see cref="SerializeToJsonStringAsync"/>.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The JSON text.</returns>
		public static String SerializeToJsonString(this Object entity)
		{
			return TaskHelper.AwaitDetached(() => SerializeToJsonStringAsync(entity));
		}

		/// <summary>Blocking counterpart of <see cref="SerializeToJsonFileAsync"/>.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <param name="fileName">Path to write to.</param>
		public static void SerializeToJsonFile(this Object entity, String fileName)
		{
			TaskHelper.AwaitDetached(() => SerializeToJsonFileAsync(entity, fileName));
		}

		#endregion

		#region Deserialization

		/// <summary>Deserializes an object from a JSON stream.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static async Task<T> DeserializeFromJsonStreamAsync<T>(this Stream stream)
		{
			var serializer = AcquireJsonSerializer<T>();
			return await Task.Run(() => (T) serializer.ReadObject(stream)).ConfigureAwait(false);
		}

		/// <summary>Deserializes an object from JSON held in memory.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="bytes">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static async Task<T> DeserializeFromJsonBytesAsync<T>(this Byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				return await memoryStream.DeserializeFromJsonStreamAsync<T>();
			}
		}

		/// <summary>Deserializes an object from a JSON file.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="file">Path to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static async Task<T> DeserializeFromJsonFileAsync<T>(this String file)
		{
			return await File.ReadAllBytes(file).DeserializeFromJsonBytesAsync<T>();
		}

		/// <summary>Deserializes an object from a JSON string.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="json">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static async Task<T> DeserializeFromJsonStringAsync<T>(this String json)
		{
			return await Encoding.GetBytes(json).DeserializeFromJsonBytesAsync<T>();
		}

		/// <summary>Blocking counterpart of <see cref="DeserializeFromJsonStreamAsync{T}"/>.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonStream<T>(this Stream stream)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonStreamAsync<T>(stream));
		}

		/// <summary>Blocking counterpart of <see cref="DeserializeFromJsonBytesAsync{T}"/>.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="bytes">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonBytes<T>(this Byte[] bytes)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonBytesAsync<T>(bytes));
		}

		/// <summary>Blocking counterpart of <see cref="DeserializeFromJsonFileAsync{T}"/>.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="file">Path to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonFile<T>(this String file)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonFileAsync<T>(file));
		}

		/// <summary>Blocking counterpart of <see cref="DeserializeFromJsonStringAsync{T}"/>.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="json">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonString<T>(this String json)
		{
			return TaskHelper.AwaitDetached(() => DeserializeFromJsonStringAsync<T>(json));
		}

		#endregion
	}
}
