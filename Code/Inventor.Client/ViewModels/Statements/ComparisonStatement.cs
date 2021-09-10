using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;
using Inventor.Core.Localization.Modules;

namespace Inventor.Client.ViewModels.Statements
{
	public class ComparisonStatement : StatementViewModel<Core.Statements.ComparisonStatement>
	{
		#region Properties

		public ConceptItem LeftValue
		{ get; set; }

		public ConceptItem RightValue
		{ get; set; }

		public ConceptItem ComparisonSign
		{ get; set; }

		#endregion

		#region Constructors

		public ComparisonStatement(ILanguage language)
			: this(string.Empty, null, null, null, language)
		{ }

		public ComparisonStatement(Core.Statements.ComparisonStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.LeftValue, language), new ConceptItem(statement.RightValue, language), new ConceptItem(statement.ComparisonSign, language), language)
		{
			BoundObject = statement;
		}

		public ComparisonStatement(string id, ConceptItem leftValue, ConceptItem rightValue, ConceptItem comparisonSign, ILanguage language)
			: base(id, language)
		{
			LeftValue = leftValue;
			RightValue = rightValue;
			ComparisonSign = comparisonSign;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new ComparisonStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageMathematicsModule>().Statements.Names.Comparison,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.ComparisonStatement CreateStatementImplementation()
		{
			return new Core.Statements.ComparisonStatement(ID, LeftValue.Concept, RightValue.Concept, ComparisonSign.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, LeftValue.Concept, RightValue.Concept, ComparisonSign.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new ComparisonStatement(ID, LeftValue, RightValue, ComparisonSign, _language);
		}
	}
}
