using System.Windows;

using Inventor.Client.UI;

namespace Inventor.Client.ViewModels
{
	public class ConsistsOfStatement : IViewModel
	{
		#region Properties

		public Core.IConcept Parent
		{ get; set; }

		public Core.IConcept Child
		{ get; set; }

		#endregion

		#region Constructors

		public ConsistsOfStatement()
			: this(null, null)
		{ }

		public ConsistsOfStatement(Core.Statements.ConsistsOfStatement statement)
			: this(statement.Parent, statement.Child)
		{
			_boundObject = statement;
		}

		public ConsistsOfStatement(Core.IConcept parent, Core.IConcept child)
		{
			Parent = parent;
			Child = child;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.ConsistsOfStatement _boundObject;

		public Window CreateEditDialog(Window owner, Core.IKnowledgeBase knowledgeBase, Core.ILanguage language)
		{
			var dialog = new ConsistsOfStatementDialog
			{
				Owner = owner,
				EditValue = this,
			};
			dialog.Initialize(knowledgeBase, language);
			return dialog;
		}

		public void ApplyCreate(Core.IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.ConsistsOfStatement(Parent, Child));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Parent, Child);
		}

		#endregion
	}
}
