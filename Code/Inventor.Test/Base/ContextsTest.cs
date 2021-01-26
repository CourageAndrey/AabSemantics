using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Test.Base
{
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

		private class TestQuestion : IQuestion
		{ }

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
