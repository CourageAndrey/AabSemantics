using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Utils;

namespace Inventor.Test.Base
{
	[TestFixture]
	public class ContextsTest
	{
		[Test]
		public void UnfinishedContextDisposingFails()
		{
			bool createNestedContext = false;

			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);
			knowledgeBase.Context.QuestionRepository.DefineQuestion(new QuestionDefinition(
				typeof(TestQuestion),
				l => string.Empty,
				() => new TestQuestionProcessorCreateNestedContext(createNestedContext)));
			var question = new TestQuestion();

			Assert.DoesNotThrow(() =>
			{
				question.Ask(knowledgeBase.Context);
			});

			createNestedContext = true;
			Assert.Throws<InvalidOperationException>(() =>
			{
				question.Ask(knowledgeBase.Context);
			});
		}

		[Test]
		public void ContextDisposingRemovesRelatedKnowledge()
		{
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);
			knowledgeBase.Context.QuestionRepository.DefineQuestion(new QuestionDefinition(
				typeof(TestQuestion),
				l => string.Empty,
				() => new TestQuestionProcessorCreateContextKnowledge()));

			new TestQuestion().Ask(knowledgeBase.Context);

			Assert.IsFalse(knowledgeBase.Statements.Enumerate<TestStatement>().Any());
		}

		[Test]
		public void OnlySystemContextIsSystem()
		{
			var language = Language.Default;

			var knowledgeBase = new KnowledgeBase(language);
			var knowledgeBaseContext = (KnowledgeBaseContext) knowledgeBase.Context;
			Assert.IsFalse(knowledgeBaseContext.IsSystem);

			var systemContext = (SystemContext) knowledgeBaseContext.Parent;
			Assert.IsTrue(systemContext.IsSystem);

			var questionContext = knowledgeBaseContext.CreateQuestionContext(new TestQuestion());
			Assert.IsFalse(questionContext.IsSystem);
		}

		[Test]
		public void AccessQuestionContextQuestion()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);
			var question = new TestQuestion();

			// act
			var questionContext = knowledgeBase.Context.CreateQuestionContext(question);

			// assert
			var contextQuestion = questionContext.Question;
			var typedContextQuestion = ((IQuestionProcessingContext<TestQuestion>) questionContext).Question;
			Assert.AreSame(question, contextQuestion);
			Assert.AreSame(question, typedContextQuestion);
		}

		[Test]
		public void ImpossibleToInstantiateMoreThanOneKnowledgeBaseContextOutOfOneSystemContext()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);
			var systemContext = (SystemContext) knowledgeBase.Context.Parent;

			// act & assert
			Assert.Throws<InvalidOperationException>(() => systemContext.Instantiate(
				new TestKnowledgeBase(),
				new TestStatementRepository(),
				new TestQuestionRepository(),
				new TestAttributeRepository()));
		}

		[Test]
		public void ImpossibleToDisposeContextWithActiveChildren()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);
			var questionContext = knowledgeBase.Context.CreateQuestionContext(new TestQuestion());
			var child1Context = questionContext.CreateQuestionContext(new TestQuestion());
			var child2Context = questionContext.CreateQuestionContext(new TestQuestion());

			// act & assert
			Assert.Throws<InvalidOperationException>(() => questionContext.Dispose());

			child1Context.Dispose();
			Assert.Throws<InvalidOperationException>(() => questionContext.Dispose());

			child2Context.Dispose();
			Assert.DoesNotThrow(() => questionContext.Dispose());
		}

		[Test]
		public void ContextDisposalWorksOnlyOnce()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);
			var questionContext = knowledgeBase.Context.CreateQuestionContext(new TestQuestion());
			var childQuestionContext = knowledgeBase.Context.CreateQuestionContext(new TestQuestion());

			// act
			questionContext.Dispose();
			questionContext.Children.Add(childQuestionContext);

			// assert
			Assert.DoesNotThrow(() => questionContext.Dispose());
		}

		private class TestKnowledgeBase : IKnowledgeBase
		{
			public ILocalizedString Name
			{ get; set; }

			public event EventHandler Changed;

			public IKnowledgeBaseContext Context
			{ get; set; }

			public ICollection<IConcept> Concepts
			{ get; set; }

			public ICollection<IStatement> Statements
			{ get; set; }

			public event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;

			public event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;

			public event EventHandler<ItemEventArgs<IStatement>> StatementAdded;

			public event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;

			public void Save(string fileName)
			{
				throw new NotSupportedException();
			}
		}

		private class TestStatementRepository : IStatementRepository
		{
			public IDictionary<Type, StatementDefinition> StatementDefinitions
			{ get; set; }

			public void DefineStatement(StatementDefinition statementDefinition)
			{
				throw new NotSupportedException();
			}
		}

		private class TestQuestionRepository : IQuestionRepository
		{
			public IDictionary<Type, QuestionDefinition> QuestionDefinitions
			{ get; set; }

			public void DefineQuestion(QuestionDefinition questionDefinition)
			{
				throw new NotSupportedException();
			}
		}

		private class TestAttributeRepository : IAttributeRepository
		{
			public IDictionary<Type, AttributeDefinition> AttributeDefinitions
			{ get; set; }
		}

		private class TestQuestion : IQuestion
		{
			public ICollection<IStatement> Preconditions
			{ get; } = new IStatement[0];
		}

		private class TestStatement : IStatement
		{
			public ILocalizedString Name
			{ get; } = null;

			public IContext Context
			{ get; set; }

			public ILocalizedString Hint
			{ get; } = null;

			public IEnumerable<IConcept> GetChildConcepts()
			{
				return new IConcept[0];
			}

			public FormattedLine DescribeTrue(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeFalse(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeQuestion(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public Boolean CheckUnique(IEnumerable<IStatement> statements)
			{
				throw new NotSupportedException();
			}
		}

		private class TestQuestionProcessorCreateNestedContext : QuestionProcessor<TestQuestion>
		{
			private readonly bool _createNestedContext;

			public TestQuestionProcessorCreateNestedContext(bool createNestedContext)
			{
				_createNestedContext = createNestedContext;
			}

			public override IAnswer Process(IQuestionProcessingContext<TestQuestion> context)
			{
				if (_createNestedContext)
				{
					new QuestionProcessingContext<TestQuestion>(context, new TestQuestion());
				}

				return null;
			}
		}

		private class TestQuestionProcessorCreateContextKnowledge : QuestionProcessor<TestQuestion>
		{
			public override IAnswer Process(IQuestionProcessingContext<TestQuestion> context)
			{
				IStatement testStatement;
				context.KnowledgeBase.Statements.Add(testStatement = new TestStatement());
				testStatement.Context = context;
				context.Scope.Add(testStatement);

				Assert.IsTrue(context.KnowledgeBase.Statements.Enumerate<TestStatement>(context).Any());

				return null;
			}
		}
	}
}
