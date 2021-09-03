using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;
using Inventor.Core.Text;

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
			foreach (var sign in ComparisonSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsComparisonSignAttribute.Value, language => language.Attributes.IsComparisonSign, new Xml.IsComparisonSignAttribute());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ComparisonStatement>(
				language => language.StatementNames.Comparison,
				statement => new Xml.ComparisonStatement(statement),
				typeof(Xml.ComparisonStatement),
				checkComparisonValueSystems);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ComparisonQuestion>();
		}

		private static void checkComparisonValueSystems(
			ICollection<ComparisonStatement> statements,
			UnstructuredContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var contradiction in statements.CheckForContradictions())
			{
				result
					.Append(
						language => language.Consistency.ErrorComparisonContradiction,
						new Dictionary<String, IKnowledge>
						{
							{ Strings.ParamLeftValue, contradiction.Value1 },
							{ Strings.ParamRightValue, contradiction.Value2 },
						})
					.Append(contradiction.Signs.EnumerateOneLine());
			}
		}
	}
}
