using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class IsEqualToStatement : StatementViewModel<Core.Statements.IsEqualToStatement>, IComparisonStatement
	{
		#region Properties

		public ConceptItem LeftValue
		{ get; set; }

		public ConceptItem RightValue
		{ get; set; }

		#endregion

		#region Constructors

		public IsEqualToStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public IsEqualToStatement(Core.Statements.IsEqualToStatement statement, ILanguage language)
			: this(new ConceptItem(statement.LeftValue, language), new ConceptItem(statement.RightValue, language), language)
		{
			_boundObject = statement;
		}

		public IsEqualToStatement(ConceptItem leftValue, ConceptItem rightValue, ILanguage language)
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
				Title = language.StatementNames.IsEqualTo,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.IsEqualToStatement CreateStatementImplementation()
		{
			return new Core.Statements.IsEqualToStatement(LeftValue.Concept, RightValue.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(LeftValue.Concept, RightValue.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new IsEqualToStatement(LeftValue, RightValue, _language);
		}
	}
}
