using System;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Questions;

namespace AabSemantics.Sample04.CustomQuestion
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Preparation section

			Console.WriteLine("This sample demonstrates work of custom user-defined Question, which just selects random Concept.");
			Console.WriteLine();

			ISemanticNetwork semanticNetwork = new SemanticNetwork(Language.Default);

			Console.WriteLine();

			#endregion

			#region Define concepts

			semanticNetwork.Concepts.Add("Alfa".CreateConceptByName());
			semanticNetwork.Concepts.Add("Bravo".CreateConceptByName());
			semanticNetwork.Concepts.Add("Charlie".CreateConceptByName());
			semanticNetwork.Concepts.Add("Delta".CreateConceptByName());
			semanticNetwork.Concepts.Add("Echo".CreateConceptByName());
			semanticNetwork.Concepts.Add("Foxtrot".CreateConceptByName());
			semanticNetwork.Concepts.Add("Golf".CreateConceptByName());
			semanticNetwork.Concepts.Add("Hotel".CreateConceptByName());
			semanticNetwork.Concepts.Add("India".CreateConceptByName());
			semanticNetwork.Concepts.Add("Juliett".CreateConceptByName());
			semanticNetwork.Concepts.Add("Kilo".CreateConceptByName());
			semanticNetwork.Concepts.Add("Lima".CreateConceptByName());
			semanticNetwork.Concepts.Add("Mike".CreateConceptByName());
			semanticNetwork.Concepts.Add("November".CreateConceptByName());
			semanticNetwork.Concepts.Add("Oscar".CreateConceptByName());
			semanticNetwork.Concepts.Add("Papa".CreateConceptByName());
			semanticNetwork.Concepts.Add("Quebec".CreateConceptByName());
			semanticNetwork.Concepts.Add("Romeo".CreateConceptByName());
			semanticNetwork.Concepts.Add("Sierra".CreateConceptByName());
			semanticNetwork.Concepts.Add("Tango".CreateConceptByName());
			semanticNetwork.Concepts.Add("Uniform".CreateConceptByName());
			semanticNetwork.Concepts.Add("Victor".CreateConceptByName());
			semanticNetwork.Concepts.Add("Whiskey".CreateConceptByName());
			semanticNetwork.Concepts.Add("X-ray".CreateConceptByName());
			semanticNetwork.Concepts.Add("Yankee".CreateConceptByName());
			semanticNetwork.Concepts.Add("Zulu".CreateConceptByName());

			#endregion

			#region Ask questions

			// Now we can work with our Semantic Network and ask questions.
			writeAnswer(
				"Question: Give me any random Code word from NATO phonetic alphabet...",
				semanticNetwork.Ask().ForRandomConcept()); // There are no explicit knowledge about this

			#endregion

			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		private static void writeAnswer(string title, IAnswer answer)
		{
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
			Console.WriteLine();
			Console.WriteLine(new string('=', 50));
			Console.WriteLine();

			Console.WriteLine(title);

			Console.WriteLine(answer.Description.ToString());

			Console.WriteLine("Explanation:");
			foreach (var statement in answer.Explanation.Statements)
			{
				Console.WriteLine(statement.DescribeTrue());
			}
		}
	}
}
