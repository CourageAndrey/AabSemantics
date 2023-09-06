using System;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Sample05.CustomStatement
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Preparation section

			Console.WriteLine("This sample demonstrates work of custom user-defined Statement with corresponding Question.");
			Console.WriteLine();

			ISemanticNetwork semanticNetwork = new SemanticNetwork(Language.Default)
				.WithModule<BooleanModule>();

			Console.WriteLine();

			#endregion

			#region Define concepts

			IConcept Alice, Bob, Charlie, Dave, Eve;

			semanticNetwork.Concepts.Add(Alice = "Alice".CreateConceptByName());
			semanticNetwork.Concepts.Add(Bob = "Bob".CreateConceptByName());
			semanticNetwork.Concepts.Add(Charlie = "Charlie".CreateConceptByName());
			semanticNetwork.Concepts.Add(Dave = "Dave".CreateConceptByName());
			semanticNetwork.Concepts.Add(Eve = "Eve".CreateConceptByName());

			#endregion

			#region Define statements

			// Height (from taller to shorter one): Alice, Bob, Charlie, Dave, Eve.
			semanticNetwork.DeclareThat(Alice).IsTallerThan(Bob);
			semanticNetwork.DeclareThat(Bob).IsTallerThan(Charlie);
			semanticNetwork.DeclareThat(Dave).IsShorterThan(Charlie);
			semanticNetwork.DeclareThat(Eve).IsShorterThan(Dave);

			#endregion

			#region Ask questions

			// Now we can work with our Semantic Network and ask questions.
			writeAnswer(
				"Question 1: Who is taller than Alice?",
				semanticNetwork.Ask().WhoIsTallerThan(Alice));

			writeAnswer(
				"Question 2: Who is taller than Bob?",
				semanticNetwork.Ask().WhoIsTallerThan(Bob));

			writeAnswer(
				"Question 3: Who is taller than Eve?",
				semanticNetwork.Ask().WhoIsTallerThan(Eve));

			writeAnswer(
				"Question 4: Who is shorter than Alice?",
				semanticNetwork.Ask().WhoIsShorterThan(Alice));

			writeAnswer(
				"Question 5: Who is shorter than Dave?",
				semanticNetwork.Ask().WhoIsShorterThan(Dave));

			writeAnswer(
				"Question 6: Who is shorter than Eve?",
				semanticNetwork.Ask().WhoIsShorterThan(Eve));

			writeAnswer(
				"Question 7: Is Bob taller than Dave?",
				semanticNetwork.Ask().IsTallerThan(Bob, Dave));

			// the same question, but asked using CheckStatementQuestion
			writeAnswer(
				"Question 8: Is that true, that Bob is taller than Dave?",
				semanticNetwork.Ask().IsTrueThat(new IsTallerThanStatement(Bob,  Dave)));

			writeAnswer(
				"Question 9: Is Dave taller than Bob?",
				semanticNetwork.Ask().IsTallerThan(Dave, Bob));

			// the same question, but asked using CheckStatementQuestion
			writeAnswer(
				"Question 10: Is that true, that Dave is taller than Bob?",
				semanticNetwork.Ask().IsTrueThat(new IsTallerThanStatement(Dave, Bob)));

			#endregion

			#region Just to show new possibility to define statements

			Console.WriteLine();
			Console.WriteLine("Please, check corresponfing SECTION in code in order to get acquainted with list-declarations.");
			semanticNetwork.Statements.Clear(); // in order not to contradict previously declared statements
			semanticNetwork.DeclareThat(Alice).IsTallerThan(new[] { Bob, Charlie, Dave, Eve });

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
