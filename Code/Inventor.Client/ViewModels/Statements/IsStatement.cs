using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class IsStatement : StatementViewModel<Core.Statements.IsStatement>
	{
		#region Properties

		public ConceptItem Parent
		{ get; set; }

		public ConceptItem Child
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public IsStatement(Core.Statements.IsStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Parent, language), new ConceptItem(statement.Child, language), language)
		{
			_boundObject = statement;
		}

		public IsStatement(ConceptItem parent, ConceptItem child, ILanguage language)
			: base(language)
		{
			Parent = parent;
			Child = child;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new IsStatementControl
			{
				EditValue = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.Clasification,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.IsStatement CreateStatementImplementation()
		{
			return new Core.Statements.IsStatement(Parent.Concept, Child.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(Parent.Concept, Child.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new IsStatement(Parent, Child, _language);
		}
	}
}
