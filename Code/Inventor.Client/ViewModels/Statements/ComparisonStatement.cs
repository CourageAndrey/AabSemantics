using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

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
			: this(null as ConceptItem, null, null, language)
		{ }

		public ComparisonStatement(Core.Statements.ComparisonStatement statement, ILanguage language)
			: this(new ConceptItem(statement.LeftValue, language), new ConceptItem(statement.RightValue, language), new ConceptItem(statement.ComparisonSign, language), language)
		{
			_boundObject = statement;
		}

		public ComparisonStatement(ConceptItem leftValue, ConceptItem rightValue, ConceptItem comparisonSign, ILanguage language)
			: base(language)
		{
			LeftValue = leftValue;
			RightValue = rightValue;
			ComparisonSign = comparisonSign;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new ComparisonStatementControl
			{
				Statement = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.Comparison,
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
			return new Core.Statements.ComparisonStatement(LeftValue.Concept, RightValue.Concept, ComparisonSign.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(LeftValue.Concept, RightValue.Concept, ComparisonSign.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new ComparisonStatement(LeftValue, RightValue, ComparisonSign, _language);
		}
	}
}
