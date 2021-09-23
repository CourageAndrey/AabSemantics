using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Concepts;
using Inventor.Mathematics;
using Inventor.Mathematics.Attributes;
using Inventor.Processes;
using Inventor.Processes.Attributes;
using Inventor.Set;
using Inventor.Set.Attributes;

namespace Inventor.Test.Xml
{
	[TestFixture]
	public class AttributesTest
	{
		[TestFixtureSetUp]
		public void InitializeModules()
		{
			new Core.Modules.BooleanModule().RegisterMetadata();
			new Core.Modules.ClassificationModule().RegisterMetadata();
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
			var xml = new Core.Xml.Concept(concept);
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
			var xml = new Core.Xml.Concept(concept);
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
			Assert.Throws<NotSupportedException>(() => new Core.Xml.Concept(concept));
		}

		private IEnumerable<IAttribute> getAllAttributes()
		{
			yield return IsComparisonSignAttribute.Value;
			yield return Core.Attributes.IsBooleanAttribute.Value;
			yield return IsProcessAttribute.Value;
			yield return IsSequenceSignAttribute.Value;
			yield return IsSignAttribute.Value;
			yield return Core.Attributes.IsValueAttribute.Value;
		}

		private class WrongAttribute : IAttribute
		{ }
	}
}
