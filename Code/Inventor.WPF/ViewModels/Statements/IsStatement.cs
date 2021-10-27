using System.Windows;

using Inventor.WPF.Controls;
using Inventor.WPF.Dialogs;
using Inventor.Semantics;
using Inventor.Semantics.Localization.Modules;

namespace Inventor.WPF.ViewModels.Statements
{
	public class IsStatement : StatementViewModel<Semantics.Statements.IsStatement>
	{
		#region Properties

		public ConceptItem Ancestor
		{ get; set; }

		public ConceptItem Descendant
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		public IsStatement(Semantics.Statements.IsStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Ancestor, language), new ConceptItem(statement.Descendant, language), language)
		{
			BoundObject = statement;
		}

		public IsStatement(string id, ConceptItem ancestor, ConceptItem descendant, ILanguage language)
			: base(id, language)
		{
			Ancestor = ancestor;
			Descendant = descendant;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new IsStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageClassificationModule>().Statements.Names.Clasification,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Semantics.Statements.IsStatement CreateStatementImplementation()
		{
			return new Semantics.Statements.IsStatement(ID, Ancestor.Concept, Descendant.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Ancestor.Concept, Descendant.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new IsStatement(ID, Ancestor, Descendant, _language);
		}
	}
}
