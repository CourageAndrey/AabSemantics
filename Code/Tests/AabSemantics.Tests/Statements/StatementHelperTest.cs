using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Contexts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class StatementHelperTest
	{
		[Test]
		public void GivenNoFilterWhenEnumerateThenReturnAll()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();

			// act
			var filtered = semanticNetwork.Statements.Enumerate().ToList();
			var filteredT = semanticNetwork.Statements.Enumerate<ComparisonStatement>().ToList();

			// assert
			Assert.IsTrue(semanticNetwork.Statements.SequenceEqual(filtered));
			Assert.IsTrue(semanticNetwork.Statements.OfType<ComparisonStatement>().SequenceEqual(filteredT));
		}

		[Test]
		public void GivenSingleContextWhenEnumerateThenReturnFiltered()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();

			foreach (var statement in semanticNetwork.Statements)
			{
				// act
				var filtered = semanticNetwork.Statements.Enumerate(statement.Context).ToList();
				var filteredT = semanticNetwork.Statements.Enumerate<ComparisonStatement>(statement.Context).ToList();

				// assert
				Assert.AreSame(filtered.Single(), statement);
				Assert.AreSame(filteredT.Single(), statement);
			}
		}

		[Test]
		public void GivenManyContextsWhenEnumerateThenReturnFiltered()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();
			ISemanticNetworkContext context = (SemanticNetworkContext) semanticNetwork.Statements.Last().Context;

			while (context != null)
			{
				// act
				var filtered = semanticNetwork.Statements.Enumerate(context.ActiveContexts).ToList();
				var filteredT = semanticNetwork.Statements.Enumerate<ComparisonStatement>(context.ActiveContexts).ToList();

				// assert
				Assert.AreEqual(context.ActiveContexts.Count - 1, filtered.Count);
				Assert.AreEqual(context.ActiveContexts.Count - 1, filteredT.Count);

				context = context.Parent as ISemanticNetworkContext;
			}
		}

		[Test]
		public void GivenContextFilterWhenEnumerateThenReturnFiltered()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();

			// act
			var filtered = semanticNetwork.Statements.Enumerate(context => (context as TestContext)?.Name.EndsWith("odd") == true).ToList();
			var filteredT = semanticNetwork.Statements.Enumerate<ComparisonStatement>(context => (context as TestContext)?.Name.EndsWith("odd") == true).ToList();

			// assert
			Assert.AreEqual(4, filtered.Count);
			Assert.IsTrue(filtered.Contains(semanticNetwork.Statements["3 < 4"]));
			Assert.IsTrue(filtered.Contains(semanticNetwork.Statements["5 < 6"]));
			Assert.IsTrue(filtered.Contains(semanticNetwork.Statements["7 < 8"]));
			Assert.IsTrue(filtered.Contains(semanticNetwork.Statements["9 < 10"]));
			Assert.AreEqual(4, filteredT.Count);
			Assert.IsTrue(filteredT.Contains(semanticNetwork.Statements["3 < 4"]));
			Assert.IsTrue(filteredT.Contains(semanticNetwork.Statements["5 < 6"]));
			Assert.IsTrue(filteredT.Contains(semanticNetwork.Statements["7 < 8"]));
			Assert.IsTrue(filteredT.Contains(semanticNetwork.Statements["9 < 10"]));
		}

		private const int _numbersCount = 10;

		private static ISemanticNetwork createTestSemanticNetwork()
		{
			var semanticNetwork = new SemanticNetwork(Language.Default);

			for (int i = 1; i <= _numbersCount; i++)
			{
				semanticNetwork.Concepts.Add(i.CreateConcept().WithAttribute(IsValueAttribute.Value));
			}

			var statements = new Dictionary<int, IStatement>();
			for (int i = 1; i < _numbersCount; i++)
			{
				var statement = new ComparisonStatement(
					$"{i} < {i+1}",
					semanticNetwork.Concepts[i.ToString()],
					semanticNetwork.Concepts[(i + 1).ToString()],
					ComparisonSigns.IsLessThan);
				semanticNetwork.Statements.Add(statement);
				statements[i] = statement;
			}

			ISemanticNetworkContext context = semanticNetwork.Context;
			for (int i = 2; i < _numbersCount; i++)
			{
				string name2 = i % 2 == 0 ? "even" : "odd";
				context = new TestContext($"{i} {name2}", context);

				statements[i].Context = context;
			}

			return semanticNetwork;
		}

		private class TestContext : SemanticNetworkContext
		{
			public string Name
			{ get; }

			internal TestContext(string name, ISemanticNetworkContext parent)
				: base(parent.Language, parent, parent.SemanticNetwork)
			{
				Name = name;
			}
		}
	}
}
