using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class IsStatement : StatementViewModel<Core.Statements.IsStatement>
	{
		#region Properties

		public ConceptItem Ancestor
		{ get; set; }

		public ConceptItem Descendant
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public IsStatement(Core.Statements.IsStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Ancestor, language), new ConceptItem(statement.Descendant, language), language)
		{
			_boundObject = statement;
		}

		public IsStatement(ConceptItem ancestor, ConceptItem descendant, ILanguage language)
			: base(language)
		{
			Ancestor = ancestor;
			Descendant = descendant;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new IsStatementControl
			{
				Statement = this,
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
			return new Core.Statements.IsStatement(Ancestor.Concept, Descendant.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(Ancestor.Concept, Descendant.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new IsStatement(Ancestor, Descendant, _language);
		}
	}
}
