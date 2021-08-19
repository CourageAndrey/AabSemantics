using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class QuestionBuilderTest
	{
		[Test]
		public void TestBuildingIsTrueThat()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var checkedStatement = semanticNetwork.SemanticNetwork.Statements.First();

			// act
			var questionRegular = new CheckStatementQuestion(checkedStatement);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IsTrueThat(checkedStatement);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingHowCompared()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new ComparisonQuestion(semanticNetwork.Number0, semanticNetwork.Number2);
			var answerRegular = (StatementAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (StatementAnswer) semanticNetwork.SemanticNetwork.Ask().HowCompared(semanticNetwork.Number0, semanticNetwork.Number2);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhichConceptsBelongToSubjectArea()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new DescribeSubjectAreaQuestion(semanticNetwork.SubjectArea_Transport);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(semanticNetwork.SubjectArea_Transport);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhichDescendantsHas()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new EnumerateChildrenQuestion(semanticNetwork.Base_Vehicle);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichDescendantsHas(semanticNetwork.Base_Vehicle);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhichContainersInclude()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new EnumerateContainersQuestion(semanticNetwork.Part_Engine);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichContainersInclude(semanticNetwork.Part_Engine);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhichPartsHas()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new EnumeratePartsQuestion(semanticNetwork.Vehicle_Car);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichPartsHas(semanticNetwork.Vehicle_Car);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhichSignsHas()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Car, true);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichSignsHas(semanticNetwork.Vehicle_Car);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingToWhichSubjectAreasBelongs()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new FindSubjectAreaQuestion(semanticNetwork.Vehicle_Car);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().ToWhichSubjectAreasBelongs(semanticNetwork.Vehicle_Car);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingIfHasSign()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new HasSignQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType, true);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfHasSign(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingIfHasSigns()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new HasSignsQuestion(semanticNetwork.Vehicle_Car, true);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfHasSigns(semanticNetwork.Vehicle_Car);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingIfIsPartOf()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new IsPartOfQuestion(semanticNetwork.Part_Engine, semanticNetwork.Vehicle_Car);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Vehicle_Car);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingIfIs()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new IsQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Base_Vehicle);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfIs(semanticNetwork.Vehicle_Car, semanticNetwork.Base_Vehicle);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingIfIsSign()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new IsSignQuestion(semanticNetwork.Sign_AreaType);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfIsSign(semanticNetwork.Sign_AreaType);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingIfConceptBelongsToSubjectArea()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new IsSubjectAreaQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.SubjectArea_Transport);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Vehicle_Car, semanticNetwork.SubjectArea_Transport);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingIfIsValue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new IsValueQuestion(semanticNetwork.AreaType_Air);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfIsValue(semanticNetwork.AreaType_Air);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhatIsMutualSequenceOfProcesses()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var processA = new Concept();
			processA.Attributes.Add(IsProcessAttribute.Value);
			var processB = new Concept();
			processB.Attributes.Add(IsProcessAttribute.Value);
			semanticNetwork.SemanticNetwork.DeclareThat(processA).StartsBeforeOtherStarted(processB);
			semanticNetwork.SemanticNetwork.DeclareThat(processA).FinishesAfterOtherFinished(processB);

			// act
			var questionRegular = new ProcessesQuestion(processA, processB);
			var answerRegular = (StatementsAnswer<ProcessesStatement>) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (StatementsAnswer<ProcessesStatement>) semanticNetwork.SemanticNetwork.Ask().WhatIsMutualSequenceOfProcesses(processA, processB);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhatIsSignValue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new SignValueQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType);
			var answerRegular = (ConceptAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptAnswer) semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType);

			// assert
			Assert.AreSame(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingWhatIs()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var questionRegular = new WhatQuestion(semanticNetwork.Vehicle_Car);
			var answerRegular = questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = semanticNetwork.SemanticNetwork.Ask().WhatIs(semanticNetwork.Vehicle_Car);

			// assert
			Assert.AreEqual(answerRegular.Description.GetPlainText(language).ToString(), answerBuilder.Description.GetPlainText(language).ToString());
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void TestBuildingPreconditions()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var numberMinus1 = new Concept();
			numberMinus1.Attributes.Add(IsValueAttribute.Value);

			var precondition = new ComparisonStatement(numberMinus1, semanticNetwork.Number0, ComparisonSigns.IsLessThan);
			var preconditions = new IStatement[] { precondition };

			// act
			var questionRegular = new ComparisonQuestion(numberMinus1, semanticNetwork.Number1, preconditions);
			var answerRegular = (StatementAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (StatementAnswer) semanticNetwork.SemanticNetwork.Supposing(preconditions).Ask().HowCompared(numberMinus1, semanticNetwork.Number1);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}
	}
}
