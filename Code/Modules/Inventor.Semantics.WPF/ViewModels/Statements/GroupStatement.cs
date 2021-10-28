using System.Windows;

using Inventor.WPF.Controls;
using Inventor.WPF.Dialogs;
using Inventor.Semantics;
using Inventor.Semantics.Set.Localization;

namespace Inventor.WPF.ViewModels.Statements
{
	public class GroupStatement : StatementViewModel<Semantics.Set.Statements.GroupStatement>
	{
		#region Properties

		public ConceptItem Area
		{ get; set; }

		public ConceptItem Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		public GroupStatement(Semantics.Set.Statements.GroupStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Area, language), new ConceptItem(statement.Concept, language), language)
		{
			BoundObject = statement;
		}

		public GroupStatement(string id, ConceptItem area, ConceptItem concept, ILanguage language)
			: base(id, language)
		{
			Area = area;
			Concept = concept;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new GroupStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageSetModule>().Statements.Names.SubjectArea,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Semantics.Set.Statements.GroupStatement CreateStatementImplementation()
		{
			return new Semantics.Set.Statements.GroupStatement(ID, Area.Concept, Concept.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Area.Concept, Concept.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new GroupStatement(ID, Area, Concept, _language);
		}
	}
}
