using System;

using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Samples._04.CustomQuestion
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

			semanticNetwork.Concepts.Add("Alfa".CreateConcept());
			semanticNetwork.Concepts.Add("Bravo".CreateConcept());
			semanticNetwork.Concepts.Add("Charlie".CreateConcept());
			semanticNetwork.Concepts.Add("Delta".CreateConcept());
			semanticNetwork.Concepts.Add("Echo".CreateConcept());
			semanticNetwork.Concepts.Add("Foxtrot".CreateConcept());
			semanticNetwork.Concepts.Add("Golf".CreateConcept());
			semanticNetwork.Concepts.Add("Hotel".CreateConcept());
			semanticNetwork.Concepts.Add("India".CreateConcept());
			semanticNetwork.Concepts.Add("Juliett".CreateConcept());
			semanticNetwork.Concepts.Add("Kilo".CreateConcept());
			semanticNetwork.Concepts.Add("Lima".CreateConcept());
			semanticNetwork.Concepts.Add("Mike".CreateConcept());
			semanticNetwork.Concepts.Add("November".CreateConcept());
			semanticNetwork.Concepts.Add("Oscar".CreateConcept());
			semanticNetwork.Concepts.Add("Papa".CreateConcept());
			semanticNetwork.Concepts.Add("Quebec".CreateConcept());
			semanticNetwork.Concepts.Add("Romeo".CreateConcept());
			semanticNetwork.Concepts.Add("Sierra".CreateConcept());
			semanticNetwork.Concepts.Add("Tango".CreateConcept());
			semanticNetwork.Concepts.Add("Uniform".CreateConcept());
			semanticNetwork.Concepts.Add("Victor".CreateConcept());
			semanticNetwork.Concepts.Add("Whiskey".CreateConcept());
			semanticNetwork.Concepts.Add("X-ray".CreateConcept());
			semanticNetwork.Concepts.Add("Yankee".CreateConcept());
			semanticNetwork.Concepts.Add("Zulu".CreateConcept());

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
