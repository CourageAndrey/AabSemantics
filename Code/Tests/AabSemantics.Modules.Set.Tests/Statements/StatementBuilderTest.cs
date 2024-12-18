using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Statements
{
	[TestFixture]
	public class StatementBuilderTest
	{
		[Test]
		public void GivenHasPartStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var whole = ConceptCreationHelper.CreateEmptyConcept();
			var part = ConceptCreationHelper.CreateEmptyConcept();

			// act
			var statementByConstructor = new HasPartStatement(null, whole, part);
			var statementByBuilderFromWhole = semanticNetwork.DeclareThat(whole).HasPart(part);
			var statementByBuilderFromPart = semanticNetwork.DeclareThat(part).IsPartOf(whole);

			// assert
			Assert.That(statementByBuilderFromWhole, Is.EqualTo(statementByConstructor));
			Assert.That(statementByBuilderFromPart, Is.EqualTo(statementByConstructor));
		}

		[Test]
		public void GivenMultipleHasPartStatements_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var whole1 = ConceptCreationHelper.CreateEmptyConcept();
			var whole2 = ConceptCreationHelper.CreateEmptyConcept();
			var part1 = ConceptCreationHelper.CreateEmptyConcept();
			var part2 = ConceptCreationHelper.CreateEmptyConcept();

			// act
			var statementsByConstructorFromWhole = new List<HasPartStatement>
			{
				new HasPartStatement(null, whole1, part1),
				new HasPartStatement(null, whole1, part2),
			};
			var statementsByConstructorFromPart = new List<HasPartStatement>
			{
				new HasPartStatement(null, whole1, part1),
				new HasPartStatement(null, whole2, part1),
			};
			var statementsByBuilderFromWhole = semanticNetwork.DeclareThat(whole1).HasParts(new[] { part1, part2 });
			var statementsByBuilderFromPart = semanticNetwork.DeclareThat(part1).IsPartOf(new[] { whole1, whole2 });

			// assert
			AssertAreEqual(statementsByConstructorFromWhole, statementsByBuilderFromWhole);
			AssertAreEqual(statementsByConstructorFromPart, statementsByBuilderFromPart);
		}

		[Test]
		public void GivenGroupStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var area = ConceptCreationHelper.CreateEmptyConcept();
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act
			var statementByConstructor = new GroupStatement(null, area, concept);
			var statementByBuilderFromArea = semanticNetwork.DeclareThat(area).IsSubjectAreaOf(concept);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).BelongsToSubjectArea(area);

			// assert
			Assert.That(statementByBuilderFromArea, Is.EqualTo(statementByConstructor));
			Assert.That(statementByBuilderFromConcept, Is.EqualTo(statementByConstructor));
		}

		[Test]
		public void GivenMultipleGroupStatements_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var area1 = ConceptCreationHelper.CreateEmptyConcept();
			var area2 = ConceptCreationHelper.CreateEmptyConcept();
			var concept1 = ConceptCreationHelper.CreateEmptyConcept();
			var concept2 = ConceptCreationHelper.CreateEmptyConcept();

			// act
			var statementsByConstructorFromArea = new List<GroupStatement>
			{
				new GroupStatement(null, area1, concept1),
				new GroupStatement(null, area1, concept2),
			};
			var statementsByConstructorFromConcept = new List<GroupStatement>
			{
				new GroupStatement(null, area1, concept1),
				new GroupStatement(null, area2, concept1),
			};
			var statementsByBuilderFromArea = semanticNetwork.DeclareThat(area1).IsSubjectAreaOf(new[] { concept1, concept2 });
			var statementsByBuilderFromConcept = semanticNetwork.DeclareThat(concept1).BelongsToSubjectAreas(new[] { area1, area2 });

			// assert
			AssertAreEqual(statementsByConstructorFromArea, statementsByBuilderFromArea);
			AssertAreEqual(statementsByConstructorFromConcept, statementsByBuilderFromConcept);
		}

		[Test]
		public void GivenHasSignStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			// act
			var statementByConstructor = new HasSignStatement(null, concept, sign);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).HasSign(sign);
			var statementByBuilderFromSign = semanticNetwork.DeclareThat(sign).IsSignOf(concept);

			// assert
			Assert.That(statementByBuilderFromConcept, Is.EqualTo(statementByConstructor));
			Assert.That(statementByBuilderFromSign, Is.EqualTo(statementByConstructor));
		}

		[Test]
		public void GivenMultipleHasSignStatements_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept1 = ConceptCreationHelper.CreateEmptyConcept();
			var concept2 = ConceptCreationHelper.CreateEmptyConcept();
			var sign1 = ConceptCreationHelper.CreateEmptyConcept();
			sign1.WithAttribute(IsSignAttribute.Value);
			var sign2 = ConceptCreationHelper.CreateEmptyConcept();
			sign2.WithAttribute(IsSignAttribute.Value);

			// act
			var statementsByConstructorFromConcept = new List<HasSignStatement>
			{
				new HasSignStatement(null, concept1, sign1),
				new HasSignStatement(null, concept1, sign2),
			};
			var statementsByConstructorFromSign = new List<HasSignStatement>
			{
				new HasSignStatement(null, concept1, sign1),
				new HasSignStatement(null, concept2, sign1),
			};
			var statementsByBuilderFromConcept = semanticNetwork.DeclareThat(concept1).HasSigns(new[] { sign1, sign2 });
			var statementsByBuilderFromSign = semanticNetwork.DeclareThat(sign1).IsSignOf(new[] { concept1, concept2 });

			// assert
			AssertAreEqual(statementsByConstructorFromConcept, statementsByBuilderFromConcept);
			AssertAreEqual(statementsByConstructorFromSign, statementsByBuilderFromSign);
		}

		[Test]
		public void GivenSignValueStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act
			var statementByConstructor = new SignValueStatement(null, concept, sign, value);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).HasSignValue(sign, value);
			var statementByBuilderFromValue = semanticNetwork.DeclareThat(value).IsSignValue(concept, sign);

			// assert
			Assert.That(statementByBuilderFromConcept, Is.EqualTo(statementByConstructor));
			Assert.That(statementByBuilderFromValue, Is.EqualTo(statementByConstructor));
		}

		private static void AssertAreEqual<T>(ICollection<T> sequence1, ICollection<T> sequence2)
			where T : IEquatable<T>
		{
			Assert.That(sequence1.Count, Is.EqualTo(sequence2.Count));
			foreach (var item in sequence1)
			{
				Assert.That(sequence2.Contains(item), Is.True);
			}
		}
	}
}
