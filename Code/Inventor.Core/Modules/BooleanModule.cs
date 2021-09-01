using System;

using Inventor.Core.Attributes;
using Inventor.Core.Questions;

namespace Inventor.Core.Modules
{
	public class BooleanModule : ExtensionModule
	{
		public const String ModuleName = "System.Boolean";

		public BooleanModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			semanticNetwork.RegisterAttribute(IsValueAttribute.Value, language => language.Attributes.IsValue, new Xml.IsValueAttribute());
			semanticNetwork.RegisterAttribute(IsBooleanAttribute.Value, language => language.Attributes.IsBoolean, new Xml.IsBooleanAttribute());

			foreach (var boolean in LogicalValues.All)
			{
				semanticNetwork.Concepts.Add(boolean);
			}

			semanticNetwork.RegisterQuestion<CheckStatementQuestion>();
		}
	}
}
