using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

using NUnit.Framework;

using AabSemantics.Serialization.Json;

namespace AabSemantics.Test.Serialization.Json
{
	[TestFixture]
	public class JsonHelperTest
	{
		[Test]
		public void CheckCustomSerializers()
		{
			// arrange
			var customSerializer = new DataContractJsonSerializer(typeof(SerializableCustom));

			// act
			customSerializer.DefineCustomJsonSerializer<SerializableCustom>();
			var acquiredSerializer = typeof(SerializableCustom).AcquireJsonSerializer();

			// assert
			Assert.AreSame(customSerializer, acquiredSerializer);
		}

		[Test]
		public void AcquireSerializerTypedAndUntyped()
		{
			// act & assert
			Assert.AreSame(JsonHelper.AcquireJsonSerializer(typeof(SerializableClass1)), JsonHelper.AcquireJsonSerializer<SerializableClass1>());
		}

		[Test]
		public void AcquireSerializerMultithreading()
		{
			// arrange
			const int threadsPerType = 10;
			var threadTypes = new List<Type>();
			for (int t = 0; t < threadsPerType; t++)
			{
				foreach (var type in new[]
				{
					typeof(SerializableClass1),
					typeof(SerializableClass2),
					typeof(SerializableClass3),
					typeof(SerializableClass4),
					typeof(SerializableClass5),
				})
				{
					threadTypes.Add(type);
				}
			}

			// act & assert
			Parallel.ForEach(threadTypes, type =>
			{
				Assert.IsNotNull(JsonHelper.AcquireJsonSerializer(type));
			});
		}

		[Test]
		public void TestSerialization()
		{
			// arrange
			var test = Test.Create();
			string tempFileName = Path.GetTempFileName();

			// act
			string serializedText;
			Test deserializedFromStream, deserializedFromBytes, deserializedFromFile, deserializedFromText;
			try
			{
				serializedText = test.SerializeToJsonString();
				test.SerializeToJsonFile(tempFileName);

				using (var fileReader = File.OpenRead(tempFileName))
				{
					deserializedFromStream = fileReader.DeserializeFromJsonStream<Test>();
				}
				deserializedFromBytes = File.ReadAllBytes(tempFileName).DeserializeFromJsonBytes<Test>();
				deserializedFromFile = tempFileName.DeserializeFromJsonFile<Test>();
				deserializedFromText = serializedText.DeserializeFromJsonString<Test>();
			}
			finally
			{
				if (File.Exists(tempFileName))
				{
					File.Delete(tempFileName);
				}
			}

			// assert
			Assert.AreEqual(test, deserializedFromStream);
			Assert.AreEqual(test, deserializedFromBytes);
			Assert.AreEqual(test, deserializedFromFile);
			Assert.AreEqual(test, deserializedFromText);
		}

		#region Serializable classes

		[DataContract]
		public class SerializableCustom
		{
			[DataMember]
			public string FieldCustom
			{ get; set; }
		}

		[DataContract]
		public class SerializableClass1
		{ }

		[DataContract]
		public class SerializableClass2
		{ }

		[DataContract]
		public class SerializableClass3
		{ }

		[DataContract]
		public class SerializableClass4
		{ }

		[DataContract]
		public class SerializableClass5
		{ }

		[DataContract]
		public class Test : IEquatable<Test>
		{
			#region Properties

			[DataMember]
			public string String
			{ get; set; }

			[DataMember]
			public int Int
			{ get; set; }

			[DataMember]
			public DateTime DateTime
			{ get; set; }

			[DataMember]
			public Test SingleChildObject
			{ get; set; }

			[DataMember]
			public Test ChildObject
			{ get; set; }

			[DataMember]
			public List<Test> ChildElements
			{ get; set; }

			[DataMember]
			public List<Test> Children
			{ get; set; }

			#endregion

			#region Constructors

			public Test()
				: this(null, 0, default(DateTime), null, Array.Empty<Test>(), Array.Empty<Test>())
			{ }

			public Test(string @string, int @int)
				: this(@string, @int, DateTime.Now, null, Array.Empty<Test>(), Array.Empty<Test>())
			{ }

			public Test(
				string @string,
				int @int,
				DateTime dateTime,
				Test singleChildObject,
				IEnumerable<Test> childElements,
				IEnumerable<Test> children)
			{
				String = @string;
				Int = @int;
				DateTime = dateTime;
				SingleChildObject = singleChildObject;
				ChildElements = new List<Test>(childElements);
				Children = new List<Test>(children);
			}

			#endregion

			public bool Equals(Test other)
			{
				return	String == other.String &&
						Int == other.Int &&
						(DateTime - other.DateTime).TotalSeconds < 1 && // because of JSON precision problems
						(SingleChildObject == null && other.SingleChildObject == null || SingleChildObject.Equals(other.SingleChildObject)) &&
						ChildElements.SequenceEqual(other.ChildElements) &&
						Children.SequenceEqual(other.Children);
			}

			public static Test Create()
			{
				return new Test(
					"Top-level parent",
					1,
					DateTime.Now,
					new Test("Single child", 2),
					new[] { new Test("Child 1.1", 3), new Test("Child 1.2", 4), },
					new[] { new Test("Child 2.1", 5), new Test("Child 2.2", 6), new Test("Child 2.3", 7), });
			}
		}

		#endregion
	}
}
