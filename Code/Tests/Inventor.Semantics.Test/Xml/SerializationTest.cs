using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Xml;
using Inventor.Semantics.Test.Sample;

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
	}
}
