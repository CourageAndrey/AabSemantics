using System.Windows;

using Inventor.Semantics.Modules.Set.Localization;
using Inventor.Semantics.Modules.WPF.Controls;
using Inventor.Semantics.Modules.WPF.Dialogs;

namespace Inventor.Semantics.Modules.WPF.ViewModels.Statements
{
	public class HasSignStatement : StatementViewModel<Semantics.Modules.Set.Statements.HasSignStatement>
	{
		#region Properties

		public ConceptItem Concept
		{ get; set; }

		public ConceptItem Sign
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		public HasSignStatement(Semantics.Modules.Set.Statements.HasSignStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Concept, language), new ConceptItem(statement.Sign, language), language)
		{
			BoundObject = statement;
		}

		public HasSignStatement(string id, ConceptItem concept, ConceptItem sign, ILanguage language)
			: base(id, language)
		{
			Concept = concept;
			Sign = sign;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new HasSignStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageSetModule>().Statements.Names.HasSign,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Semantics.Modules.Set.Statements.HasSignStatement CreateStatementImplementation()
		{
			return new Semantics.Modules.Set.Statements.HasSignStatement(ID, Concept.Concept, Sign.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Concept.Concept, Sign.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new HasSignStatement(ID, Concept, Sign, _language);
		}
	}
}
