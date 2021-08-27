using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Utils;
using Inventor.Core.Questions;

namespace Inventor.Test.Base
{
	// This test checks Question processing contexts, base on default implementation of IQuestion.Ask() method.
	//
	//	public IAnswer Ask(ISemanticNetworkContext context)
	//	{
	//		using (var questionContext = context.CreateQuestionContext(this))
	//		{
	//			return Process(questionContext);
	//		}
	//	}
	[TestFixture]
	public class ContextsTest
	{
		[Test]
		public void EnsureThatImpossibleToDisposeContextWhichHasNotDisposedChildContexts()
		{
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			Assert.DoesNotThrow(() =>
			{
				// test context is created without children and can be successfully disposed
				new TestQuestionCreateNestedContext(false).Ask(semanticNetwork.Context);
			});

			Assert.Throws<InvalidOperationException>(() =>
			{
				// test context is created with unfinished children and fails to dispose
				new TestQuestionCreateNestedContext(true).Ask(semanticNetwork.Context);
			});
		}

		// This test partially duplicates previous one.
		[Test]
		public void ImpossibleToDisposeContextWithActiveChildren()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var questionContext = semanticNetwork.Context.CreateQuestionContext(new TestQuestionCreateContextKnowledge());
			var child1Context = questionContext.CreateQuestionContext(new TestQuestionCreateNestedContext(false));
			var child2Context = questionContext.CreateQuestionContext(new TestQuestionCreateNestedContext(false));

			// act & assert
			Assert.Throws<InvalidOperationException>(() => questionContext.Dispose());

			child1Context.Dispose();
			Assert.Throws<InvalidOperationException>(() => questionContext.Dispose());

			child2Context.Dispose();
			Assert.DoesNotThrow(() => questionContext.Dispose());
		}

		[Test]
		public void ContextDisposingRemovesStatementsAddedByIt()
		{
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			// this context adds to semantic network new test statement, attached to this context
			new TestQuestionCreateContextKnowledge().Ask(semanticNetwork.Context);

			// as context has been disposed after previous line, ensure, that added test stametement(s) was (were) also deleted
			Assert.IsFalse(semanticNetwork.Statements.Enumerate<TestStatement>().Any());
		}

		[Test]
		public void OnlySystemContextIsSystem()
		{
			var language = Language.Default;

			var semanticNetwork = new SemanticNetwork(language);
			var semanticNetworkContext = (SemanticNetworkContext) semanticNetwork.Context;
			Assert.IsFalse(semanticNetworkContext.IsSystem);

			var systemContext = (SystemContext) semanticNetworkContext.Parent;
			Assert.IsTrue(systemContext.IsSystem);

			var questionContext = semanticNetworkContext.CreateQuestionContext(new TestQuestionCreateContextKnowledge());
			Assert.IsFalse(questionContext.IsSystem);
		}

		[Test]
		public void AccessQuestionContextQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var question = new TestQuestionCreateContextKnowledge();

			// act
			var questionContext = semanticNetwork.Context.CreateQuestionContext(question);

			// assert
			var contextQuestion = questionContext.Question;
			var typedContextQuestion = ((IQuestionProcessingContext<TestQuestionCreateContextKnowledge>) questionContext).Question;
			Assert.AreSame(question, contextQuestion);
			Assert.AreSame(question, typedContextQuestion);
		}

		[Test]
		public void ImpossibleToInstantiateMoreThanOneSemanticNetworkContextOutOfOneSystemContext()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var systemContext = (SystemContext) semanticNetwork.Context.Parent;

			// act & assert
			Assert.Throws<InvalidOperationException>(() => systemContext.Instantiate(
				new TestSemanticNetwork(),
				new TestStatementRepository(),
				new TestQuestionRepository(),
				new TestAttributeRepository()));
		}

		[Test]
		public void ContextDisposalWorksOnlyOnce()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var questionContext = semanticNetwork.Context.CreateQuestionContext(new TestQuestionCreateContextKnowledge());
			var childQuestionContext = semanticNetwork.Context.CreateQuestionContext(new TestQuestionCreateNestedContext(false));

			// act
			questionContext.Dispose();
			questionContext.Children.Add(childQuestionContext);

			// assert
			Assert.DoesNotThrow(() => questionContext.Dispose());
		}

		private class TestSemanticNetwork : ISemanticNetwork
		{
			public ILocalizedString Name
			{ get; set; }

			public ISemanticNetworkContext Context
			{ get; set; }

			public ICollection<IConcept> Concepts
			{ get; set; }

			public ICollection<IStatement> Statements
			{ get; set; }

			public event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;

			public event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;

			public event EventHandler<ItemEventArgs<IStatement>> StatementAdded;

			public event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;
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

		private class TestStatement : IStatement
		{
			public ILocalizedString Name
			{ get; } = null;

			public string ID
			{ get; } = null;

			public IContext Context
			{ get; set; }

			public ILocalizedString Hint
			{ get; } = null;

			public IEnumerable<IConcept> GetChildConcepts()
			{
				return Array.Empty<IConcept>();
			}

			public FormattedLine DescribeTrue()
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeFalse()
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeQuestion()
			{
				throw new NotSupportedException();
			}

			public Boolean CheckUnique(IEnumerable<IStatement> statements)
			{
				throw new NotSupportedException();
			}
		}

		private class TestQuestionCreateNestedContext : Question
		{
			private readonly bool _createNestedContext;

			public TestQuestionCreateNestedContext(bool createNestedContext)
			{
				_createNestedContext = createNestedContext;
			}

			public override IAnswer Process(IQuestionProcessingContext context)
			{
				if (_createNestedContext)
				{
					new QuestionProcessingContext<TestQuestionCreateNestedContext>(context, new TestQuestionCreateNestedContext(false));
				}

				return null;
			}
		}

		private class TestQuestionCreateContextKnowledge : Question
		{
			public override IAnswer Process(IQuestionProcessingContext context)
			{
				IStatement testStatement;
				context.SemanticNetwork.Statements.Add(testStatement = new TestStatement());
				testStatement.Context = context;
				context.Scope.Add(testStatement);

				Assert.IsTrue(context.SemanticNetwork.Statements.Enumerate<TestStatement>(context).Any());

				return null;
			}
		}
	}
}
