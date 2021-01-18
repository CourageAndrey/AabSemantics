using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels
{
	public class IsStatement : IViewModel
	{
		#region Properties

		public ConceptItem Parent
		{ get; set; }

		public ConceptItem Child
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement()
			: this(null as ConceptItem, null)
		{ }

		public IsStatement(Core.Statements.IsStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Parent, language), new ConceptItem(statement.Child, language))
		{
			_boundObject = statement;
		}

		public IsStatement(ConceptItem parent, ConceptItem child)
		{
			Parent = parent;
			Child = child;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.IsStatement _boundObject;

		public Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
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
				Title = language.StatementNames.Clasification,
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
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.IsStatement(Parent.Concept, Child.Concept));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Parent.Concept, Child.Concept);
		}

		#endregion
	}
}
