using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class GetCommonQuestionTest
	{
		[Test]
		public void ReturnEmptyIfConceptsCanNotBeCompared()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.AreaType_Air, semanticNetwork.MotorType_Jet);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnEmptyWithExplanationIfNoCommonFound()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_JetFighter);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}

		[Test]
		public void ReturnAllCommon()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatInCommon(semanticNetwork.Vehicle_Steamboat, semanticNetwork.Vehicle_SteamLocomotive);

			// assert
			Assert.IsFalse(answer.IsEmpty);

			Assert.AreSame(semanticNetwork.Sign_MotorType, (((ConceptsAnswer) answer).Result).Single());

			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}

		[Test]
		public void ReturnAllCommonManyLevels()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			CreateCompareConceptsTest(semanticNetwork);

			// act
			var answer = semanticNetwork.Ask().WhatInCommon(
				semanticNetwork.Concepts.First(c => c.HasAttribute<IsProcessAttribute>()),
				semanticNetwork.Concepts.Last(c => c.HasAttribute<IsProcessAttribute>()));

			// assert
			Assert.IsFalse(answer.IsEmpty);

			var signs = (((ConceptsAnswer) answer).Result).Select(c => c.Name.GetValue(language)).ToList();
			Assert.AreEqual(2, signs.Count);
			Assert.IsTrue(signs.Contains(SignSameValues));
			Assert.IsTrue(signs.Contains(SignBothNotSet));

			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}

		internal const string SignSameValues = "Same values";
		internal const string SignBothNotSet = "Both not set";
		internal const string SignDifferentValues = "Different values";
		internal const string SignFirstNotSet = "First not set";
		internal const string SignSecondNotSet = "Second not set";
		internal const string SignPseudoCommon = "Pseudo common";
		internal const string SignPseudoDifference = "Pseudo difference";

		internal static void CreateCompareConceptsTest(ISemanticNetwork semanticNetwork)
		{
			var parent = new Concept(new LocalizedStringConstant(l => "Parent 1"));
			var parentOfParent = new Concept(new LocalizedStringConstant(l => "Parent 2"));
			var differentParent = new Concept(new LocalizedStringConstant(l => "Different Parent"));
			var concept1 = new Concept(new LocalizedStringConstant(l => "Concept 1"));
			var concept2 = new Concept(new LocalizedStringConstant(l => "Concept 2"));
			concept1.Attributes.Add(IsProcessAttribute.Value);
			concept2.Attributes.Add(IsProcessAttribute.Value);
			semanticNetwork.Concepts.Add(parent);
			semanticNetwork.Concepts.Add(parentOfParent);
			semanticNetwork.Concepts.Add(differentParent);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);

			semanticNetwork.DeclareThat(parent).IsAncestorOf(concept1);
			semanticNetwork.DeclareThat(parent).IsAncestorOf(concept2);
			semanticNetwork.DeclareThat(parentOfParent).IsAncestorOf(parent);
			semanticNetwork.DeclareThat(differentParent).IsAncestorOf(concept1);
			semanticNetwork.DeclareThat(differentParent).IsAncestorOf(concept2);

			var signSameValues = new Concept(new LocalizedStringConstant(l => SignSameValues));
			var signBothNotSet = new Concept(new LocalizedStringConstant(l => SignBothNotSet));
			var signDifferentValues = new Concept(new LocalizedStringConstant(l => SignDifferentValues));
			var signFirstNotSet = new Concept(new LocalizedStringConstant(l => SignFirstNotSet));
			var signSecondNotSet = new Concept(new LocalizedStringConstant(l => SignSecondNotSet));
			var signPseudoCommon = new Concept(new LocalizedStringConstant(l => SignPseudoCommon));
			var signPseudoDifference = new Concept(new LocalizedStringConstant(l => SignPseudoDifference));
			foreach (var sign in new[]
			{
				signSameValues,
				signBothNotSet,
				signDifferentValues,
				signFirstNotSet,
				signSecondNotSet,
				signPseudoCommon,
				signPseudoDifference,
			})
			{
				sign.Attributes.Add(IsSignAttribute.Value);
			}
			semanticNetwork.Concepts.Add(signSameValues);
			semanticNetwork.Concepts.Add(signBothNotSet);
			semanticNetwork.Concepts.Add(signDifferentValues);
			semanticNetwork.Concepts.Add(signFirstNotSet);
			semanticNetwork.Concepts.Add(signSecondNotSet);
			semanticNetwork.Concepts.Add(signPseudoCommon);
			semanticNetwork.Concepts.Add(signPseudoDifference);

			semanticNetwork.DeclareThat(parent).HasSign(signSameValues);
			semanticNetwork.DeclareThat(parent).HasSign(signBothNotSet);
			semanticNetwork.DeclareThat(parentOfParent).HasSign(signDifferentValues);
			semanticNetwork.DeclareThat(differentParent).HasSign(signFirstNotSet);
			semanticNetwork.DeclareThat(differentParent).HasSign(signSecondNotSet);

			semanticNetwork.DeclareThat(parent).HasSignValue(signSameValues, LogicalValues.True);
			semanticNetwork.DeclareThat(concept1).HasSignValue(signDifferentValues, LogicalValues.True);
			semanticNetwork.DeclareThat(concept2).HasSignValue(signDifferentValues, LogicalValues.False);
			semanticNetwork.DeclareThat(concept2).HasSignValue(signFirstNotSet, LogicalValues.True);
			semanticNetwork.DeclareThat(concept1).HasSignValue(signSecondNotSet, LogicalValues.True);
		}
	}
}
