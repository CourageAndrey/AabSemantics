using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class IsGreaterThanStatement : StatementViewModel<Core.Statements.IsGreaterThanStatement>, IComparisonStatement
	{
		#region Properties

		public ConceptItem LeftValue
		{ get; set; }

		public ConceptItem RightValue
		{ get; set; }

		#endregion

		#region Constructors

		public IsGreaterThanStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public IsGreaterThanStatement(Core.Statements.IsGreaterThanStatement statement, ILanguage language)
			: this(new ConceptItem(statement.LeftValue, language), new ConceptItem(statement.RightValue, language), language)
		{
			_boundObject = statement;
		}

		public IsGreaterThanStatement(ConceptItem leftValue, ConceptItem rightValue, ILanguage language)
			: base(language)
		{
			LeftValue = leftValue;
			RightValue = rightValue;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new ComparisonStatementControl
			{
				EditValue = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.IsGreaterThan,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.IsGreaterThanStatement CreateStatementImplementation()
		{
			return new Core.Statements.IsGreaterThanStatement(LeftValue.Concept, RightValue.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(LeftValue.Concept, RightValue.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new IsGreaterThanStatement(LeftValue, RightValue, _language);
		}
	}
}
