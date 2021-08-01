﻿using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class DescribeSubjectAreaQuestionTest
	{
		[Test]
		public void SubjectAreaWithoutConceptsHasNoConcepts()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);
			var noSubjectAreaConcept = SystemConcepts.GetAll().First();

			// act
			var question = new DescribeSubjectAreaQuestion(noSubjectAreaConcept);
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void SubjectAreaWithConceptCanFindIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var area = LogicalValues.True;
			var concept = LogicalValues.False;
			var groupStatement = new GroupStatement(area, concept);
			semanticNetwork.SemanticNetwork.Statements.Add(groupStatement);

			// act
			var question = new DescribeSubjectAreaQuestion(area);
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);

			Assert.AreSame(groupStatement, answer.Explanation.Statements.Single());

			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.AreEqual(concept, conceptsAnswer.Result.Single());
		}

		[Test]
		public void MultipleConcepts()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var question = new DescribeSubjectAreaQuestion(semanticNetwork.SubjectArea_Transport);
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);

			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.Greater(conceptsAnswer.Result.Count, 0);

			var groupStatements = answer.Explanation.Statements.OfType<GroupStatement>().ToList();
			Assert.AreEqual(answer.Explanation.Statements.Count, groupStatements.Count);

			Assert.IsTrue(groupStatements.All(statement => statement.Area == semanticNetwork.SubjectArea_Transport));
			Assert.AreEqual(groupStatements.Count, conceptsAnswer.Result.Count);
			Assert.AreEqual(groupStatements.Count, groupStatements.Select(statement => statement.Concept).Intersect(conceptsAnswer.Result).Count());
		}
	}
}
