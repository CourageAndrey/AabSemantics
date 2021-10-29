using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Boolean;

namespace Samples.Semantics.Sample07.CustomModule
{
	class Program
	{
		static void Main(string[] args)
		{
			// greeting
			Console.WriteLine("This sample demonstrates how to create custom module.");
			Console.WriteLine();

			// initialize metadata
			new CustomModule().RegisterMetadata();
			Language.PrepareModulesToSerialization<Language>();

			// load additional languages
			var startupPath = AppDomain.CurrentDomain.BaseDirectory;
			var languages = startupPath.LoadAdditionalLanguages();
			languages.Add(Language.Default);

			// select active sample language
			int selectedLanguage;
			do
			{
				Console.WriteLine("Please, select one of sample languages:");
				int languageNumber = 1;
				foreach (var l in languages)
				{
					Console.WriteLine($"{languageNumber++}. [{l.Culture}] {l.Name}");
				}

				var key = Console.ReadKey();
				if (!int.TryParse(key.KeyChar.ToString(), out selectedLanguage) || selectedLanguage < 1)
				{
					Console.WriteLine($"Please, type number from 1 to {languages.Count}.");
					selectedLanguage = int.MinValue;
				}

				Console.WriteLine();
			} while (selectedLanguage < 0 || selectedLanguage > languages.Count);

			var language = languages.Skip(selectedLanguage - 1).First();

			// create semantic network
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<CustomModule>();

			Func<string, IConcept> createEditableConcept = text => new Concept(
				null,
				new LocalizedStringVariable(new[]
				{
					new KeyValuePair<string, string>("en-US", text),
					new KeyValuePair<string, string>("ru-RU", text),
				}),
				new LocalizedStringVariable(new[]
				{
					new KeyValuePair<string, string>("en-US", text + " (hint)"),
					new KeyValuePair<string, string>("ru-RU", text + " (hint)"),
				}));

			IConcept concept1, concept2;
			semanticNetwork.Concepts.Add(concept1 = createEditableConcept("Test 1").WithAttribute(CustomAttribute.Value));
			semanticNetwork.Concepts.Add(concept2 = createEditableConcept("Test 2").WithAttribute(CustomAttribute.Value));

			CustomStatement correct, wrong;
			semanticNetwork.Statements.Add(correct = new CustomStatement(null, concept1, concept2));
			semanticNetwork.Statements.Add(wrong = new CustomStatement(null, concept1, concept1));

			// check consistency - "wrong" has to be detected
			Console.WriteLine();
			Console.WriteLine("Check consistency:");
			var errors = semanticNetwork.CheckConsistency();
			Console.Write(TextRepresenters.PlainString.RepresentText(errors, language));
			Console.WriteLine();

			// check [de]serialization works
			string saveFile = "test.xml";
			Inventor.Semantics.Xml.SemanticNetworkXmlExtensions.Save(semanticNetwork, saveFile);
			Inventor.Semantics.Xml.SemanticNetworkXmlExtensions.LoadSemanticNetworkFromXml(saveFile, language);

			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
