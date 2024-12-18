using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class StatementBuilderTest
	{
		[Test]
		public void GivenIsStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var ancestor = ConceptCreationHelper.CreateEmptyConcept();
			var descendant = ConceptCreationHelper.CreateEmptyConcept();

			// act
			var statementByConstructor = new IsStatement(null, ancestor, descendant);
			var statementByBuilderFromAncestor = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(descendant);
			var statementByBuilderFromDescendant = semanticNetwork.DeclareThat(descendant).IsDescendantOf(ancestor);

			// assert
			Assert.That(statementByBuilderFromAncestor, Is.EqualTo(statementByConstructor));
			Assert.That(statementByBuilderFromDescendant, Is.EqualTo(statementByConstructor));
		}

		[Test]
		public void GivenMultipleIsStatements_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var ancestor = ConceptCreationHelper.CreateEmptyConcept();
			var ancestor1 = ConceptCreationHelper.CreateEmptyConcept();
			var ancestor2 = ConceptCreationHelper.CreateEmptyConcept();
			var ancestor3 = ConceptCreationHelper.CreateEmptyConcept();
			var descendant = ConceptCreationHelper.CreateEmptyConcept();
			var descendant1 = ConceptCreationHelper.CreateEmptyConcept();
			var descendant2 = ConceptCreationHelper.CreateEmptyConcept();
			var descendant3 = ConceptCreationHelper.CreateEmptyConcept();

			// act
			var statementsByBuilderFromAncestor = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(new[] { descendant1, descendant2, descendant3 });
			var statementsByBuilderFromDescendant = semanticNetwork.DeclareThat(descendant).IsDescendantOf(new[] { ancestor1, ancestor2, ancestor3 });

			// assert
			Assert.That(semanticNetwork.Statements.Count, Is.EqualTo(6));
			Assert.That(semanticNetwork.Statements.All(s => s is IsStatement), Is.True);
			Assert.That(statementsByBuilderFromAncestor.Count, Is.EqualTo(3));
			Assert.That(statementsByBuilderFromAncestor.All(s => s.Ancestor == ancestor), Is.True);
			Assert.That(statementsByBuilderFromDescendant.Count, Is.EqualTo(3));
			Assert.That(statementsByBuilderFromDescendant.All(s => s.Descendant == descendant), Is.True);
		}

		[Test]
		public void GivenNoSemanticNetwork_WhenTryToCreateStatementBuilder_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementBuilder(null, 1.CreateConceptByObject()));
		}

		[Test]
		public void GivenNoConcept_WhenTryToCreateStatementBuilder_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementBuilder(new SemanticNetwork(Language.Default), null));
		}
	}
}
