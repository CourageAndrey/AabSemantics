﻿using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Contexts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Questions;

namespace Inventor.Semantics.Test.Contexts
{
	[TestFixture]
	public class ContextsTest
	{
		[Test]
		public void EnsureThatPossibleToDisposeContextWithoutChildContexts()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			// act and assert
			Assert.DoesNotThrow(() =>
			{
				new TestQuestionCreateNestedContext().Ask(semanticNetwork.Context);
			});
		}

		[Test]
		public void EnsureThatPossibleToDisposeContextWithAllDisposedChildContexts()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var disposedContexts = new List<bool>();

			// act and assert
			for (int i = 0; i < 10; i++)
			{
				disposedContexts.Add(true);
				Assert.DoesNotThrow(() =>
				{
					new TestQuestionCreateNestedContext(disposedContexts).Ask(semanticNetwork.Context);
				});
			}
		}

		[Test]
		public void EnsureThatImpossibleToDisposeContextWhichHasAllNotDisposedChildContexts()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var disposedContexts = new List<bool>();

			// act and assert
			for (int i = 0; i < 10; i++)
			{
				disposedContexts.Add(false);
				Assert.Throws<InvalidOperationException>(() =>
				{
					new TestQuestionCreateNestedContext(disposedContexts).Ask(semanticNetwork.Context);
				});
			}
		}

		[Test]
		public void EnsureThatImpossibleToDisposeContextWhichHasOneNotDisposedChildContext()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var disposedContexts = new[] { true, true, true, true, true, true, true, true, true, true };

			// act and assert
			for (int i = 0; i < 10; i++)
			{
				disposedContexts[i] = false;
				Assert.Throws<InvalidOperationException>(() =>
				{
					new TestQuestionCreateNestedContext(disposedContexts).Ask(semanticNetwork.Context);
				});
				disposedContexts[i] = true;
			}
		}

		// This test partially duplicates previous one.
		[Test]
		public void ImpossibleToDisposeContextWithActiveChildren()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var questionContext = semanticNetwork.Context.CreateQuestionContext(new TestQuestionCreateContextKnowledge());
			var child1Context = questionContext.CreateQuestionContext(new TestQuestionCreateNestedContext());
			var child2Context = questionContext.CreateQuestionContext(new TestQuestionCreateNestedContext());

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
			Assert.Throws<InvalidOperationException>(() => systemContext.Instantiate(new TestSemanticNetwork()));
		}

		[Test]
		public void ContextDisposalWorksOnlyOnce()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var questionContext = semanticNetwork.Context.CreateQuestionContext(new TestQuestionCreateContextKnowledge());
			var childQuestionContext = semanticNetwork.Context.CreateQuestionContext(new TestQuestionCreateNestedContext());

			// act
			questionContext.Dispose();
			questionContext.Children.Add(childQuestionContext);

			// assert
			Assert.DoesNotThrow(() => questionContext.Dispose());
		}

		[Test]
		public void ExplicitlyCheckContextDisposing()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			// act && assert
			Assert.DoesNotThrow(() =>
			{
				using (new DisposableProcessingContext(semanticNetwork.Context))
				{ }
			});

			Assert.DoesNotThrow(() =>
			{
				using (var context = createChildDisposableContext(semanticNetwork.Context))
				{
					for (int i = 0; i < 5; i++)
					{
						var child = createChildDisposableContext(context);
						child.Dispose(); // this call remove child from context.Children
						context.Children.Add(child); // return child back
					}
				}
			});

			Assert.Throws<InvalidOperationException>(() =>
			{
				using (var context = createChildDisposableContext(semanticNetwork.Context))
				{
					createChildDisposableContext(context);
				}
			});
		}

		private static DisposableProcessingContext createChildDisposableContext(ISemanticNetworkContext parent)
		{
			var statement = new IsStatement(null, new Concept(), new Concept());
			var question = new CheckStatementQuestion(statement);
			return new QuestionProcessingContext<CheckStatementQuestion>(parent, question);
		}

		private class TestSemanticNetwork : ISemanticNetwork
		{
			public ILocalizedString Name
			{ get; set; }

			public ISemanticNetworkContext Context
			{ get; set; }

			public IKeyedCollection<IConcept> Concepts
			{ get; set; }

			public IKeyedCollection<IStatement> Statements
			{ get; set; }

			public IDictionary<String, IExtensionModule> Modules
			{ get; set; }
		}

		private class TestStatement : IStatement
		{
			public ILocalizedString Name
			{ get; } = null;

			public string ID
			{ get; } = string.Empty;

			public IContext Context
			{ get; set; }

			public ILocalizedString Hint
			{ get; } = null;

			public IEnumerable<IConcept> GetChildConcepts()
			{
				return Array.Empty<IConcept>();
			}

			public IText DescribeTrue()
			{
				throw new NotSupportedException();
			}

			public IText DescribeFalse()
			{
				throw new NotSupportedException();
			}

			public IText DescribeQuestion()
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
			private readonly ICollection<bool> _disposedNestedContexts;

			public TestQuestionCreateNestedContext(ICollection<bool> disposedNestedContexts)
			{
				_disposedNestedContexts = disposedNestedContexts;
			}

			public TestQuestionCreateNestedContext()
				: this(Array.Empty<bool>())
			{ }

			public override IAnswer Process(IQuestionProcessingContext context)
			{
				foreach (bool disposeNested in _disposedNestedContexts)
				{
					var nestedContext = new QuestionProcessingContext<TestQuestionCreateNestedContext>(context, new TestQuestionCreateNestedContext());
					if (disposeNested)
					{
						nestedContext.Dispose();
					}
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
