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

			var ancestor = ConceptCreationHelper.CreateConcept();
			var descendant = ConceptCreationHelper.CreateConcept();

			// act
			var statementByConstructor = new IsStatement(null, ancestor, descendant);
			var statementByBuilderFromAncestor = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(descendant);
			var statementByBuilderFromDescendant = semanticNetwork.DeclareThat(descendant).IsDescendantOf(ancestor);

			// assert
			Assert.AreEqual(statementByConstructor, statementByBuilderFromAncestor);
			Assert.AreEqual(statementByConstructor, statementByBuilderFromDescendant);
		}

		[Test]
		public void GivenMultipleIsStatements_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var ancestor = ConceptCreationHelper.CreateConcept();
			var ancestor1 = ConceptCreationHelper.CreateConcept();
			var ancestor2 = ConceptCreationHelper.CreateConcept();
			var ancestor3 = ConceptCreationHelper.CreateConcept();
			var descendant = ConceptCreationHelper.CreateConcept();
			var descendant1 = ConceptCreationHelper.CreateConcept();
			var descendant2 = ConceptCreationHelper.CreateConcept();
			var descendant3 = ConceptCreationHelper.CreateConcept();

			// act
			var statementsByBuilderFromAncestor = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(new[] { descendant1, descendant2, descendant3 });
			var statementsByBuilderFromDescendant = semanticNetwork.DeclareThat(descendant).IsDescendantOf(new[] { ancestor1, ancestor2, ancestor3 });

			// assert
			Assert.AreEqual(6, semanticNetwork.Statements.Count);
			Assert.IsTrue(semanticNetwork.Statements.All(s => s is IsStatement));
			Assert.AreEqual(3, statementsByBuilderFromAncestor.Count);
			Assert.IsTrue(statementsByBuilderFromAncestor.All(s => s.Ancestor == ancestor));
			Assert.AreEqual(3, statementsByBuilderFromDescendant.Count);
			Assert.IsTrue(statementsByBuilderFromDescendant.All(s => s.Descendant == descendant));
		}

		[Test]
		public void GivenNoSemanticNetwork_WhenTryToCreateStatementBuilder_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementBuilder(null, 1.CreateConcept()));
		}

		[Test]
		public void GivenNoConcept_WhenTryToCreateStatementBuilder_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementBuilder(new SemanticNetwork(Language.Default), null));
		}
	}
}
