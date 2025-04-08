using System;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Sample01.StatementsAndQuestions
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

			// Initialize modules metadata
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
				new SetModule(),
				//new MathematicsModule(),
				//new ProcessesModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}
			Console.WriteLine("Modules are initialized...");

			// Semantic Network is our starting point.
			ISemanticNetwork semanticNetwork = new SemanticNetwork(language)
				.WithModules(modules);
			Console.WriteLine("Semantic network is created...");

			// Some concepts are predefined. All of them can be found using AabSemantics.SystemConcepts.GetAll() method.
			Console.WriteLine($"Semantic network contains {semanticNetwork.Concepts.Count} concepts.");
			Console.WriteLine();

			#endregion

			#region Define concepts

			// Need to define our own concepts to work with ...
			IConcept animal = "Kingdom: Animalia".CreateConceptById("Animal");
			IConcept chordate = "Phylum: Chordata".CreateConceptById("Chordate");
			IConcept mammal = "Class: Mammalia".CreateConceptById("Mammal");
			IConcept carnivor = "Order: Carnivora".CreateConceptById("Carnivor");
			// Just skip Ursidae, Ursinae and Ursini for short.
			IConcept bear = "Genus: Ursus".CreateConceptById("Bear");
			IConcept americanBlackBear = "Ursus americanus".CreateConceptById("American black bear");
			IConcept brownBear = "Ursus arctos".CreateConceptById("Brown bear");
			IConcept polarBear = "Ursus maritimus".CreateConceptById("Polar bear");
			IConcept asianBlackBear = "Ursus thibetanuss".CreateConceptById("Asian black bear");
			IConcept pandaBear = "Ailuropoda melanoleuca".CreateConceptById("Panda bear");

			// Pay attention to attributes of concepts below.
			IConcept hairColor = "Hair color".CreateConceptById("Hair color").WithAttribute(IsSignAttribute.Value);
			IConcept black = "Color: Black".CreateConceptById("Black").WithAttribute(IsValueAttribute.Value);
			IConcept brown = "Color: Brown".CreateConceptById("Brown").WithAttribute(IsValueAttribute.Value);
			IConcept white = "Color: White".CreateConceptById("White").WithAttribute(IsValueAttribute.Value);
			IConcept blackAndWhite = "Color: Mixed (black and white)".CreateConceptById("Mixed (black and white)").WithAttribute(IsValueAttribute.Value);

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
			semanticNetwork.Concepts.Add(blackAndWhite);
			// Actually, this sample is so easy, that we could skip adding concepts to semantic network...

			#endregion

			#region Define statements

			// The most important part of preparation work is definition of statements.
			var r = TextRenders.PlainString;
			IsStatement x = semanticNetwork.DeclareThat(chordate).IsDescendantOf(animal); // Or we can use DeclareThat(animal).IsAncestorOf(chordate) instead.
			var c = x.ToCustomStatement();
			var t = r.Render(c.DescribeTrue(), language);
			Console.Write(t);

			semanticNetwork.DeclareThat(mammal).IsDescendantOf(chordate);
			semanticNetwork.DeclareThat(carnivor).IsDescendantOf(mammal);
			semanticNetwork.DeclareThat(bear).IsDescendantOf(carnivor);
			semanticNetwork.DeclareThat(americanBlackBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(brownBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(polarBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(asianBlackBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(pandaBear).IsDescendantOf(bear);

			// 3 statements below are also redundant for this simple case.
			semanticNetwork.DeclareThat(black).IsDescendantOf(hairColor);
			semanticNetwork.DeclareThat(brown).IsDescendantOf(hairColor);
			semanticNetwork.DeclareThat(white).IsDescendantOf(hairColor);
			semanticNetwork.DeclareThat(blackAndWhite).IsDescendantOf(hairColor);

			semanticNetwork.DeclareThat(bear).HasSign(hairColor); // Or we can use semanticNetwork.DeclareThat(hairColor).IsSignOf(bear) instead.

			semanticNetwork.DeclareThat(americanBlackBear).HasSignValue(hairColor, black); // Or we can use semanticNetwork.DeclareThat(black).IsSignValue(americanBlackBear, hairColor) instead.
			semanticNetwork.DeclareThat(brownBear).HasSignValue(hairColor, brown);
			semanticNetwork.DeclareThat(polarBear).HasSignValue(hairColor, white);
			semanticNetwork.DeclareThat(asianBlackBear).HasSignValue(hairColor, black);
			semanticNetwork.DeclareThat(pandaBear).HasSignValue(hairColor, blackAndWhite);

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

			writeAnswer(
				"Question 5: What is the difference between Panda and Asian black bears have?",
				semanticNetwork.Ask().WhatIsTheDifference(pandaBear, asianBlackBear));

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
