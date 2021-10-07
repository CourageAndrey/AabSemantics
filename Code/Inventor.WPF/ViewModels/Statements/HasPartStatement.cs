using System.Windows;

using Inventor.WPF.Controls;
using Inventor.WPF.Dialogs;
using Inventor.Core;
using Inventor.Set.Localization;

namespace Inventor.WPF.ViewModels.Statements
{
	public class HasPartStatement : StatementViewModel<Set.Statements.HasPartStatement>
	{
		#region Properties

		public ConceptItem Whole
		{ get; set; }

		public ConceptItem Part
		{ get; set; }

		#endregion

		#region Constructors

		public HasPartStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		public HasPartStatement(Set.Statements.HasPartStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Whole, language), new ConceptItem(statement.Part, language), language)
		{
			BoundObject = statement;
		}

		public HasPartStatement(string id, ConceptItem whole, ConceptItem part, ILanguage language)
			: base(id, language)
		{
			Whole = whole;
			Part = part;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new HasPartStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageSetModule>().Statements.Names.Composition,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Set.Statements.HasPartStatement CreateStatementImplementation()
		{
			return new Set.Statements.HasPartStatement(ID, Whole.Concept, Part.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Whole.Concept, Part.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new HasPartStatement(ID, Whole, Part, _language);
		}
	}
}
