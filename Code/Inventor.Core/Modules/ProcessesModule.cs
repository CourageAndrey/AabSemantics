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
			Repositories.RegisterAttribute(IsProcessAttribute.Value, language => language.Attributes.IsProcess, new Xml.IsProcessAttribute());
			Repositories.RegisterAttribute(IsSequenceSignAttribute.Value, language => language.Attributes.IsSequenceSign, new Xml.IsSequenceSignAttribute());

			foreach (var sign in SequenceSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}

			Repositories.RegisterStatement<ProcessesStatement>(language => language.StatementNames.Processes, statement => new Xml.ProcessesStatement(statement as ProcessesStatement));

			Repositories.RegisterQuestion<ProcessesQuestion>();
		}
	}
}
