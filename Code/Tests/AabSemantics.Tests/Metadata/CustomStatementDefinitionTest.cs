using System;
using System.Collections.Generic;
using System.Linq;
using AabSemantics.Concepts;
using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Metadata
{
	[TestFixture]
	public class CustomStatementDefinitionTest
	{
		[SetUp]
		public void SetUp()
		{
			Repositories.Reset();
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void GivenNoKind_WhenTryToCreate_ThenFail(string kind)
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new CustomStatementDefinition(
				kind,
				new[] { "param" },
				language => null,
				language => null,
				language => null));
		}

		[Test]
		public void GivenCustomStatementDefinition_WhenCreate_ThenSaveNameAndKind()
		{
			// arrange
			const string kind = "kind";
			var concepts = new[] { "parameter" };

			// act
			var definition = new CustomStatementDefinition(
				kind,
				concepts,
				language => null,
				language => null,
				language => null);

			// assert
			Assert.That(definition.ToString(), Is.EqualTo(kind));
			Assert.That(definition.Name.GetValue(Language.Default), Is.EqualTo(kind));
			Assert.That(definition.Concepts.Single(), Is.EqualTo("parameter"));
		}

		[Test]
		public void GivenCustomStatementDefinitions_WhenRepositoriesCreates_ThenExist()
		{
			try
			{
				// arrange
				Repositories.RegisterCustomStatement(
					"custom",
					new[] { "x" },
					s => "test true",
					s => "test false",
					s => "test question");

				var language = Language.Default;
				var statement = new CustomStatement(null, "custom", new Dictionary<string, IConcept>{ { "x", "x".CreateConceptById() } });

				// act
				var statementDefinition = Repositories.Statements.Definitions.Single();
				var questionDefinition = Repositories.Questions.Definitions.Single();

				// assert
				Assert.That(statementDefinition.Key, Is.SameAs(typeof(CustomStatement)));
				Assert.That(statementDefinition.Value.Type, Is.SameAs(typeof(CustomStatement)));
				Assert.That(statementDefinition.Value.GetName(language), Is.EqualTo("Custom Statement"));
				Assert.That(() => statementDefinition.Value.DescribeTrue(statement).ToString(), Throws.InstanceOf<NotSupportedException>());
				Assert.That(() => statementDefinition.Value.DescribeFalse(statement).ToString(), Throws.InstanceOf<NotSupportedException>());
				Assert.That(() => statementDefinition.Value.DescribeQuestion(statement).ToString(), Throws.InstanceOf<NotSupportedException>());

				Assert.That(statement.Name.ToString(), Is.EqualTo("LOCALIZED_STRING \"Custom Statement\""));
				Assert.That(statement.DescribeTrue().ToString(), Is.EqualTo("test true (\"Custom Statement\")"));
				Assert.That(statement.DescribeFalse().ToString(), Is.EqualTo("test false"));
				Assert.That(statement.DescribeQuestion().ToString(), Is.EqualTo("test question"));

				Assert.That(questionDefinition.Key, Is.SameAs(typeof(CustomStatementQuestion)));
				Assert.That(questionDefinition.Value.Type, Is.SameAs(typeof(CustomStatementQuestion)));
				Assert.That(questionDefinition.Value.GetName(language), Is.EqualTo("CustomStatementQuestion"));
			}
			finally
			{
				Repositories.Reset();
			}
		}
	}
}
