using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Inventor.Semantics.Utils
{
	public static class JsonHelper
	{
		public static String SerializeToJsonString(this DataContractJsonSerializer serializer, Object snapshot)
		{
			Byte[] jsonBytes;
			using (var memoryStream = new MemoryStream())
			{
				serializer.WriteObject(memoryStream, snapshot);
				jsonBytes = memoryStream.ToArray();
			}
			return Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
		}

		public static Object DeserializeFromJsonString(this DataContractJsonSerializer serializer, String json)
		{
			Byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
			using (var memoryStream = new MemoryStream(jsonBytes))
			{
				memoryStream.Seek(0, SeekOrigin.Begin);
				return serializer.ReadObject(memoryStream);
			}
		}
	}
}
