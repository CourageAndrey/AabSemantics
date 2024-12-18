using System;
using System.Linq;
using AabSemantics.Concepts;
using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Questions;
using AabSemantics.Statements;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class WhatQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new WhatQuestion(null));
		}

		[Test]
		public void GivenWhatIs_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var questionRegular = new WhatQuestion(semanticNetwork.Vehicle_Car);
			var answerRegular = questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);
			var textRegular = render.RenderText(answerRegular.Description, language).ToString();

			var answerBuilder = semanticNetwork.SemanticNetwork.Ask().WhatIs(semanticNetwork.Vehicle_Car);
			var textBuilder = render.RenderText(answerBuilder.Description, language).ToString();

			// assert
			Assert.That(textBuilder, Is.EqualTo(textRegular));
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);

			Assert.That(textRegular.Contains("\" is \"") &&
						textRegular.Contains(" with following sign values (properties):") &&
						textRegular.Contains(" sign value is equal to "), Is.True);
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIs(LogicalValues.True);

			// assert
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenJustClassification_WhenBeingAsked_ThenAnswerShort()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<SetModule>();

			IConcept child, parent;
			semanticNetwork.Concepts.Add(child = "child".CreateConceptByName());
			semanticNetwork.Concepts.Add(parent = "parent".CreateConceptByName());
			semanticNetwork.DeclareThat(child).IsDescendantOf(parent);

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.Ask().WhatIs(child);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
			Assert.That(text.Contains(" is ") && text.Contains("."), Is.True);
		}
	}
}
