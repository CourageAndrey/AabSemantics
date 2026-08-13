using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Modules.Mathematics.Localization;

namespace AabSemantics.Extensions.WPF.ViewModels.Statements
{
	/// <summary>Editable view over the comparison statement.</summary>
	public class ComparisonStatement : StatementViewModel<Modules.Mathematics.Statements.ComparisonStatement>
	{
		#region Properties

		/// <summary>The left-hand value.</summary>
		public ConceptItem LeftValue
		{ get; set; }

		/// <summary>The right-hand value.</summary>
		public ConceptItem RightValue
		{ get; set; }

		/// <summary>The comparison sign.</summary>
		public ConceptItem ComparisonSign
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty view model for a new statement.</summary>
		/// <param name="language">Language used to render the statement's text.</param>
		public ComparisonStatement(ILanguage language)
			: this(string.Empty, null, null, null, language)
		{ }

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public ComparisonStatement(Modules.Mathematics.Statements.ComparisonStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.LeftValue, language), new ConceptItem(statement.RightValue, language), new ConceptItem(statement.ComparisonSign, language), language)
		{
			BoundObject = statement;
		}

		/// <summary>Creates a view model from explicit values.</summary>
		/// <param name="id">Identifier of the edited statement; empty when creating a new one.</param>
		/// <param name="leftValue">The left-hand value.</param>
		/// <param name="rightValue">The right-hand value.</param>
		/// <param name="comparisonSign">The comparison sign.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public ComparisonStatement(string id, ConceptItem leftValue, ConceptItem rightValue, ConceptItem comparisonSign, ILanguage language)
			: base(id, language)
		{
			LeftValue = leftValue;
			RightValue = rightValue;
			ComparisonSign = comparisonSign;
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>Builds the dialog used to edit this statement.</summary>
		/// <param name="owner">Window the dialog belongs to.</param>
		/// <param name="semanticNetwork">Network supplying the pick lists.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>An unshown dialog.</returns>
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

		/// <summary>Builds the statement from the edited values.</summary>
		/// <returns>The created statement.</returns>
		protected override Modules.Mathematics.Statements.ComparisonStatement CreateStatementImplementation()
		{
			return new Modules.Mathematics.Statements.ComparisonStatement(ID, LeftValue.Concept, RightValue.Concept, ComparisonSign.Concept);
		}

		/// <summary>Writes the edited values onto the bound statement.</summary>
		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, LeftValue.Concept, RightValue.Concept, ComparisonSign.Concept);
		}

		#endregion

		/// <summary>Copies the view model.</summary>
		/// <returns>An independent copy.</returns>
		public override StatementViewModel Clone()
		{
			return new ComparisonStatement(ID, LeftValue, RightValue, ComparisonSign, _language);
		}
	}
}
