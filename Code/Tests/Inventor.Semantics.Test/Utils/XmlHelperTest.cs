using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using NUnit.Framework;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Test.Utils
{
	[TestFixture]
	public class XmlHelperTest
	{
		[Test]
		public void CheckCustomSerializers()
		{
			// arrange
			var customSerializer = new XmlSerializer(typeof(SerializableCustom));

			// act
			customSerializer.DefineCustomXmlSerializer<SerializableCustom>();
			var acquiredSerializer = typeof(SerializableCustom).AcquireXmlSerializer();

			// assert
			Assert.AreSame(customSerializer, acquiredSerializer);
		}

		[Test]
		public void AcquireSerializerTypedAndUntyped()
		{
			// act & assert
			Assert.AreSame(XmlHelper.AcquireXmlSerializer(typeof(SerializableClass1)), XmlHelper.AcquireXmlSerializer<SerializableClass1>());
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
				Assert.IsNotNull(XmlHelper.AcquireXmlSerializer(type));
			});
		}

		[Test]
		public void TestSerialization()
		{
			// arrange
			var test = Test.Create();
			string tempFileName = Path.GetTempFileName();

			// act
			XmlDocument serializedDocument;
			XmlElement serializedElement;
			Test deserializedFromStream, deserializedFromBytes, deserializedFromFile, deserializedFromText;
			try
			{
				serializedDocument = test.SerializeToXmlDocument();
				serializedElement = test.SerializeToXmlElement();
				test.SerializeToXmlFile(tempFileName);

				using (var xmlReader = new XmlTextReader(tempFileName))
				{
					deserializedFromStream = xmlReader.DeserializeFromXmlStream<Test>();
				}
				deserializedFromBytes = File.ReadAllBytes(tempFileName).DeserializeFromXmlBytes<Test>();
				deserializedFromFile = tempFileName.DeserializeFromXmlFile<Test>();
				deserializedFromText = serializedDocument.OuterXml.DeserializeFromXmlText<Test>();
			}
			finally
			{
				if (File.Exists(tempFileName))
				{
					File.Delete(tempFileName);
				}
			}

			// assert
			Assert.AreEqual(serializedDocument.DocumentElement.OuterXml, serializedElement.OuterXml);
			Assert.AreEqual(test, deserializedFromStream);
			Assert.AreEqual(test, deserializedFromBytes);
			Assert.AreEqual(test, deserializedFromFile);
			Assert.AreEqual(test, deserializedFromText);
		}

		#region Serializable classes

		[Serializable, XmlRoot(nameof(SerializableCustom))]
		public class SerializableCustom
		{
			[XmlElement]
			public string FieldCustom
			{ get; set; }
		}

		[XmlType]
		public class SerializableClass1
		{ }

		[XmlType]
		public class SerializableClass2
		{ }

		[XmlType]
		public class SerializableClass3
		{ }

		[XmlType]
		public class SerializableClass4
		{ }

		[XmlType]
		public class SerializableClass5
		{ }

		[XmlType]
		public class Test : IEquatable<Test>
		{
			#region Properties

			[XmlAttribute]
			public string String
			{ get; set; }

			[XmlElement]
			public int Int
			{ get; set; }

			[XmlAttribute]
			public DateTime DateTime
			{ get; set; }

			[XmlElement]
			public Test SingleChildObject
			{ get; set; }

			[XmlElement]
			public Test ChildObject
			{ get; set; }

			[XmlElement("ChildElement")]
			public List<Test> ChildElements
			{ get; set; }

			[XmlArray("Children")]
			[XmlArrayItem("Child")]
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
						DateTime == other.DateTime &&
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
