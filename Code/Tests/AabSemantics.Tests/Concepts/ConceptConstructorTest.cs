using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;

namespace AabSemantics.Tests.Concepts
{
	[TestFixture]
	public class ConceptConstructorTest
	{
		private const string TestConceptId = "123";

		[Test]
		public void GivenAllPropertiesSet_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.That(concept.ID, Is.EqualTo(TestConceptId));
		}

		[Test]
		public void GivenNoId_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(null, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.That(string.IsNullOrEmpty(concept.ID), Is.False);
		}

		[Test]
		public void GivenNoName_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, null, new LocalizedStringVariable());

			// assert
			Assert.That(concept.Name, Is.Not.Null);
		}

		[Test]
		public void GivenNoHint_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = new Concept(TestConceptId, new LocalizedStringVariable(), null);

			// assert
			Assert.That(concept.Hint, Is.Not.Null);
		}

		[Test]
		public void GivenDifferentIdAndName_WhenCreateConcept_ThenSucceed()
		{
			// arrange && act
			var concept = TestConceptId.CreateConceptById(new string(TestConceptId.Reverse().ToArray()));

			// assert
			Assert.That(concept.Name.GetValue(Language.Default), Is.Not.EqualTo(concept.ID));
		}
	}
}
