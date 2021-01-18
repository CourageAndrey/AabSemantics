using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;

namespace Inventor.Client.ViewModels
{
	public class IsStatement : IViewModel
	{
		#region Properties

		public Core.IConcept Parent
		{ get; set; }

		public Core.IConcept Child
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement()
			: this(null, null)
		{ }

		public IsStatement(Core.Statements.IsStatement statement)
			: this(statement.Parent, statement.Child)
		{
			_boundObject = statement;
		}

		public IsStatement(Core.IConcept parent, Core.IConcept child)
		{
			Parent = parent;
			Child = child;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.IsStatement _boundObject;

		public Window CreateEditDialog(Window owner, Core.IKnowledgeBase knowledgeBase, Core.ILanguage language)
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
				Title = "Is",
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			return dialog;
		}

		public void ApplyCreate(Core.IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.IsStatement(Parent, Child));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Parent, Child);
		}

		#endregion
	}
}
