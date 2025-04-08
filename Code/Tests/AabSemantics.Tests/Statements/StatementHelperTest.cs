using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Contexts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class StatementHelperTest
	{
		[Test]
		public void GivenNoFilter_WhenEnumerate_ThenReturnAll()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();

			// act
			var filtered = semanticNetwork.Statements.Enumerate().ToList();
			var filteredT = semanticNetwork.Statements.Enumerate<IsStatement>().ToList();

			// assert
			Assert.That(semanticNetwork.Statements.SequenceEqual(filtered), Is.True);
			Assert.That(semanticNetwork.Statements.OfType<IsStatement>().SequenceEqual(filteredT), Is.True);
		}

		[Test]
		public void GivenSingleContext_WhenEnumerate_ThenReturnFiltered()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();

			foreach (var statement in semanticNetwork.Statements)
			{
				// act
				var filtered = semanticNetwork.Statements.Enumerate(statement.Context).ToList();
				var filteredT = semanticNetwork.Statements.Enumerate<IsStatement>(statement.Context).ToList();

				// assert
				Assert.That(filtered.Single(), Is.SameAs(statement));
				Assert.That(filteredT.Single(), Is.SameAs(statement));
			}
		}

		[Test]
		public void GivenManyContexts_WhenEnumerate_ThenReturnFiltered()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();
			ISemanticNetworkContext context = (SemanticNetworkContext) semanticNetwork.Statements.Last().Context;

			while (context != null)
			{
				// act
				var filtered = semanticNetwork.Statements.Enumerate(context.ActiveContexts).ToList();
				var filteredT = semanticNetwork.Statements.Enumerate<IsStatement>(context.ActiveContexts).ToList();

				// assert
				Assert.That(filtered.Count, Is.EqualTo(context.ActiveContexts.Count - 1));
				Assert.That(filteredT.Count, Is.EqualTo(context.ActiveContexts.Count - 1));

				context = context.Parent as ISemanticNetworkContext;
			}
		}

		[Test]
		public void GivenContextFilter_WhenEnumerate_ThenReturnFiltered()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();

			// act
			var filtered = semanticNetwork.Statements.Enumerate(context => (context as TestContext)?.Name.EndsWith("odd") == true).ToList();
			var filteredT = semanticNetwork.Statements.Enumerate<IsStatement>(context => (context as TestContext)?.Name.EndsWith("odd") == true).ToList();

			// assert
			Assert.That(filtered.Count, Is.EqualTo(4));
			Assert.That(filtered.Contains(semanticNetwork.Statements["3 < 4"]), Is.True);
			Assert.That(filtered.Contains(semanticNetwork.Statements["5 < 6"]), Is.True);
			Assert.That(filtered.Contains(semanticNetwork.Statements["7 < 8"]), Is.True);
			Assert.That(filtered.Contains(semanticNetwork.Statements["9 < 10"]), Is.True);
			Assert.That(filteredT.Count, Is.EqualTo(4));
			Assert.That(filteredT.Contains(semanticNetwork.Statements["3 < 4"]), Is.True);
			Assert.That(filteredT.Contains(semanticNetwork.Statements["5 < 6"]), Is.True);
			Assert.That(filteredT.Contains(semanticNetwork.Statements["7 < 8"]), Is.True);
			Assert.That(filteredT.Contains(semanticNetwork.Statements["9 < 10"]), Is.True);
		}

		private const int _numbersCount = 10;

		private static ISemanticNetwork CreateTestSemanticNetwork()
		{
			var semanticNetwork = new SemanticNetwork(Language.Default);

			for (int i = 1; i <= _numbersCount; i++)
			{
				semanticNetwork.Concepts.Add(i.CreateConceptByObject().WithAttribute(IsValueAttribute.Value));
			}

			var statements = new Dictionary<int, IStatement>();
			for (int i = 1; i < _numbersCount; i++)
			{
				var statement = new IsStatement(
					$"{i} < {i+1}",
					semanticNetwork.Concepts[i.ToString()],
					semanticNetwork.Concepts[(i + 1).ToString()]);
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
