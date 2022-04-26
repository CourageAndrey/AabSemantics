using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Xml;
using Inventor.Semantics.Test.Sample;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Test.Xml
{
	[TestFixture]
	public class SerializationTest
	{
		[Test]
		public void CheckLargeSemanticNetworkSerialization()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;
			var name = (LocalizedStringVariable) semanticNetwork.Name;
			var locales = name.Locales;
			string testFileName = Path.GetTempFileName();

			// act
			Semantics.SemanticNetwork restored;
			try
			{
				semanticNetwork.Save(testFileName);
				restored = testFileName.LoadSemanticNetworkFromXml(language);
			}
			finally
			{
				if (File.Exists(testFileName))
				{
					File.Delete(testFileName);
				}
			}

			// assert
			var restoredName = (LocalizedStringVariable) restored.Name;
			Assert.IsTrue(locales.SequenceEqual(restoredName.Locales));
			foreach (string locale in locales)
			{
				Assert.AreEqual(name.GetValue(locale), restoredName.GetValue(locale));
			}

			var conceptMapping = new Dictionary<IConcept, IConcept>();
			Assert.AreEqual(semanticNetwork.Concepts.Count, restored.Concepts.Count);
			foreach (var concept in semanticNetwork.Concepts/*.Except(systemConcepts)*/)
			{
				var restoredConcept = restored.Concepts.Single(c =>
					c.Name.GetValue(language) == concept.Name.GetValue(language) &&
					c.Hint.GetValue(language) == concept.Hint.GetValue(language));
				conceptMapping[concept] = restoredConcept;
			}

			var systemConcepts = new HashSet<IConcept>(SystemConcepts.GetAll());
			foreach (var mapping in conceptMapping)
			{
				if (systemConcepts.Contains(mapping.Key))
				{
					Assert.AreSame(mapping.Key, mapping.Value);
				}
				else
				{
					Assert.AreNotSame(mapping.Key, mapping.Value);
				}
			}

			Assert.AreEqual(semanticNetwork.Statements.Count, restored.Statements.Count);
			foreach (var statement in semanticNetwork.Statements)
			{
				var statementType = statement.GetType();
				var childConcepts = statement.GetChildConcepts().Select(c => conceptMapping[c]).ToList();

				// Note: check method Single() below means, that test semantic network can not contain statement duplicates.
				restored.Statements.Single(s => statementType == s.GetType() && childConcepts.SequenceEqual(s.GetChildConcepts()));
			}
		}

		[Test]
		public void CheckCustomSerializers()
		{
			// arrange
			var customSerializer = new XmlSerializer(typeof(SerializableCustom));

			// act
			customSerializer.DefineCustomSerializer<SerializableCustom>();
			var acquiredSerializer = typeof(SerializableCustom).AcquireSerializer();

			// assert
			Assert.AreSame(customSerializer, acquiredSerializer);
		}

		[Test]
		public void CheckMultiThreading()
		{
			// arrange
			var types = new[]
			{
				typeof(Serializable1),
				typeof(Serializable2),
				typeof(Serializable3),
				typeof(Serializable4),
			};

			const int attemptsPerType = 10;
			var threads = new List<Thread>();
			for (int t = 0; t < attemptsPerType; t++)
			{
				foreach (var type in types)
				{
					threads.Add(new Thread(() =>
					{
						type.AcquireSerializer();
					}));
				}
			}

			// act
			foreach (var thread in threads)
			{
				thread.Start();
			}

			while (!threads.TrueForAll(t => t.ThreadState == ThreadState.Stopped))
			{
				Thread.Sleep(100);
			}
		}
	}

	[Serializable, XmlRoot(nameof(SerializableCustom))]
	public class SerializableCustom
	{
		[XmlElement]
		public string FieldCustom
		{ get; set; }
	}

	[Serializable, XmlRoot(nameof(Serializable1))]
	public class Serializable1
	{
		[XmlElement]
		public string Field1
		{ get; set; }
	}

	[Serializable, XmlRoot(nameof(Serializable2))]
	public class Serializable2
	{
		[XmlElement]
		public string Field2
		{ get; set; }
	}

	[Serializable, XmlRoot(nameof(Serializable3))]
	public class Serializable3
	{
		[XmlElement]
		public string Field3
		{ get; set; }
	}

	[Serializable, XmlRoot(nameof(Serializable4))]
	public class Serializable4
	{
		[XmlElement]
		public string Field4
		{ get; set; }
	}
}
