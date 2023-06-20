using System;
using System.Linq;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Mutations;
using AabSemantics.Statements;

namespace AabSemantics.Sample08.Productions
{
	class Program
	{
		static void Main(string[] args)
		{
			// greeting
			Console.WriteLine("This sample demonstrates how to use production mechanism to automatically compare concepts.");
			Console.WriteLine();

			// metadata
			ILanguage language = Language.Default;
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new MathematicsModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			// data
			ISemanticNetwork semanticNetwork = new SemanticNetwork(language).WithModules(modules);

			var unsorted = new[] { 1, 3, 7, 8, 4, 9, 5, 2, 6 };
			foreach (int digit in unsorted)
			{
				semanticNetwork.Concepts.Add(digit.CreateConcept().WithAttribute(IsValueAttribute.Value));
			}
			Console.WriteLine($"Initial statements count: {semanticNetwork.Statements.Count}");

			semanticNetwork.DeclareThat(semanticNetwork.Concepts["1"]).IsLessThan(semanticNetwork.Concepts["2"]);

			// productions
			var lastNumberSearchPattern = new ConceptSearchPattern(concept => semanticNetwork.Statements.Count(r => r.GetChildConcepts().Contains(concept)) == 1);
			var lastNumberFilter = new StatementConceptFilter<ComparisonStatement>(statement => statement.RightValue, lastNumberSearchPattern );
			var nextNumberSearchPattern = new ConceptSearchPattern(concept => semanticNetwork.Concepts.Keys.Contains((int.Parse(concept.ID) + 1).ToString()));
			var nextNumberFilter = new StatementConceptFilter<ComparisonStatement>(statement => statement.RightValue, nextNumberSearchPattern );
			var statementSearchPattern = new StatementSearchPattern<ComparisonStatement>(conceptFilters: new[] { lastNumberFilter, nextNumberFilter });
			var production = new Production(statementSearchPattern, match =>
			{
				var lastNumber = (IConcept) match.Knowledge[lastNumberSearchPattern];
				var nextNumber = semanticNetwork.Concepts[(int.Parse(lastNumber.ID) + 1).ToString()];
				semanticNetwork.DeclareThat(lastNumber).IsLessThan(nextNumber);

				Console.WriteLine($"Production has applied to {lastNumber} and {nextNumber}.");
			});

			// apply
			var mutations = semanticNetwork.Mutate(new IMutation[] { production });

			// show result
			var representer = TextRepresenters.PlainString;
			Console.WriteLine();
			Console.WriteLine($"{mutations.Count} mutations have been applied. Defined statements:");
			foreach (var statement in semanticNetwork.Statements)
			{
				var text = statement.DescribeTrue();
				Console.WriteLine(representer.RepresentText(text, language).ToString());
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
