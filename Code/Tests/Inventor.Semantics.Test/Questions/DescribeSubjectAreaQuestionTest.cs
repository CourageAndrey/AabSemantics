using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Boolean.Concepts;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Modules.Set.Questions;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Questions
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
			var answer = semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(noSubjectAreaConcept);

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
			var groupStatement = semanticNetwork.SemanticNetwork.DeclareThat(concept).BelongsToSubjectArea(area);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(area);

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
			var answer = semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(semanticNetwork.SubjectArea_Transport);

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
