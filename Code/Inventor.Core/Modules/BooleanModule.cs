using System;

using Inventor.Core.Attributes;
using Inventor.Core.Concepts;
using Inventor.Core.Metadata;
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
			foreach (var boolean in LogicalValues.All)
			{
				semanticNetwork.Concepts.Add(boolean);
			}
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsValueAttribute.Value, language => language.Concepts.Attributes.IsValue, new Xml.IsValueAttribute());
			Repositories.RegisterAttribute(IsBooleanAttribute.Value, language => language.Concepts.Attributes.IsBoolean, new Xml.IsBooleanAttribute());
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CheckStatementQuestion>();
		}
	}
}
