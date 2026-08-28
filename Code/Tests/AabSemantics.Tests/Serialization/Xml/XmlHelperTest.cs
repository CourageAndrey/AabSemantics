using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using NUnit.Framework;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Tests.Serialization.Xml
{
	[TestFixture]
	public class XmlHelperTest
	{
		[Test]
		public void GivenXmlHelper_WhenResetCache_ThenRemoveAllDefinedSerializers()
		{
			// arrange
			var serializersField = typeof(XmlHelper).GetField("_serializers", BindingFlags.GetField | BindingFlags.Static | BindingFlags.NonPublic);
			var serializers = (IDictionary<Type, XmlSerializer>) serializersField.GetValue(null);

			typeof(SerializableClass1).AcquireXmlSerializer();
			typeof(SerializableClass2).AcquireXmlSerializer();
			typeof(SerializableClass3).AcquireXmlSerializer();
			typeof(SerializableClass4).AcquireXmlSerializer();
			typeof(SerializableClass5).AcquireXmlSerializer();
			Assert.That(serializers.Count, Is.GreaterThan(0));

			// act
			XmlHelper.ResetCache();

			// assert
			Assert.That(serializers.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenCustomSerializer_WhenAcquireSerializer_ThenReturnCustom()
		{
			// arrange
			var customSerializer = new XmlSerializer(typeof(SerializableCustom));

			// act
			customSerializer.DefineCustomXmlSerializer<SerializableCustom>();
			var acquiredSerializer = typeof(SerializableCustom).AcquireXmlSerializer();

			// assert
			Assert.That(acquiredSerializer, Is.SameAs(customSerializer));
		}

		[Test]
		public void GivenTypedAndUntypedOverloads_WhenAcquireSerializer_ThenWorkTheSame()
		{
			// act & assert
			Assert.That(XmlHelper.AcquireXmlSerializer<SerializableClass1>(), Is.SameAs(XmlHelper.AcquireXmlSerializer(typeof(SerializableClass1))));
		}

		[Test]
		public void GivenMultithreading_WhenAcquireSerializer_ThenSucceed()
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
				Assert.That(XmlHelper.AcquireXmlSerializer(type), Is.Not.Null);
			});
		}

		[Test]
		public void GivenDifferentWays_WhenCheckSerialization_ThenAllWorkTheSame()
		{
			// arrange
			var test = Test.Create();
			string tempFileName = Path.GetTempFileName();

			// act
			string serializedString;
			XmlDocument serializedDocument;
			XmlElement serializedElement;
			Test deserializedFromStream, deserializedFromBytes, deserializedFromFile, deserializedFromDocument, deserializedFromString;
			try
			{
				serializedString = test.SerializeToXmlString();
				serializedDocument = test.SerializeToXmlDocument();
				serializedElement = test.SerializeToXmlElement();
				test.SerializeToXmlFile(tempFileName);

				using (var xmlReader = new XmlTextReader(tempFileName))
				{
					deserializedFromStream = xmlReader.DeserializeFromXmlStream<Test>();
				}
				deserializedFromBytes = File.ReadAllBytes(tempFileName).DeserializeFromXmlBytes<Test>();
				deserializedFromFile = tempFileName.DeserializeFromXmlFile<Test>();
				deserializedFromDocument = serializedDocument.OuterXml.DeserializeFromXmlString<Test>();
				deserializedFromString = serializedString.DeserializeFromXmlString<Test>();
			}
			finally
			{
				if (File.Exists(tempFileName))
				{
					File.Delete(tempFileName);
				}
			}

			// assert
			Assert.That(serializedElement.OuterXml, Is.EqualTo(serializedDocument.DocumentElement.OuterXml));
			Assert.That(deserializedFromStream, Is.EqualTo(test));
			Assert.That(deserializedFromBytes, Is.EqualTo(test));
			Assert.That(deserializedFromFile, Is.EqualTo(test));
			Assert.That(deserializedFromDocument, Is.EqualTo(test));
			Assert.That(deserializedFromString, Is.EqualTo(test));
		}

		[Test]
		public async Task GivenDifferentAsyncWays_WhenCheckSerialization_ThenAllWorkTheSame()
		{
			// arrange
			var test = Test.Create();
			string tempFileName = Path.GetTempFileName();

			// act
			string serializedString;
			XmlDocument serializedDocument;
			XmlElement serializedElement;
			Test deserializedFromStream, deserializedFromBytes, deserializedFromFile, deserializedFromString;
			try
			{
				var serializedStringTask = test.SerializeToXmlStringAsync();
				var serializedDocumentTask = test.SerializeToXmlDocumentAsync();
				var serializedElementTask = test.SerializeToXmlElementAsync();

				// in-memory serialization has nothing to await
				Assert.That(serializedStringTask.IsCompleted, Is.True);
				Assert.That(serializedDocumentTask.IsCompleted, Is.True);
				Assert.That(serializedElementTask.IsCompleted, Is.True);

				serializedString = await serializedStringTask;
				serializedDocument = await serializedDocumentTask;
				serializedElement = await serializedElementTask;

				await test.SerializeToXmlFileAsync(tempFileName);

				using (var xmlReader = new XmlTextReader(tempFileName))
				{
					deserializedFromStream = await xmlReader.DeserializeFromXmlStreamAsync<Test>();
				}
				deserializedFromBytes = await File.ReadAllBytes(tempFileName).DeserializeFromXmlBytesAsync<Test>();
				deserializedFromFile = await tempFileName.DeserializeFromXmlFileAsync<Test>();
				deserializedFromString = await serializedString.DeserializeFromXmlStringAsync<Test>();
			}
			finally
			{
				if (File.Exists(tempFileName))
				{
					File.Delete(tempFileName);
				}
			}

			// assert
			Assert.That(serializedString, Is.EqualTo(test.SerializeToXmlString()));
			Assert.That(serializedElement.OuterXml, Is.EqualTo(serializedDocument.DocumentElement.OuterXml));
			Assert.That(deserializedFromStream, Is.EqualTo(test));
			Assert.That(deserializedFromBytes, Is.EqualTo(test));
			Assert.That(deserializedFromFile, Is.EqualTo(test));
			Assert.That(deserializedFromString, Is.EqualTo(test));
		}

		[Test]
		public void GivenCancelledToken_WhenSerializeOrDeserialize_ThenThrow()
		{
			// arrange
			var test = Test.Create();
			string tempFileName = Path.GetTempFileName();

			try
			{
				test.SerializeToXmlFile(tempFileName);
				string serializedString = test.SerializeToXmlString();
				byte[] serializedBytes = File.ReadAllBytes(tempFileName);

				using (var tokenSource = new CancellationTokenSource())
				{
					tokenSource.Cancel();
					var token = tokenSource.Token;

					// act & assert
					Assert.That(async () => await test.SerializeToXmlStringAsync(token), Throws.InstanceOf<OperationCanceledException>());
					Assert.That(async () => await test.SerializeToXmlDocumentAsync(token), Throws.InstanceOf<OperationCanceledException>());
					Assert.That(async () => await test.SerializeToXmlElementAsync(token), Throws.InstanceOf<OperationCanceledException>());
					Assert.That(async () => await test.SerializeToXmlFileAsync(tempFileName, token), Throws.InstanceOf<OperationCanceledException>());
					Assert.That(async () => await serializedBytes.DeserializeFromXmlBytesAsync<Test>(token), Throws.InstanceOf<OperationCanceledException>());
					Assert.That(async () => await tempFileName.DeserializeFromXmlFileAsync<Test>(token), Throws.InstanceOf<OperationCanceledException>());
					Assert.That(async () => await serializedString.DeserializeFromXmlStringAsync<Test>(token), Throws.InstanceOf<OperationCanceledException>());

					using (var xmlReader = new XmlTextReader(tempFileName))
					{
						Assert.That(async () => await xmlReader.DeserializeFromXmlStreamAsync<Test>(token), Throws.InstanceOf<OperationCanceledException>());
					}

					// the blocking wrappers report cancellation just as the asynchronous ones do
					Assert.That(() => test.SerializeToXmlFile(tempFileName, token), Throws.InstanceOf<OperationCanceledException>());
					Assert.That(() => tempFileName.DeserializeFromXmlFile<Test>(token), Throws.InstanceOf<OperationCanceledException>());
				}
			}
			finally
			{
				if (File.Exists(tempFileName))
				{
					File.Delete(tempFileName);
				}
			}
		}

		[Test]
		public void GivenAsyncSerializationToFile_WhenCompare_ThenWriteTheSameBytesAsXmlDocumentWould()
		{
			// arrange
			var test = Test.Create();
			string tempFileName = Path.GetTempFileName();
			string referenceFileName = Path.GetTempFileName();

			// act
			byte[] written, reference;
			try
			{
				test.SerializeToXmlFile(tempFileName);
				test.SerializeToXmlDocument().Save(referenceFileName);

				written = File.ReadAllBytes(tempFileName);
				reference = File.ReadAllBytes(referenceFileName);
			}
			finally
			{
				foreach (string fileName in new[] { tempFileName, referenceFileName })
				{
					if (File.Exists(fileName))
					{
						File.Delete(fileName);
					}
				}
			}

			// assert
			Assert.That(written, Is.EqualTo(reference));
		}

		[Test]
		public void GivenOverridenAttributes_WhenSerializeDeserialize_ThenSucceed()
		{
			// arrange
			var test = new SerializationParent
			{
				ChildrenA =
				{
					new SerializationChildA1(),
					new SerializationChildA2(),
					new SerializationChildA3(),
				},
				ChildrenB =
				{
					new SerializationChildB1(),
					new SerializationChildB2(),
					new SerializationChildB3(),
				},
			};

			var overrides = new[]
			{
				new XmlHelper.PropertyTypes(nameof(SerializationParent.ChildrenA), typeof(SerializationParent), new Dictionary<string, Type>
				{
					{ "A1", typeof(SerializationChildA1) },
					{ "A2", typeof(SerializationChildA2) },
					{ "A3", typeof(SerializationChildA3) },
				}),
				new XmlHelper.PropertyTypes(nameof(SerializationParent.ChildrenB), typeof(SerializationParent), new Dictionary<string, Type>
				{
					{ "B1", typeof(SerializationChildB1) },
					{ "B2", typeof(SerializationChildB2) },
					{ "B3", typeof(SerializationChildB3) },
				}),
			};

			var error = Assert.Throws<InvalidOperationException>(() => test.SerializeToXmlDocument());
			var innerError = (InvalidOperationException) error.InnerException;
			Assert.That(innerError.Message.Contains("XmlInclude"), Is.True);

			// act as extension & assert
			typeof(SerializationParent).DefineTypeOverrides(overrides);

			string xml = test.SerializeToXmlString();
			var deserialized = xml.DeserializeFromXmlString<SerializationParent>();

			Assert.That(deserialized.Equals(test), Is.True);

			// clear & try to assert again
			XmlHelper.DefineCustomXmlSerializer<SerializationParent>(new XmlSerializer(typeof(SerializationParent)));

			error = Assert.Throws<InvalidOperationException>(() => test.SerializeToXmlDocument());
			innerError = (InvalidOperationException)error.InnerException;
			Assert.That(innerError.Message.Contains("XmlInclude"), Is.True);

			// act and assert by type
			XmlHelper.DefineTypeOverrides<SerializationParent>(overrides);

			xml = test.SerializeToXmlString();
			deserialized = xml.DeserializeFromXmlString<SerializationParent>();

			Assert.That(deserialized.Equals(test), Is.True);
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
						(SingleChildObject == null && other.SingleChildObject == null || SingleChildObject != null && other.SingleChildObject != null && SingleChildObject.Equals(other.SingleChildObject)) &&
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

		[XmlType]
		public class SerializationParent : IEquatable<SerializationParent>
		{
			[XmlArray(nameof(ChildrenA))]
			public List<SerializationChildA> ChildrenA
			{ get; set; } = new List<SerializationChildA>();

			[XmlArray(nameof(ChildrenB))]
			public List<SerializationChildB> ChildrenB
			{ get; set; } = new List<SerializationChildB>();

			public bool Equals(SerializationParent other)
			{
				return ChildrenA.SequenceEqual(other.ChildrenA) && ChildrenB.SequenceEqual(other.ChildrenB);
			}

			public override bool Equals(object obj)
			{
				return Equals(obj as SerializationParent);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		[XmlType]
		public abstract class SerializationChildA : IEquatable<SerializationChildA>
		{
			public bool Equals(SerializationChildA other)
			{
				return GetType() == other.GetType();
			}

			public override bool Equals(object obj)
			{
				return Equals(obj as SerializationChildA);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		[XmlType]
		public class SerializationChildA1 : SerializationChildA
		{ }

		[XmlType]
		public class SerializationChildA2 : SerializationChildA
		{ }

		[XmlType]
		public class SerializationChildA3 : SerializationChildA
		{ }

		[XmlType]
		public abstract class SerializationChildB : IEquatable<SerializationChildB>
		{
			public bool Equals(SerializationChildB other)
			{
				return GetType() == other.GetType();
			}

			public override bool Equals(object obj)
			{
				return Equals(obj as SerializationChildB);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		[XmlType]
		public class SerializationChildB1 : SerializationChildB
		{ }

		[XmlType]
		public class SerializationChildB2 : SerializationChildB
		{ }

		[XmlType]
		public class SerializationChildB3 : SerializationChildB
		{ }

		#endregion
	}
}
