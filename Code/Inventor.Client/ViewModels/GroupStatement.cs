using System.Windows;

using Inventor.Client.Dialogs;

namespace Inventor.Client.ViewModels
{
	public class GroupStatement : IViewModel
	{
		#region Properties

		public Core.IConcept Area
		{ get; set; }

		public Core.IConcept Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement()
			: this(null, null)
		{ }

		public GroupStatement(Core.Statements.GroupStatement statement)
			: this(statement.Area, statement.Concept)
		{
			_boundObject = statement;
		}

		public GroupStatement(Core.IConcept area, Core.IConcept concept)
		{
			Area = area;
			Concept = concept;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.GroupStatement _boundObject;

		public Window CreateEditDialog(Window owner, Core.IKnowledgeBase knowledgeBase, Core.ILanguage language)
		{
			var dialog = new GroupStatementDialog
			{
				Owner = owner,
				EditValue = this,
			};
			dialog.Initialize(knowledgeBase, language);
			return dialog;
		}

		public void ApplyCreate(Core.IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.GroupStatement(Area, Concept));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Area, Concept);
		}

		#endregion
	}
}
