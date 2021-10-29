using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Mathematics;
using Inventor.Semantics.Mathematics.Attributes;
using Inventor.Semantics.Processes;
using Inventor.Semantics.Processes.Attributes;
using Inventor.Semantics.Set;
using Inventor.Semantics.Set.Attributes;

namespace Inventor.Semantics.Test.Xml
{
	[TestFixture]
	public class AttributesTest
	{
		[TestFixtureSetUp]
		public void InitializeModules()
		{
			new Semantics.Modules.BooleanModule().RegisterMetadata();
			new Semantics.Modules.ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
		}

		[Test]
		[TestCaseSource(nameof(getAllAttributes))]
		public void OneAttribute(IAttribute attribute)
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(attribute);

			// act
			var xml = new Semantics.Xml.Concept(concept);
			var restored = xml.Load();

			// assert
			Assert.AreSame(attribute, restored.Attributes.Single());
		}

		[Test]
		public void AllAttributes()
		{
			// arrange
			var concept = new Concept();
			foreach (var attribute in getAllAttributes())
			{
				concept.WithAttribute(attribute);
			}

			// act
			var xml = new Semantics.Xml.Concept(concept);
			var restored = xml.Load();

			// assert
			Assert.IsTrue(restored.Attributes.SequenceEqual(concept.Attributes));
		}

		[Test]
		public void UnknownAttribute()
		{
			// arrange
			var concept = new Concept();
			concept.WithAttribute(new WrongAttribute());

			// act & assert
			Assert.Throws<NotSupportedException>(() => new Semantics.Xml.Concept(concept));
		}

		private IEnumerable<IAttribute> getAllAttributes()
		{
			yield return IsComparisonSignAttribute.Value;
			yield return Semantics.Attributes.IsBooleanAttribute.Value;
			yield return IsProcessAttribute.Value;
			yield return IsSequenceSignAttribute.Value;
			yield return IsSignAttribute.Value;
			yield return Semantics.Attributes.IsValueAttribute.Value;
		}

		private class WrongAttribute : IAttribute
		{ }
	}
}
