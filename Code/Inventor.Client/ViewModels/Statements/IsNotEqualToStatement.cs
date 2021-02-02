using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class IsNotEqualToStatement : StatementViewModel<Core.Statements.IsNotEqualToStatement>, IComparisonStatement
	{
		#region Properties

		public ConceptItem LeftValue
		{ get; set; }

		public ConceptItem RightValue
		{ get; set; }

		#endregion

		#region Constructors

		public IsNotEqualToStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public IsNotEqualToStatement(Core.Statements.IsNotEqualToStatement statement, ILanguage language)
			: this(new ConceptItem(statement.LeftValue, language), new ConceptItem(statement.RightValue, language), language)
		{
			_boundObject = statement;
		}

		public IsNotEqualToStatement(ConceptItem leftValue, ConceptItem rightValue, ILanguage language)
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
				Title = language.StatementNames.IsNotEqualTo,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.IsNotEqualToStatement CreateStatementImplementation()
		{
			return new Core.Statements.IsNotEqualToStatement(LeftValue.Concept, RightValue.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(LeftValue.Concept, RightValue.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new IsNotEqualToStatement(LeftValue, RightValue, _language);
		}
	}
}
