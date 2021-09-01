using System;

using Inventor.Core.Attributes;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Modules
{
	public class ProcessesModule : ExtensionModule
	{
		public const String ModuleName = "System.Processes";

		public ProcessesModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			semanticNetwork.RegisterAttribute(IsProcessAttribute.Value, language => language.Attributes.IsProcess, new Xml.IsProcessAttribute());
			semanticNetwork.RegisterAttribute(IsSequenceSignAttribute.Value, language => language.Attributes.IsSequenceSign, new Xml.IsSequenceSignAttribute());

			foreach (var sign in SequenceSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}

			semanticNetwork.RegisterStatement<ProcessesStatement>(language => language.StatementNames.Processes);

			semanticNetwork.RegisterQuestion<ProcessesQuestion>();
		}
	}
}
