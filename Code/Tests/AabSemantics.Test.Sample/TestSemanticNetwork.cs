using System;
using System.Collections.Generic;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Mathematics.Tests;
using AabSemantics.Modules.Processes.Tests;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Modules.Set.Tests;
using AabSemantics.Statements;

namespace AabSemantics.Test.Sample
{
	public class CombinedTestSemanticNetwork
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		public MathematicsTestSemanticNetwork Mathematics
		{ get; }

		public ProcessesTestSemanticNetwork Processes
		{ get; }

		public SetTestSemanticNetwork Set
		{ get; }

		#region Subject Areas

		public IConcept SubjectArea_Numbers
		{ get; }

		public IConcept SubjectArea_Processes
		{ get; }

		#endregion

		#endregion

		public CombinedTestSemanticNetwork(ISemanticNetwork semanticNetwork)
		{
			SemanticNetwork = semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			((LocalizedStringVariable) SemanticNetwork.Name).SetLocale("ru-RU", "Тестовая база знаний");
			((LocalizedStringVariable) SemanticNetwork.Name).SetLocale("en-US", "Test knowledgebase");

			Mathematics = SemanticNetwork.CreateMathematicsTestData();
			Processes = SemanticNetwork.CreateProcessesTestData();
			Set = SemanticNetwork.CreateSetTestData();

			SemanticNetwork.Concepts.Add(SubjectArea_Numbers = new Concept(nameof(SubjectArea_Numbers), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Числа" },
				{ "en-US", "Numbers" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Числа." },
				{ "en-US", "Numbers." },
			})));

			SemanticNetwork.Concepts.Add(SubjectArea_Processes = new Concept(nameof(SubjectArea_Processes), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Процессы" },
				{ "en-US", "Processes" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Процессы." },
				{ "en-US", "Processes." },
			})));

			SemanticNetwork.DeclareThat(SubjectArea_Numbers).IsSubjectAreaOf(new[]
			{
				Mathematics.Number0,
				Mathematics.NumberZero,
				Mathematics.NumberNotZero,
				Mathematics.Number1,
				Mathematics.Number1or2,
				Mathematics.Number2,
				Mathematics.Number2or3,
				Mathematics.Number3,
				Mathematics.Number3or4,
				Mathematics.Number4,
			});

			SemanticNetwork.DeclareThat(SubjectArea_Processes).IsSubjectAreaOf(new[]
			{
				Processes.ProcessA,
				Processes.ProcessB,
			});
		}
	}

	public static class TestSemanticNetworkExtension
	{
		public static CombinedTestSemanticNetwork CreateCombinedTestData(this ISemanticNetwork semanticNetwork)
		{
			return new CombinedTestSemanticNetwork(semanticNetwork);
		}
	}
}