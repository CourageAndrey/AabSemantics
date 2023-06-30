using System;
using System.Collections.Generic;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Tests
{
	public static class TestSemanticNetworkExtension
	{
		public static ProcessesTestSemanticNetwork CreateProcessesTestData(this ISemanticNetwork semanticNetwork)
		{
			return new ProcessesTestSemanticNetwork(semanticNetwork);
		}
	}

	public class ProcessesTestSemanticNetwork
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		#region Processes

		public IConcept ProcessA
		{ get; }

		public IConcept ProcessB
		{ get; }

		#endregion

		#endregion

		public ProcessesTestSemanticNetwork(ISemanticNetwork semanticNetwork)
		{
			#region Semantic network

			SemanticNetwork = semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<ProcessesModule>();

			#endregion

			#region Processes
			
			Func<String, LocalizedStringVariable> getString = text => new LocalizedStringVariable(
				new Dictionary<String, String>
				{
					{ "ru-RU", text },
					{ "en-US", text },
				});

			SemanticNetwork.Concepts.Add(ProcessA = new Concept(nameof(ProcessA), getString("Process A")));
			SemanticNetwork.Concepts.Add(ProcessB = new Concept(nameof(ProcessB), getString("Process B")));

			#endregion

			#region Concept Attributes

			ProcessA.WithAttribute(IsProcessAttribute.Value);
			ProcessB.WithAttribute(IsProcessAttribute.Value);

			#endregion

			#region Statements

			SemanticNetwork.DeclareThat(ProcessA).StartsBeforeOtherStarted(ProcessB);
			SemanticNetwork.DeclareThat(ProcessA).FinishesAfterOtherFinished(ProcessB);

			#endregion
		}
	}
}