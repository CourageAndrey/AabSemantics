using System;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Sample03.Processes
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Preparation section

			Console.WriteLine("This sample demonstrates how sequence of processes works.");
			Console.WriteLine("It works with months and some events happening during the year.");
			Console.WriteLine();

			ISemanticNetwork semanticNetwork = new SemanticNetwork(Language.Default)
				.WithModule<ProcessesModule>();

			Console.WriteLine();

			#endregion

			#region Define concepts

			IConcept january = "January".CreateConceptByName();
			IConcept february = "February".CreateConceptByName();
			IConcept march = "March".CreateConceptByName();
			IConcept april = "April".CreateConceptByName();
			IConcept may = "May".CreateConceptByName();
			IConcept june = "June".CreateConceptByName();
			IConcept july = "July".CreateConceptByName();
			IConcept august = "August".CreateConceptByName();
			IConcept september = "September".CreateConceptByName();
			IConcept october = "October".CreateConceptByName();
			IConcept november = "November".CreateConceptByName();
			IConcept december = "December".CreateConceptByName();

			var months = new[]
			{
				january,
				february,
				march,
				april,
				may,
				june,
				july,
				august,
				september,
				october,
				november,
				december,
			};

			foreach (var month in months)
			{
				semanticNetwork.Concepts.Add(month.WithAttribute(IsProcessAttribute.Value));
			}

			#endregion

			#region Define statements

			for (int m = 0; m < 11; m++)
			{
				var currentMonth = months[m];
				var nextMonth = months[m + 1];
				semanticNetwork.DeclareThat(currentMonth).FinishesBeforeOtherStarted(nextMonth); // means automatically StartsBeforeOtherStarted
			}
			// As you can see, this system desribes only one year.
			// I.e., December is the last month - end of all events.

			#endregion

			#region Ask questions

			// Now we can work with our Semantic Network and ask questions.
			writeAnswer(
				"Question 1: What is the sequence of January and December within one year?",
				semanticNetwork.Ask().WhatIsMutualSequenceOfProcesses(months[0], months[11])); // There are no explicit knowledge about this.

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
