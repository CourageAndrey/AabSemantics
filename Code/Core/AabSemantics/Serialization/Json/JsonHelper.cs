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
		public static String SerializeToJsonString(this Object entity)
		{
			var jsonBytes = entity.SerializeToJsonBytes();
			return Encoding.GetString(jsonBytes, 0, jsonBytes.Length);
		}

		/// <summary>Blocking counterpart of <see cref="SerializeToJsonFileAsync"/>.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <param name="fileName">Path to write to.</param>
		public static void SerializeToJsonFile(this Object entity, String fileName)
		{
			TaskHelper.AwaitDetached(() => SerializeToJsonFileAsync(entity, fileName));
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="SerializeToJsonString"/>. The serializer has no
		/// asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The JSON text.</returns>
		public static Task<String> SerializeToJsonStringAsync(this Object entity)
		{
			return TaskHelper.FromSynchronous(() => entity.SerializeToJsonString());
		}

		/// <summary>Serializes an object to a file, overwriting it.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <param name="fileName">Path to write to.</param>
		public static async Task SerializeToJsonFileAsync(this Object entity, String fileName)
		{
			await AsyncFile.WriteAllBytesAsync(fileName, entity.SerializeToJsonBytes()).ConfigureAwait(false);
		}

		/// <summary>Serializes an object to JSON bytes in the <see cref="Encoding"/> of this helper.</summary>
		/// <param name="entity">Object to serialize.</param>
		/// <returns>The JSON text as bytes.</returns>
		private static Byte[] SerializeToJsonBytes(this Object entity)
		{
			using (var memoryStream = new MemoryStream())
			{
				var serializer = AcquireJsonSerializer(entity.GetType());
				serializer.WriteObject(memoryStream, entity);
				return memoryStream.ToArray();
			}
		}

		#endregion

		#region Deserialization

		/// <summary>Deserializes an object from a JSON stream.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonStream<T>(this Stream stream)
		{
			var serializer = AcquireJsonSerializer<T>();
			return (T) serializer.ReadObject(stream);
		}

		/// <summary>Deserializes an object from JSON held in memory.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="bytes">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonBytes<T>(this Byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				return memoryStream.DeserializeFromJsonStream<T>();
			}
		}

		/// <summary>Blocking counterpart of <see cref="DeserializeFromJsonFileAsync{T}"/>.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="file">Path to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonFile<T>(this String file)
		{
			return File.ReadAllBytes(file).DeserializeFromJsonBytes<T>();
		}

		/// <summary>Deserializes an object from a JSON string.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="json">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static T DeserializeFromJsonString<T>(this String json)
		{
			return Encoding.GetBytes(json).DeserializeFromJsonBytes<T>();
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="DeserializeFromJsonStream{T}"/>. The serializer has
		/// no asynchronous API, so the stream is drained on the calling thread and the returned task
		/// is completed.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static Task<T> DeserializeFromJsonStreamAsync<T>(this Stream stream)
		{
			return TaskHelper.FromSynchronous(() => stream.DeserializeFromJsonStream<T>());
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="DeserializeFromJsonBytes{T}"/>. The serializer has
		/// no asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="bytes">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static Task<T> DeserializeFromJsonBytesAsync<T>(this Byte[] bytes)
		{
			return TaskHelper.FromSynchronous(() => bytes.DeserializeFromJsonBytes<T>());
		}

		/// <summary>Deserializes an object from a JSON file.</summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="file">Path to read from.</param>
		/// <returns>The deserialized object.</returns>
		public static async Task<T> DeserializeFromJsonFileAsync<T>(this String file)
		{
			// the file is read into memory first, as the serializer itself cannot read asynchronously
			var bytes = await AsyncFile.ReadAllBytesAsync(file).ConfigureAwait(false);
			return bytes.DeserializeFromJsonBytes<T>();
		}

		/// <summary>
		/// Asynchronous counterpart of <see cref="DeserializeFromJsonString{T}"/>. The serializer has
		/// no asynchronous API and nothing but memory is touched, so the returned task is completed.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="json">JSON text.</param>
		/// <returns>The deserialized object.</returns>
		public static Task<T> DeserializeFromJsonStringAsync<T>(this String json)
		{
			return TaskHelper.FromSynchronous(() => json.DeserializeFromJsonString<T>());
		}

		#endregion
	}
}
