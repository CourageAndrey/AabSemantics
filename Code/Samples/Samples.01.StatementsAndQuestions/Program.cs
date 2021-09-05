using System;

using Inventor.Core;
using Inventor.Core.Attributes;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Samples._01.StatementsAndQuestions
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Preparation section

			Console.WriteLine("This sample demonstrates simple work with concepts and statements.");
			Console.WriteLine("It works with biological classification of Bears.");
			Console.WriteLine("(More details abour Bears can be found here: https://en.wikipedia.org/wiki/Ursus_(mammal))");
			Console.WriteLine();

			// We need Language in order to create Semantic Network - so, let's use default one.
			ILanguage language = Language.Default;
			Console.WriteLine($"Selected language: ({language.Culture}) {language.Name}");

			// Semantic Network is our starting point.
			ISemanticNetwork semanticNetwork = new SemanticNetwork(language);
			Console.WriteLine("Semantic network is created...");

			// Some concepts are predefined. All of them can be found using Inventor.Core.SystemConcepts.GetAll() method.
			Console.WriteLine($"Semantic network contains {semanticNetwork.Concepts.Count} concepts.");
			Console.WriteLine();

			#endregion

			#region Define concepts

			// Need to define our own concepts to work with ...
			IConcept animal = "Kingdom: Animalia".CreateConcept("Animal");
			IConcept chordate = "Phylum: Chordata".CreateConcept("Chordate");
			IConcept mammal = "Class: Mammalia".CreateConcept("Mammal");
			IConcept carnivor = "Order: Carnivora".CreateConcept("Carnivor");
			// Just skip Ursidae, Ursinae and Ursini for short.
			IConcept bear = "Genus: Ursus".CreateConcept("Bear");
			IConcept americanBlackBear = "Ursus americanus".CreateConcept("American black bear");
			IConcept brownBear = "Ursus arctos".CreateConcept("Brown bear");
			IConcept polarBear = "Ursus maritimus".CreateConcept("Polar bear");
			IConcept asianBlackBear = "Ursus thibetanuss".CreateConcept("Asian black bear");
			
			// Pay attention to attributes of concepts below.
			IConcept hairColor = "Hair color".CreateConcept("Hair color").WithAttribute(IsSignAttribute.Value);
			IConcept black = "Color: Black".CreateConcept("Black").WithAttribute(IsValueAttribute.Value);
			IConcept brown = "Color: Brown".CreateConcept("Brown").WithAttribute(IsValueAttribute.Value);
			IConcept white = "Color: White".CreateConcept("White").WithAttribute(IsValueAttribute.Value);
			
			// ... and add them to our semantic network.
			semanticNetwork.Concepts.Add(animal);
			semanticNetwork.Concepts.Add(chordate);
			semanticNetwork.Concepts.Add(mammal);
			semanticNetwork.Concepts.Add(carnivor);
			semanticNetwork.Concepts.Add(bear);
			semanticNetwork.Concepts.Add(americanBlackBear);
			semanticNetwork.Concepts.Add(brownBear);
			semanticNetwork.Concepts.Add(polarBear);
			semanticNetwork.Concepts.Add(asianBlackBear);
			semanticNetwork.Concepts.Add(hairColor);
			semanticNetwork.Concepts.Add(black);
			semanticNetwork.Concepts.Add(brown);
			semanticNetwork.Concepts.Add(white);
			// Actually, this sample is so easy, that we could skip adding concepts to semantic network...

			#endregion

			#region Define statements

			// The most important part of preparation work is definition of statements.
			semanticNetwork.DeclareThat(chordate).IsDescendantOf(animal); // Or we can use DeclareThat(animal).IsAncestorOf(chordate) instead.
			semanticNetwork.DeclareThat(mammal).IsDescendantOf(chordate);
			semanticNetwork.DeclareThat(carnivor).IsDescendantOf(mammal);
			semanticNetwork.DeclareThat(bear).IsDescendantOf(carnivor);
			semanticNetwork.DeclareThat(americanBlackBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(brownBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(polarBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(asianBlackBear).IsDescendantOf(bear);

			// 3 statements below are also redundant for this simple case.
			semanticNetwork.DeclareThat(black).IsDescendantOf(hairColor);
			semanticNetwork.DeclareThat(brown).IsDescendantOf(hairColor);
			semanticNetwork.DeclareThat(white).IsDescendantOf(hairColor);

			semanticNetwork.DeclareThat(bear).HasSign(hairColor); // Or we can use semanticNetwork.DeclareThat(hairColor).IsSignOf(bear) instead.

			semanticNetwork.DeclareThat(americanBlackBear).HasSignValue(hairColor, black); // Or we can use semanticNetwork.DeclareThat(black).IsSignValue(americanBlackBear, hairColor) instead.
			semanticNetwork.DeclareThat(brownBear).HasSignValue(hairColor, brown);
			semanticNetwork.DeclareThat(polarBear).HasSignValue(hairColor, white);
			semanticNetwork.DeclareThat(asianBlackBear).HasSignValue(hairColor, black);

			#endregion

			#region Ask questions

			// Now we can work with our Semantic Network and ask questions.
			writeAnswer(
				"Question 1: Is Bear an animal?",
				semanticNetwork.Ask().IfIs(bear, animal)); // There are no explicit knowledge about this.

			writeAnswer(
				"Question 2: What kind of bears are there?",
				semanticNetwork.Ask().WhichDescendantsHas(bear));

			writeAnswer(
				"Question 3: What is the difference between Polar and Brown bears?",
				semanticNetwork.Ask().WhatIsTheDifference(polarBear, brownBear));

			writeAnswer(
				"Question 4: What in common American and Asian black bears have?",
				semanticNetwork.Ask().WhatInCommon(americanBlackBear, asianBlackBear));

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
