using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class GroupStatement : StatementViewModel<Core.Statements.GroupStatement>
	{
		#region Properties

		public ConceptItem Area
		{ get; set; }

		public ConceptItem Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement()
			: this(null as ConceptItem, null)
		{ }

		public GroupStatement(Core.Statements.GroupStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Area, language), new ConceptItem(statement.Concept, language))
		{
			_boundObject = statement;
		}

		public GroupStatement(ConceptItem area, ConceptItem concept)
		{
			Area = area;
			Concept = concept;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new GroupStatementControl
			{
				EditValue = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.SubjectArea,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.GroupStatement CreateStatementImplementation()
		{
			return new Core.Statements.GroupStatement(Area.Concept, Concept.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(Area.Concept, Concept.Concept);
		}

		#endregion
	}
}
