using System;

using Inventor.Core.Attributes;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Modules
{
	public class MathematicsModule : ExtensionModule
	{
		public const String ModuleName = "System.Mathematics";

		public MathematicsModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			semanticNetwork.RegisterAttribute(IsComparisonSignAttribute.Value, language => language.Attributes.IsComparisonSign, new Xml.IsComparisonSignAttribute());

			foreach (var sign in ComparisonSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}

			semanticNetwork.RegisterStatement<ComparisonStatement>(language => language.StatementNames.Comparison);

			semanticNetwork.RegisterQuestion<ComparisonQuestion>();
		}
	}
}
