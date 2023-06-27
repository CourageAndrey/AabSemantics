using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Set;
using AabSemantics.Modules.Set.Attributes;

namespace AabSemantics.Tests.Serialization.Xml
{
	[TestFixture]
	public class AttributesTest
	{
		[OneTimeSetUp]
		public void InitializeModules()
		{
			new Modules.Boolean.BooleanModule().RegisterMetadata();
			new Modules.Classification.ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
		}

		[Test]
		[TestCaseSource(nameof(GetAllAttributes))]
		public void GivenOneAttribute_WhenSerializeDeserialize_ThenSucceed(IAttribute attribute)
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(attribute);

			// act
			var xml = new AabSemantics.Serialization.Xml.Concept(concept);
			var restored = xml.Load();

			// assert
			Assert.AreSame(attribute, restored.Attributes.Single());
		}

		[Test]
		public void GivenAllAttributes_WhenSerializeDeserialize_ThenSucceed()
		{
			// arrange
			var concept = new Concept();
			foreach (var attribute in GetAllAttributes())
			{
				concept.WithAttribute(attribute);
			}

			// act
			var xml = new AabSemantics.Serialization.Xml.Concept(concept);
			var restored = xml.Load();

			// assert
			Assert.IsTrue(restored.Attributes.SequenceEqual(concept.Attributes));
		}

		[Test]
		public void GivenUnknownAttribute_WhenTryToSerialize_ThenFail()
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(new WrongAttribute());

			// act & assert
			Assert.Throws<NotSupportedException>(() => new AabSemantics.Serialization.Xml.Concept(concept));
		}

		private static IEnumerable<IAttribute> GetAllAttributes()
		{
			yield return IsComparisonSignAttribute.Value;
			yield return Modules.Boolean.Attributes.IsBooleanAttribute.Value;
			yield return IsProcessAttribute.Value;
			yield return IsSequenceSignAttribute.Value;
			yield return IsSignAttribute.Value;
			yield return Modules.Boolean.Attributes.IsValueAttribute.Value;
		}

		private class WrongAttribute : IAttribute
		{ }
	}
}
