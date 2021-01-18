using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels
{
	public class GroupStatement : IViewModel
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

		private Core.Statements.GroupStatement _boundObject;

		public Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
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

		public void ApplyCreate(IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.GroupStatement(Area.Concept, Concept.Concept));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Area.Concept, Concept.Concept);
		}

		#endregion
	}
}
